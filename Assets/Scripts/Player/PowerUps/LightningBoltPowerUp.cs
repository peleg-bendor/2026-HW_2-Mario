using UnityEngine;

// Grants Mario a temporary speed boost. Looks up PlayerSpeedBoost directly rather than through
// an interface, matching StarPowerUp - no second implementer yet to justify segregating a
// capability out of it.
public class LightningBoltPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        if (player != null)
        {
            Debug.Log("LightningBoltPowerUp applied to " + player.name);
            PlayerSpeedBoost speedBoost = player.GetComponent<PlayerSpeedBoost>();
            if (speedBoost != null)
            {
                speedBoost.ActivateBoost();
            }
        }
    }
}
