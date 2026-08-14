using UnityEngine;

// Owns how many strikes Mario has left, and nothing else. It doesn't move him, doesn't draw
// the number, and doesn't restart the game - it only counts, and says so when the count hits
// zero.
public class StrikesManager : MonoBehaviour
{
    public static event System.Action<int> OnStrikeCountChanged;

    // Raised the instant strikes hit zero. Nothing here reloads the scene, so this can fire
    // inline even though PlayerDeath subscribes to the same hazard event with no guaranteed
    // order against this one - there is no reload left for the two of them to race.
    public static event System.Action OnGameOver;

    [SerializeField] private int startingStrikes = 3;

    // Consulted only to skip a deduction while Mario's invincible - this manager doesn't
    // otherwise know or care that a player component exists. Looked up by tag rather than
    // assigned by hand, so rebuilding the level can replace Mario without leaving this
    // pointing at a component that no longer exists.
    private PlayerInvincible playerInvincible;

    private int strikesRemaining;

    private void OnEnable()
    {
        SC_Death.OnHazardCollision += OnHazardCollision;
        StrikePowerUp.OnStrikeGained += OnStrikeGained;
    }

    private void OnDisable()
    {
        SC_Death.OnHazardCollision -= OnHazardCollision;
        StrikePowerUp.OnStrikeGained -= OnStrikeGained;
    }

    private void Awake()
    {
        strikesRemaining = startingStrikes;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerInvincible = player.GetComponent<PlayerInvincible>();
    }

    private void Start()
    {
        OnStrikeCountChanged?.Invoke(strikesRemaining);
    }

    private void OnHazardCollision()
    {
        // Already at zero means the game is over and only waiting for the scene to reload,
        // which happens a frame later at the earliest. Physics keeps running until then, so
        // without this a hazard touched in that gap drives the count negative.
        if (strikesRemaining <= 0)
            return;

        // No PlayerInvincible found is treated as never invincible, not an error - matches
        // PlayerDeath's own fallback for the same optional reference.
        if (playerInvincible != null && playerInvincible.IsInvincible)
            return;

        strikesRemaining--;
        Debug.Log("Strike lost - " + strikesRemaining + " remaining");
        OnStrikeCountChanged?.Invoke(strikesRemaining);

        if (strikesRemaining <= 0)
            OnGameOver?.Invoke();
    }

    private void OnStrikeGained()
    {
        // Capped at the starting amount rather than a separate maximum, so the ceiling can't
        // drift away from the number Mario begins with.
        if (strikesRemaining >= startingStrikes)
        {
            Debug.Log("Strike pickup ignored - already at max (" + startingStrikes + ")");
            return;
        }

        strikesRemaining++;
        Debug.Log("Strike gained - " + strikesRemaining + " remaining");
        OnStrikeCountChanged?.Invoke(strikesRemaining);
    }
}
