using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsDead = false;
    public Color EnemyColor;

    public int OrderIndex;   // ⭐ 정답 순서 번호 (0~3)

    private EnemyManager _manager;

    private void Awake()
    {
        _manager = FindFirstObjectByType<EnemyManager>();
    }

    // 초기화
    public void ResetEnemy()
    {
        IsDead = false;
        gameObject.SetActive(true);
    }

    // EnemyManager에서 정답으로 처리해 죽는 경우
    public void DieSuccess()
    {
        if (IsDead)
            return;

        IsDead = true;
        gameObject.SetActive(false);

        if (_manager != null)
            _manager.CheckAllEnemiesDead();
    }

    // 적이 총에 맞았을 때 호출
    public void OnHit()
    {
        if (IsDead)
            return;

        if (_manager != null)
            _manager.EnemyHit(this);
    }
}
