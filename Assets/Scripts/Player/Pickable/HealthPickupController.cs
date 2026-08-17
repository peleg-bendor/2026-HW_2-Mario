using UnityEngine;

// A heart lying in the level. Same shape as every other pickup controller: detect Mario, hand
// over a power-up, disappear.
public class HealthPickupController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameLog.Info(LogCategory.Pickup, "Health pickup collected");
            this.gameObject.SetActive(false);
            col.gameObject.GetComponent<PlayerPowerUp>().CollectPowerUp(new HealthPowerUp());
        }
    }
}
