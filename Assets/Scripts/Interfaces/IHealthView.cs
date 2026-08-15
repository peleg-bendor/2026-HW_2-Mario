// Draws Mario's current health. Knows nothing about hazards or pickups - only how to
// show a number.
public interface IHealthView
{
    void ShowHealth(int currentHealth);
}
