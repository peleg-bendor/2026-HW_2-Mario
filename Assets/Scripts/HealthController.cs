using UnityEngine;

// Wires HealthModel to HealthView and to the game events that change health. Subscribes to
// hazard hits and the health pickup's event, applies the change to the model, and pushes the
// result to the view. Also owns the game-over signal GameEndManager listens for.
public class HealthController : MonoBehaviour
{
    public static event System.Action OnGameOver;

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private HealthView healthView;

    // Consulted only to skip a loss while Mario's invincible - looked up by tag rather than
    // assigned by hand, so rebuilding the level can replace Mario without leaving this pointing
    // at a component that no longer exists.
    private PlayerInvincible playerInvincible;

    private IHealthModel model;

    private void OnEnable()
    {
        SC_Death.OnHazardCollision += OnHazardCollision;
        HealthPowerUp.OnHealthGained += OnHealthGained;
    }

    private void OnDisable()
    {
        SC_Death.OnHazardCollision -= OnHazardCollision;
        HealthPowerUp.OnHealthGained -= OnHealthGained;
    }

    private void Awake()
    {
        model = new HealthModel(maxHealth);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerInvincible = player.GetComponent<PlayerInvincible>();

        if (healthView == null)
            GameLog.Warning(LogCategory.Player, "No HealthView assigned, health will not be drawn");
    }

    private void Start()
    {
        if (healthView != null)
            healthView.ShowHealth(model.CurrentHealth);
    }

    private void OnHazardCollision()
    {
        // No PlayerInvincible found is treated as never invincible, not an error - matches
        // PlayerDeath's own fallback for the same optional reference.
        if (playerInvincible != null && playerInvincible.IsInvincible)
            return;

        if (!model.Lose())
            return;

        GameLog.Info(LogCategory.Player, "Health lost - " + model.CurrentHealth + " remaining");

        if (healthView != null)
            healthView.ShowHealth(model.CurrentHealth);

        if (model.CurrentHealth <= 0)
            OnGameOver?.Invoke();
    }

    private void OnHealthGained()
    {
        if (!model.Gain())
        {
            GameLog.Info(LogCategory.Player, "Health pickup ignored - already at max (" + model.MaxHealth + ")");
            return;
        }

        GameLog.Info(LogCategory.Player, "Health gained - " + model.CurrentHealth + " remaining");

        if (healthView != null)
            healthView.ShowHealth(model.CurrentHealth);
    }
}
