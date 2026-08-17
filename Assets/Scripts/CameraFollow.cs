using UnityEngine;

// Keeps the camera centred on Mario: snapped onto him at load, smoothed from then on. Runs in
// LateUpdate so it reads his fully resolved position for the frame instead of trailing physics
// by a step.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0.15f;

    // Looked up by tag rather than assigned by hand, because Mario is created along with the
    // rest of the level - a reference wired in the Inspector would point at a destroyed object
    // the moment the level is built again.
    private Transform target;

    // Only x and y follow. Depth is read once at startup rather than hardcoded, so moving the
    // camera in the Editor doesn't silently leave a stale number in here.
    private float cameraZ;

    // SmoothDamp needs somewhere to keep its working velocity between calls. Nothing else
    // reads it.
    private Vector3 followVelocity;

    private void Awake()
    {
        cameraZ = transform.position.z;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            GameLog.Warning(LogCategory.Game, "No object tagged Player found, the camera will not follow");
            return;
        }

        target = player.transform;

        // Snapped rather than smoothed for the first frame, so the camera opens on Mario instead
        // of sliding in from wherever it was last saved. Every game over and every win reloads
        // the scene, so that slide would otherwise happen several times a session.
        transform.position = DesiredPosition();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = Vector3.SmoothDamp(transform.position, DesiredPosition(), ref followVelocity, smoothTime);
    }

    // Mario's world position already accounts for whatever his parent objects are offset by, so
    // following it needs no correction for how the level is nested.
    private Vector3 DesiredPosition()
    {
        return new Vector3(target.position.x, target.position.y, cameraZ);
    }
}
