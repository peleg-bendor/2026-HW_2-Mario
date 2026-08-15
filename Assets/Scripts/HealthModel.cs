// Plain C# implementation of IHealthModel - no Unity types, so it can be constructed without a
// scene. HealthController owns the one instance and is the only thing that touches it.
public class HealthModel : IHealthModel
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

    public HealthModel(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public bool Gain()
    {
        if (CurrentHealth >= MaxHealth)
            return false;

        CurrentHealth++;
        return true;
    }

    public bool Lose()
    {
        if (CurrentHealth <= 0)
            return false;

        CurrentHealth--;
        return true;
    }
}
