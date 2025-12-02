using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
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
        Debug.Log($"Player took {dmg} damage, current HP: {HP}");
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            // 현재 HP 값으로 슬라이더의 Value를 설정
            healthSlider.value = HP;
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
}
