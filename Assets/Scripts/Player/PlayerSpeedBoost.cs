using System.Collections;
using UnityEngine;

// Runs Mario's temporary speed boost - owns the timer and writes PlayerMovement.speed directly.
// Not a general speed-modifier system: there is only one effect to coordinate, so it writes the
// field it needs rather than exposing a multiplier for something else to read.
public class PlayerSpeedBoost : MonoBehaviour
{
    [SerializeField] private float boostDuration = 5f;
    [SerializeField] private float boostFraction = 0.5f;

    private PlayerMovement movement;
    private PlayerInvincible playerInvincible;
    private float baseSpeed;
    private Coroutine boostRoutine;

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
        movement = GetComponent<PlayerMovement>();
        playerInvincible = GetComponent<PlayerInvincible>();

        if (movement == null)
            GameLog.Warning(LogCategory.Player, "No PlayerMovement found, the speed boost will do nothing");
        else
            baseSpeed = movement.speed;
    }

    public void ActivateBoost()
    {
        // A second bolt restarts the timer rather than leaving two runs going, the same fix
        // PlayerInvincible makes for a second star.
        if (boostRoutine != null)
        {
            StopCoroutine(boostRoutine);
            GameLog.Info(LogCategory.Player, "Speed boost restarted");
        }
        else
        {
            GameLog.Info(LogCategory.Player, "Speed boost activated");
        }

        boostRoutine = StartCoroutine(BoostCoroutine());
    }

    private IEnumerator BoostCoroutine()
    {
        if (movement != null)
            movement.speed = baseSpeed * (1f + boostFraction);

        yield return new WaitForSeconds(boostDuration);

        RestoreSpeed();
        GameLog.Info(LogCategory.Player, "Speed boost ended");
    }

    private void OnHazardCollision()
    {
        // No PlayerInvincible on this object is treated as never invincible, matching
        // PlayerDeath's and HealthController's fallback for the same optional reference.
        if (playerInvincible != null && playerInvincible.IsInvincible)
            return;

        if (boostRoutine == null)
            return;

        StopCoroutine(boostRoutine);
        RestoreSpeed();
        GameLog.Info(LogCategory.Player, "Speed boost cancelled by hazard hit");
    }

    // Always restores to the speed captured once in Awake rather than dividing the current
    // value back down, so nothing here can drift or compound across a restart or a cancel.
    private void RestoreSpeed()
    {
        if (movement != null)
            movement.speed = baseSpeed;

        boostRoutine = null;
    }
}
