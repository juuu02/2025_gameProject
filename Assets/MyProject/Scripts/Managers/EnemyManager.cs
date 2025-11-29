using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] Enemies;   // Spawn된 Enemy Clone들

    private int _sequenceIndex = 0;      // 현재 정답 체크 위치
    private int[] _correctSequence;      // RoundManager에서 전달된 정답 배열

    private RoundManager _roundManager;

    private void Awake()
    {
        _roundManager = FindFirstObjectByType<RoundManager>();

        if (_roundManager == null)
            Debug.LogError("❌ EnemyManager: RoundManager를 찾을 수 없습니다!");
    }

    // ⭐ RoundManager가 정답 배열을 넣어줌
    public void SetCorrectSequence(int[] sequence)
    {
        _correctSequence = (int[])sequence.Clone();
        _sequenceIndex = 0;

        Debug.Log($"[EnemyManager] 정답 배열 세팅: {_correctSequence[0]}, {_correctSequence[1]}, {_correctSequence[2]}, {_correctSequence[3]}");
    }

    // ⭐ Spawn_enemy에서 호출됨
    public void SetupEnemiesWithClones(GameObject[] clones)
    {
        Enemies = clones;

        // Enemy 초기화
        foreach (var obj in Enemies)
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.ResetEnemy();
            obj.SetActive(true);
        }
    }

    // ⭐ Enemy Hit 처리
    public void EnemyHit(Enemy enemyHit)
    {
        if (_correctSequence == null)
        {
            Debug.LogError("❌ EnemyManager: 정답 배열이 없습니다!");
            return;
        }

        int requiredIndex = _correctSequence[_sequenceIndex]; // 지금 맞춰야 할 색 인덱스
        int enemyIndex = enemyHit.ColorIndex;                 // 이 Enemy의 고유 인덱스

        Debug.Log($"EnemyHit → EnemyIndex: {enemyIndex}, Required: {requiredIndex}");

        if (enemyIndex == requiredIndex)
        {
            // 정답
            enemyHit.DieSuccess();
            _sequenceIndex++;

            if (_sequenceIndex >= 4)
            {
                Debug.Log("🎉 모든 정답 순서 완료!");
                _roundManager.OnAllEnemiesDefeated();
            }
        }
        else
        {
            Debug.Log("❌ 오답! 잘못된 Cowboy를 쐈습니다.");
        }
    }

    // Enemy가 개별적으로 죽을 때마다 호출되는 함수
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
        // 정답을 4개 맞췄다면 true, 아니면 false 반환
        return _sequenceIndex >= 4;
    }
}
