using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] Enemies;

    private int _sequenceIndex = 0;
    private int[] _correctSequence;

    private RoundManager _roundManager;

    private void Awake()
    {
        _roundManager = FindFirstObjectByType<RoundManager>();
    }

    public void SetCorrectSequence(int[] sequence)
    {
        _correctSequence = (int[])sequence.Clone();
        _sequenceIndex = 0;
    }

    public void SetupEnemiesWithClones(GameObject[] clones)
    {
        Enemies = clones;

        foreach (var obj in Enemies)
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.ResetEnemy();
            obj.SetActive(true);
        }
    }

    public void EnemyHit(Enemy enemyHit)
    {
        if (_correctSequence == null) return;

        int requiredIndex = _correctSequence[_sequenceIndex];
        int enemyIndex = enemyHit.ColorIndex;

        if (enemyIndex == requiredIndex)
        {
            enemyHit.DieSuccess();
            _sequenceIndex++;

            if (_sequenceIndex >= 4)
                _roundManager.OnAllEnemiesDefeated();
        }
    }

    public void CheckAllEnemiesDead()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy.activeSelf)
                return;
        }

        _roundManager.OnAllEnemiesDefeated();
    }

    public bool IsMissionComplete()
    {
        return _sequenceIndex >= 4;
    }
}
