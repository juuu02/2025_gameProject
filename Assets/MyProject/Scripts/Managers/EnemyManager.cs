using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] Enemies;

    public void SetupEnemies(int round)
    {
        foreach (var enemy in Enemies)
        {
            enemy.SetActive(true);
            enemy.GetComponent<Enemy>().ResetEnemy(); 
        }
    }

    public void CheckAllEnemiesDead()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy.activeSelf)
                return;
        }

        // 모든 적이 죽었으면 RoundManager로 보고
        FindObjectOfType<RoundManager>().OnAllEnemiesDefeated();
    }
}
