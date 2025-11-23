using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Enemy _enemy;

    void Start()
    {
        currentHealth = maxHealth;
        _enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // 🔥 총알 맞았다는 “신호”만 Enemy에게 보냄
        if (_enemy != null)
            _enemy.OnHit();

        // ❗ 체력으로 죽으면 안 됨
        // EnemyManager가 정답인지 판단해서 죽임
    }

    // 정답일 때만 Enemy.cs에서 호출되는 함수
    public void Die()
    {
        if (_enemy != null)
        {
            _enemy.DieSuccess();
        }
        else
        {
            Destroy(gameObject); // 혹시 Enemy 컴포넌트 없으면 백업
        }
    }
}
