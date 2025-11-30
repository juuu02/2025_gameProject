using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHP = 100f;
    public float HP = 100f;

    public delegate void PlayerDeathHandler();
    public event PlayerDeathHandler OnPlayerDeath;

    private bool _isDead = false;

    void Start()
    {
        HP = MaxHP;
    }

    public void TakeDamage(float dmg)
    {
        if (_isDead) return;

        HP -= dmg;
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
}
