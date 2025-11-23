using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] Enemies;
    private int _sequenceIndex = 0;
    private ColorSequenceManager _colorManager;

    private void Awake()
    {
        _colorManager = FindFirstObjectByType<ColorSequenceManager>();
    }

    public void SetupEnemies(int round)
    {
        _sequenceIndex = 0;

        foreach (var enemy in Enemies)
        {
            enemy.SetActive(true);
            enemy.GetComponent<Enemy>().ResetEnemy(); 
        }
    }

    public void EnemyHit(Enemy enemyHit)
    {
        Color requiredColor = _colorManager.GetColorAtIndex(_sequenceIndex);

        Color enemyColor = enemyHit.EnemyColor;

        if (enemyColor.Equals(requiredColor))
        {
            enemyHit.DieSuccess();

            _sequenceIndex++;

            if (_sequenceIndex >= _colorManager._cubes.Length)
            {
                Debug.Log("모든 색상 순서를 맞췄습니다! 다음 라운드로 넘어갈 수 있습니다.");
            }
        }
        else
        {
            Debug.Log("잘못된 색상을 맞췄습니다! 다시 시도하세요.");
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
        FindFirstObjectByType<RoundManager>().OnAllEnemiesDefeated();
    }
}
