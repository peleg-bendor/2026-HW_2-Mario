using System.Collections;
using UnityEngine;

// Runs Mario's temporary invincibility - owns the flag, the timer, and the visual cue.
// Coroutine-driven on purpose (see EnemySpawner for the Task-based version of the same
// "wait then flip something back" shape, done the other way).
public class PlayerInvincible : MonoBehaviour
{
    [SerializeField] private float powerUpDuration = 5f;
    [SerializeField] private Color invincibleColor = new Color(1f, 0.84f, 0f);

    public bool IsInvincible { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Color baseColor;
    private Coroutine invincibilityRoutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            GameLog.Warning(LogCategory.Player, "No SpriteRenderer found, the invincibility tint will do nothing");
        else
            baseColor = spriteRenderer.color;
    }

    public void ActivateInvincibility()
    {
        // A second star restarts the timer rather than leaving two runs going. Two of them would
        // both end on their own schedule, so the earlier one would cut the effect short instead
        // of the later one extending it.
        if (invincibilityRoutine != null)
        {
            StopCoroutine(invincibilityRoutine);
            GameLog.Info(LogCategory.Player, "Invincibility restarted");
        }
        else
        {
            GameLog.Info(LogCategory.Player, "Invincibility activated");
        }

        invincibilityRoutine = StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        IsInvincible = true;

        if (spriteRenderer != null)
            spriteRenderer.color = invincibleColor;

        yield return new WaitForSeconds(powerUpDuration);

        if (spriteRenderer != null)
            spriteRenderer.color = baseColor;

        IsInvincible = false;
        invincibilityRoutine = null;
        GameLog.Info(LogCategory.Player, "Invincibility ended");
    }
}
