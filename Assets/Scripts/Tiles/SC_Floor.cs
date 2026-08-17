using UnityEngine;

// Marks a GameObject as a floor tile, and so doubles as the allowlist the rest of the project uses
// to tell real terrain from everything else - which is how projectiles and the jump check exclude
// pickups, a landed axe and an enemy's head without naming any of them.
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
                GameLog.Verbose(LogCategory.Tile, "Mario touched floor tile from the side");
            }
        }
    }

    // Public and static because MovingFloor's rider check needs the identical landed-on-top test
    // the landing event above uses, and one copy of the geometry is worth more than two.
    public static bool IsAboveTile(Collision2D col, Transform tileTransform)
    {
        float otherY = col.gameObject.transform.position.y;
        float tileY = tileTransform.position.y;
        float otherColliderHalfHeight = col.collider.bounds.extents.y;

        return otherY > tileY + otherColliderHalfHeight;
    }
}
