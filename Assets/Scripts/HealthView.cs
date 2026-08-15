using TMPro;
using UnityEngine;

// Draws Mario's health onto a label. Owns no count of its own - HealthController tells it what
// to show.
public class HealthView : MonoBehaviour, IHealthView
{
    [SerializeField] private TextMeshProUGUI healthText;

    public void ShowHealth(int currentHealth)
    {
        if (healthText != null)
            healthText.text = "Health: " + currentHealth;
    }
}
