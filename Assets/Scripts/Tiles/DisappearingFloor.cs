using System.Collections;
using UnityEngine;

// Cycles a floor tile between solid and passable on a fixed timer, from scene load rather than on
// Mario's touch. Toggles the SpriteRenderer and Collider2D rather than the GameObject, since a
// disabled GameObject would stop running this coroutine and the tile would never come back.
public class DisappearingFloor : MonoBehaviour
{
    [SerializeField] private float visibleDuration = 2f;
    [SerializeField] private float hiddenDuration = 2f;

    private SpriteRenderer spriteRenderer;
    private Collider2D floorCollider;

    // Every tile's coroutine starts on the same frame and waits the same durations, so their
    // Show()/Hide() calls land on the same frame too - this dedupes the log to one line per
    // collective transition instead of one per tile.
    private static int lastLoggedShowFrame = -1;
    private static int lastLoggedHideFrame = -1;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        floorCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null)
            GameLog.Warning(LogCategory.Tile, "No SpriteRenderer found, the tile will never visibly vanish");

        if (floorCollider == null)
            GameLog.Warning(LogCategory.Tile, "No Collider2D found, the tile will always block Mario");
    }

    void Start()
    {
        StartCoroutine(CycleVisibility());
    }

    private IEnumerator CycleVisibility()
    {
        while (true)
        {
            Show();
            yield return new WaitForSeconds(visibleDuration);

            Hide();
            yield return new WaitForSeconds(hiddenDuration);
        }
    }

    private void Show()
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (floorCollider != null)
            floorCollider.enabled = true;

        if (Time.frameCount != lastLoggedShowFrame)
        {
            lastLoggedShowFrame = Time.frameCount;
            GameLog.Verbose(LogCategory.Tile, "Disappearing floor tiles appeared");
        }
    }

    private void Hide()
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (floorCollider != null)
            floorCollider.enabled = false;

        if (Time.frameCount != lastLoggedHideFrame)
        {
            lastLoggedHideFrame = Time.frameCount;
            GameLog.Verbose(LogCategory.Tile, "Disappearing floor tiles vanished");
        }
    }
}
