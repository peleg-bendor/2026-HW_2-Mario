using System.Collections;
using UnityEngine;

// Runs Mario's temporary invincibility - owns the flag, the timer, and the visual cue.
// Coroutine-driven on purpose (see EnemySpawner for the Task-based version of the same
// "wait then flip something back" shape, done the other way).
public class PlayerInvincible : MonoBehaviour
{
    [SerializeField] private float powerUpDuration = 10f;
    [SerializeField] private Color invincibleColor = new Color(1f, 0.84f, 0f);

    public bool IsInvincible { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Color baseColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            Debug.LogWarning("PlayerInvincible: no SpriteRenderer found, the invincibility tint will do nothing");
        else
            baseColor = spriteRenderer.color;
    }

    public void ActivateInvincibility()
    {
        Debug.Log("Invincibility activated for " + gameObject.name);
        StartCoroutine(InvincibilityCoroutine());
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
        Debug.Log("Invincibility ended for " + gameObject.name);
    }
}
