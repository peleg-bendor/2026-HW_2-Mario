using UnityEngine;
using UnityEngine.InputSystem;

// Mario's jump, single and double. Split from PlayerMovement because it owns state that outlives
// a frame - how many jumps have been spent since the last landing - which horizontal movement
// never needs. The ground check itself is not here: it lives in GroundedExtension, so anything
// else that needs to ask can, without this class exposing it.
public class PlayerJump : MonoBehaviour
{
    public float jumpSpeed = 100;

    // How far below Mario's feet to look for a floor tile.
    [SerializeField] private float groundProbeDepth = 0.2f;

    [SerializeField] private int maxJumps = 2;

    private int jumpsUsed = 0;

    private Rigidbody2D rigid;
    private Collider2D bodyCollider;

    private void OnEnable()
    {
        SC_Floor.OnFloorCollision += OnFloorCollision;
    }

    private void OnDisable()
    {
        SC_Floor.OnFloorCollision -= OnFloorCollision;
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();

        if (rigid == null)
            GameLog.Warning(LogCategory.Player, "No Rigidbody2D found, jumping will do nothing");

        if (bodyCollider == null)
            GameLog.Warning(LogCategory.Player, "No Collider2D found, Mario will always read as airborne");
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            Jump();
    }

    private void OnFloorCollision()
    {
        // SC_Floor raises this on every tile Mario walks onto, so only a real transition -
        // jumps spent, now back on the ground - is worth acting on.
        if (jumpsUsed > 0)
        {
            GameLog.Verbose(LogCategory.Player, "Mario landed on floor");
            jumpsUsed = 0;
        }
    }

    private void Jump()
    {
        bool inAir = bodyCollider.IsInAir(groundProbeDepth);

        // What makes the second jump conditional on being airborne. In the air with nothing spent
        // means Mario walked off a ledge rather than jumped, so the ground jump is charged here
        // instead of given away, and the only jump left to him is the double jump.
        if (inAir && jumpsUsed == 0)
            jumpsUsed = 1;

        if (jumpsUsed >= maxJumps)
        {
            GameLog.Info(LogCategory.Player, "Jump ignored - Mario has to land before jumping again");
            return;
        }

        if (rigid == null)
            return;

        // Cleared rather than added to, so the impulse is the whole launch. A double jump taken
        // mid-fall would otherwise start from that fall's velocity and reach about half the
        // height of one taken at the apex, off the same key press.
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, 0f);
        rigid.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);

        jumpsUsed++;
        GameLog.Info(LogCategory.Player, "Mario jumped (" + jumpsUsed + " of " + maxJumps + ")");
    }
}
