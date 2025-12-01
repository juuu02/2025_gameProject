using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    [Header("PlayerHealth")]
    public float MaxHP = 100f;
    public float HP = 100f;

    [Header("UI Reference")]
    public Slider healthSlider;

    public delegate void PlayerDeathHandler();
    public event PlayerDeathHandler OnPlayerDeath;

    private bool _isDead = false;

    void Start()
    {
        HP = MaxHP;

        if (healthSlider != null)
        {
            healthSlider.maxValue = MaxHP;
            healthSlider.value = HP;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (_isDead) return;

        HP -= dmg;
        HP = Mathf.Max(HP, 0f);
        UpdateHealthUI();

        if (HP <= 0)
        {
            Die();
        }


    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;
        HP = 0;

        OnPlayerDeath?.Invoke();

        var rm = FindFirstObjectByType<RoundManager>();
        if (rm != null)
            rm.TriggerGameOver();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = HP;
        }
    }
}
