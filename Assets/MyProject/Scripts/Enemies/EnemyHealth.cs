using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Enemy _enemy;
    private EnemyManager _manager;

    void Start()
    {
        currentHealth = maxHealth;

        _enemy = GetComponent<Enemy>();
        _manager = FindFirstObjectByType<EnemyManager>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // 🔥 총을 맞으면 Enemy.OnHit() 호출
        if (_enemy != null)
            _enemy.OnHit();

        // ❗ 체력 기반으로 죽으면 안 됨.
        //    퍼즐 매니저가 죽일지 말지 결정함.
        //    Die() 호출 금지.
    }

    // ⭐ 색상 순서가 맞아서 죽는 경우에만 호출됨
    public void Die()
    {
        if (_enemy != null)
        {
            _enemy.DieSuccess();
        }
        else
        {
            Destroy(gameObject); // 백업
        }
    }
}
