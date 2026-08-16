using UnityEngine;

// Patrols a floor tile between its home cell and travelDistance world units to the right, forever,
// and carries anything resting on top of it along for the ride. Movement lives on a Kinematic
// Rigidbody2D moved with MovePosition rather than a plain Transform write, since this is the only
// floor tile whose collider actually moves. No coordinator - each instance patrols on its own.
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

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        if (rigid == null)
        {
            Debug.LogWarning("MovingFloor: no Rigidbody2D found, the tile will never move");
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
            Debug.Log(movingToFarSide ? "Cloud tile turned back toward the far side" : "Cloud tile turned back toward home");
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
