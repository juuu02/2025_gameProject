using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField]
    public int ColorIndex;   // Inspector에서 보이고, 자동 할당됨

    public int OrderIndex;
    public bool IsDead = false;

    private EnemyManager _manager;

    private void Awake()
    {
        _manager = FindFirstObjectByType<EnemyManager>();

        // 🔥 이름 기반 ColorIndex 자동 설정
        ApplyColorIndexByName();
    }

    // 이름을 기반으로 ColorIndex 자동 할당
    private void ApplyColorIndexByName()
    {
        string n = gameObject.name.ToLower();

        if (n.Contains("red")) ColorIndex = 0;
        else if (n.Contains("yellow")) ColorIndex = 1;
        else if (n.Contains("green")) ColorIndex = 2;
        else if (n.Contains("blue")) ColorIndex = 3;
        else
            Debug.LogWarning($"⚠ {gameObject.name}: 색상 이름을 알 수 없습니다. 직접 ColorIndex를 설정하세요!");
    }

    public void ResetEnemy()
    {
        IsDead = false;
        gameObject.SetActive(true);
    }

    public void DieSuccess()
    {
        if (IsDead) return;

        IsDead = true;
        gameObject.SetActive(false);

        _manager?.CheckAllEnemiesDead();
    }

    public void OnHit()
    {
        if (IsDead) return;

        _manager?.EnemyHit(this);
    }
}
