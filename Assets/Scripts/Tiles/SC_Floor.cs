using UnityEngine;

// Marks a GameObject as a floor tile. Doubles as the allowlist the rest of the project uses to
// tell real terrain from everything else, which is how projectiles and the jump check exclude
// pickups, a landed axe and an enemy's head without naming any of them. Also exposes the
// landed-on-top-vs-bumped-the-side geometry check as a static method, for MovingFloor to reuse.
public class SC_Floor : MonoBehaviour
{
    public delegate void FloorCollisionHandler();
    public static event FloorCollisionHandler OnFloorCollision;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (IsAboveTile(col, transform))
            {
                // Raised for every tile Mario steps onto, not only real landings, since each
                // tile is its own collider. Whether it counts as a landing is PlayerJump's call,
                // because that is what tracks jump state.
                if (OnFloorCollision != null)
                    OnFloorCollision();
            }
            else
            {
                Debug.Log("Mario touched floor tile from the side");
            }
        }
    }

    // Whether the other side of this collision rests on top of the tile rather than bumping its
    // side - its centre clears the tile's own by roughly its own collider's half-height. Read live
    // off that collider so it survives the other object ever being resized. Shared by this class's
    // own landing event and MovingFloor's rider check, which need the identical geometry test.
    public static bool IsAboveTile(Collision2D col, Transform tileTransform)
    {
        float otherY = col.gameObject.transform.position.y;
        float tileY = tileTransform.position.y;
        float otherColliderHalfHeight = col.collider.bounds.extents.y;

        return otherY > tileY + otherColliderHalfHeight;
    }
}
