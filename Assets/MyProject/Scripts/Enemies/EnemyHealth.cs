using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Enemy _enemy;

    void Start()
    {
        currentHealth = maxHealth;
        _enemy = GetComponentInParent<Enemy>();

        if (_enemy == null)
        {
            Debug.LogError($"❌ {gameObject.name}: Enemy 스크립트를 찾을 수 없습니다! 계층 구조를 확인해주세요.");
        }
    }

    public void TakeDamage(float amount)
    {
        // 🚨 수정된 부분: 체력 검사를 여기서 하지 말고, Enemy 스크립트의 상태를 믿으세요.
        // 만약 Enemy 스크립트가 "나 이미 죽은 상태야(IsDead)"라고 하면 그때 무시하는 게 안전합니다.

        if (_enemy != null && _enemy.IsDead) return;

        currentHealth -= amount;

        // 체력이 0이 되어도, 일단 맞았다는 신호는 무조건 보내야 합니다!
        // (왜냐? 순서가 틀려서 안 죽고 버티고 있는 상태일 수도 있으니까요)
        if (_enemy != null)
        {
            _enemy.OnHit();
        }
        else
        {
            Debug.LogWarning($"⚠ {gameObject.name}가 맞았지만, 연결된 Enemy 스크립트가 없습니다.");
        }
    }

    public void Die()
    {
        if (_enemy != null)
        {
            _enemy.DieSuccess();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}