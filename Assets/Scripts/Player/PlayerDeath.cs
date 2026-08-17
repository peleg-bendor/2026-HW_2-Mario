using UnityEngine;

// Puts Mario back where the level started him whenever a hazard hits him, unless he's currently
// invincible. Deliberately counts nothing: HealthController subscribes to the same event
// independently and owns whether that hit was the last one.
public class PlayerDeath : MonoBehaviour
{
    private Vector3 startPosition;
    private PlayerInvincible playerInvincible;

    private void OnEnable()
    {
        SC_Death.OnHazardCollision += OnHazardCollision;
    }

    private void OnDisable()
    {
        SC_Death.OnHazardCollision -= OnHazardCollision;
    }

    void Awake()
    {
        // Captured from wherever Mario actually sits at load rather than hardcoded, so moving
        // his starting position in the Editor moves the respawn point with it.
        startPosition = transform.position;
        playerInvincible = GetComponent<PlayerInvincible>();
    }

    private void OnHazardCollision()
    {
        // No PlayerInvincible on this object is treated as never invincible, not an error - the
        // component is optional, not something every hazard-affected object needs.
        if (playerInvincible != null && playerInvincible.IsInvincible)
            return;

        transform.position = startPosition;
        GameLog.Info(LogCategory.Player, "Mario respawned at start position");
    }
}
