using UnityEngine;

// Patrols a floor tile between its home cell and travelDistance to the right, forever, carrying
// anything resting on top along for the ride. Movement goes through a Kinematic Rigidbody2D rather
// than a Transform write, since this is the only floor tile whose collider actually moves.
public class MovingFloor : MonoBehaviour
{
    [SerializeField] private float travelDistance = 2f;
    [SerializeField] private float speed = 1f;

    private Rigidbody2D rigid;
    private Vector2 homePosition;
    private bool movingToFarSide = true;

    // This step's own movement, read by OnCollisionStay2D to shift whatever is resting on top -
    // set fresh in FixedUpdate before Unity's physics step runs the collision callbacks that use it.
    private Vector2 frameDelta;

    // Static so the guard is shared: tiles that share a speed reach a turn on the same frame, and
    // this keeps the log to one line per collective turn instead of one per tile.
    private static int lastLoggedFarSideFrame = -1;
    private static int lastLoggedHomeFrame = -1;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        if (rigid == null)
        {
            GameLog.Warning(LogCategory.Tile, "No Rigidbody2D found, the tile will never move");
            return;
        }

        homePosition = rigid.position;
    }

    void FixedUpdate()
    {
        if (rigid == null)
            return;

        Vector2 target = movingToFarSide ? homePosition + Vector2.right * travelDistance : homePosition;
        Vector2 previousPosition = rigid.position;
        Vector2 newPosition = Vector2.MoveTowards(previousPosition, target, speed * Time.fixedDeltaTime);

        frameDelta = newPosition - previousPosition;
        rigid.MovePosition(newPosition);

        if (newPosition == target)
        {
            movingToFarSide = !movingToFarSide;
            LogTurn();
        }
    }

    private void LogTurn()
    {
        if (movingToFarSide)
        {
            if (Time.frameCount == lastLoggedFarSideFrame)
                return;

            lastLoggedFarSideFrame = Time.frameCount;
            GameLog.Verbose(LogCategory.Tile, "Moving floor tiles turned toward the far side");
        }
        else
        {
            if (Time.frameCount == lastLoggedHomeFrame)
                return;

            lastLoggedHomeFrame = Time.frameCount;
            GameLog.Verbose(LogCategory.Tile, "Moving floor tiles turned back toward home");
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (frameDelta == Vector2.zero || col.rigidbody == null)
            return;

        if (SC_Floor.IsAboveTile(col, transform))
            col.rigidbody.position += frameDelta;
    }
}
