using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool IsDead = false;

    private BossManager _manager;

    private void Awake()
    {
        _manager = FindFirstObjectByType<BossManager>();

    }

    public void ResetBoss()
    {
        IsDead = false;
        gameObject.SetActive(true);
    }

    public void DieSuccess()
    {
        if (IsDead) return;

        IsDead = true;
        gameObject.SetActive(false);

        //_manager?.CheckAllEnemiesDead();
    }

    public void OnHit()
    {
        if (IsDead) return;

        _manager?.BossHit(this);
    }
}

