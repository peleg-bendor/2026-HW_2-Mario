// Owns Mario's health count and the rules for changing it. No Unity types - HealthController
// wraps this and does the Unity-facing wiring.
public interface IHealthModel
{
    int CurrentHealth { get; }
    int MaxHealth { get; }

    // Returns whether the count actually changed, so a caller can tell an ignored change
    // (already at max, already at zero) from a real one without re-reading the value.
    bool Gain();
    bool Lose();
}
