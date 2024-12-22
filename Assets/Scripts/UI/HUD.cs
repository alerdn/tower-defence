using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image _healthBar;

    private void Start()
    {
        PlayerController.Instance.OnHealthUpdated += UpdateHealthBar;
    }

    private void UpdateHealthBar(int health, int maxHealth)
    {
        _healthBar.fillAmount = (float)health / (float)maxHealth;
    }
}