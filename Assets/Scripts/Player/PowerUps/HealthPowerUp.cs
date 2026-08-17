using UnityEngine;

// Grants Mario an extra point of health. Unlike the other power-ups it ignores the player
// argument entirely: health lives on HealthController, a scene-level manager, so there is
// nothing on Mario to reach into. It announces the gain instead and lets the controller apply it.
public class HealthPowerUp : IPowerUp
{
    public static event System.Action OnHealthGained;

    public void ApplyPowerUp(GameObject player)
    {
        OnHealthGained?.Invoke();
    }
}
