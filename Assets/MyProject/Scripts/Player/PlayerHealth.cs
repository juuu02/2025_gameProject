using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Settings")]
    public float MaxHP = 100f;
    public float HP = 100f;

    public delegate void PlayerDeathHandler();
    public event PlayerDeathHandler OnPlayerDeath;

    private bool _isDead = false;

    void Start()
    {
        HP = MaxHP;
    }

    // 🔥 보스 or 다른 공격자가 이 함수를 호출해서 데미지를 줌
    public void TakeDamage(float dmg)
    {
        if (_isDead) return;

        HP -= dmg;
        Debug.Log($"💥 플레이어 피격! 남은 HP: {HP}");

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

        Debug.Log("💀 플레이어 사망!");

        // BossManager에서 처리할 수 있도록 이벤트 발생
        OnPlayerDeath?.Invoke();
    }
}
