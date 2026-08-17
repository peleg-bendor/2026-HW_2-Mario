using UnityEngine;

// Whether a collider is standing on this game's terrain, as an extension so any script can ask
// without carrying a probe of its own. Not a general Unity helper: ground here means anything
// carrying SC_Floor, which is why there is no LayerMask parameter to hand it a different answer.
public static class GroundedExtension
{
    // Keeps the probe inside the collider's own silhouette. A full-width box would touch a wall
    // tile the collider is pressed flat against while falling, and read that as ground.
    private const float GroundProbeWidthFactor = 0.9f;

    // Sampled on demand rather than tracked every frame. The floor is many separate tile colliders
    // rather than one surface, which makes per-frame grounded tracking noisy. A single sample
    // carries no state that can go stale, so a wrong reading costs one call and nothing else.
    public static bool IsGrounded(this Collider2D collider, float probeDepth)
    {
        if (collider == null)
            return false;

        Bounds body = collider.bounds;

        // A box across the whole footprint rather than a circle under the centre. Standing on a
        // platform's last tile leaves the centre hanging past the tile edge while a round collider
        // is still perched on the corner, where a centre-only probe sees nothing.
        Vector2 probeSize = new Vector2(body.size.x * GroundProbeWidthFactor, probeDepth);
        Vector2 probeCenter = new Vector2(body.center.x, body.min.y - probeDepth * 0.5f);

        Collider2D[] hits = Physics2D.OverlapBoxAll(probeCenter, probeSize, 0f);

        foreach (Collider2D hit in hits)
        {
            // SC_Floor is what marks a tile as real terrain, which excludes the caller's own
            // collider, pickups, a landed axe and an enemy's head without naming any of them.
            if (hit != null && hit.GetComponent<SC_Floor>() != null)
                return true;
        }

        return false;
    }

    // Kept beside IsGrounded rather than replacing it. The probe is the primitive worth having;
    // this is the question the jump code asks, and neither is worth hiding behind the other.
    public static bool IsInAir(this Collider2D collider, float probeDepth)
    {
        return collider.IsGrounded(probeDepth) == false;
    }
}
