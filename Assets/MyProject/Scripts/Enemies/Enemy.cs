using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField]
    public int ColorIndex;   // Inspector에서 확인 가능

    public int OrderIndex;
    public bool IsDead = false;

    private EnemyManager _manager;

    private void Awake()
    {
        _manager = FindFirstObjectByType<EnemyManager>();
        ApplyColorIndexByName();
    }

    private void ApplyColorIndexByName()
    {
        string n = gameObject.name.ToLower();

        // 실수로 0으로 초기화되는 걸 방지하기 위해 -1로 시작
        int tempIndex = -1;

        if (n.Contains("red")) tempIndex = 0;
        else if (n.Contains("yellow")) tempIndex = 1;
        else if (n.Contains("green")) tempIndex = 2;
        else if (n.Contains("blue")) tempIndex = 3;

        if (tempIndex != -1)
        {
            ColorIndex = tempIndex;
        }
        else
        {
            Debug.LogError($"🔥 {gameObject.name}: 이름에 색깔(Red, Yellow, Green, Blue)이 없어서 Index 설정 실패! 현재 Index: {ColorIndex}");
        }
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

        // 매니저가 없을 경우를 대비해 null 체크
        if (_manager != null)
            _manager.CheckAllEnemiesDead();
    }

    // EnemyHealth에서 호출하는 함수
    public void OnHit()
    {
        if (IsDead) return;

        Debug.Log($"💥 {gameObject.name} 피격 신호 받음! (ColorIndex: {ColorIndex})");

        if (_manager != null)
        {
            _manager.EnemyHit(this);
        }
        else
        {
            Debug.LogError("❌ EnemyManager를 찾을 수 없습니다!");
        }
    }
}