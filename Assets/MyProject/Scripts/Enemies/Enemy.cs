using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsDead = false;
    public Color EnemyColor;
    private EnemyManager _manager;

    private void Awake()
    {
        _manager = FindFirstObjectByType<EnemyManager>();
    }

    // 기본 초기화
    public void ResetEnemy()
    {
        IsDead = false;
        gameObject.SetActive(true);
    }

    public void DieSuccess()
    {
        IsDead = true;
        gameObject.SetActive(false);

        _manager.CheckAllEnemiesDead();
    }

    public void OnHit()
    {
        _manager.EnemyHit(this);
    }
}
