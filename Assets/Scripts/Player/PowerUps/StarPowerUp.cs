using UnityEngine;

// Grants Mario temporary invincibility. Looks up PlayerInvincible directly rather than through
// an interface - unlike AxePowerUp/FireFlowerPowerUp, there's no second implementer yet to
// justify segregating a capability out of it.
public class StarPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        if (player != null)
        {
            PlayerInvincible invincible = player.GetComponent<PlayerInvincible>();
            if (invincible != null)
            {
                invincible.ActivateInvincibility();
            }
        }
    }
}
