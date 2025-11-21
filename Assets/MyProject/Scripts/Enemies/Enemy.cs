using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsDead = false;

    // 기본 초기화
    public void ResetEnemy()
    {
        IsDead = false;
        gameObject.SetActive(true);
    }

    // 적을 죽일 때
    public void Die()
    {
        IsDead = true;
        gameObject.SetActive(false);

        // 적 하나 죽을 때 EnemyManager에게 보고
        FindObjectOfType<EnemyManager>().CheckAllEnemiesDead();
    }
}
