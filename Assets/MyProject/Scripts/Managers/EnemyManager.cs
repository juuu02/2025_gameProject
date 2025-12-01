using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] Enemies;

    [Header("Sequence Logic")]
    private int _sequenceIndex = 0;
    private int[] _correctSequence; // 정답 컬러 순서 (길이 4)

    private RoundManager _roundManager;

    // ⭐ 적은 항상 4명이므로, 최대 숫자는 4로 고정!
    private const int MaxEnemies = 4;

    [Header("Difficulty Settings")]
    // 라운드별 강가 도착 목표 시간 (초)
    private float[] arrivalTimes = new float[]
    {
        10.0f, // Round 1
        9.0f,  // Round 2
        8.0f,  // Round 3
        9.5f,  // Boss 1
        7.5f,  // Round 5
        6.5f,  // Round 6
        5.5f,  // Round 7
        7.0f,  // Boss 2
        4.5f,  // Round 8
        4.0f,  // Round 9
        3.5f,  // Round 10
        6.0f   // Final Boss
    };

    private int currentRound = 1;

    private void Awake()
    {
        _roundManager = FindFirstObjectByType<RoundManager>();
    }

    // ----------------------------------------------------
    //  정답 순서 설정
    // ----------------------------------------------------
    public void SetCorrectSequence(int[] sequence)
    {
        _correctSequence = (int[])sequence.Clone();
        _sequenceIndex = 0;
    }

    // ----------------------------------------------------
    //  속도 계산 및 적 설정
    // ----------------------------------------------------
    public void SetupEnemiesWithClones(GameObject[] clones)
    {
        Enemies = clones;

        // 현재 라운드 시간 가져오기
        float arrivalTime = arrivalTimes[Mathf.Clamp(currentRound - 1, 0, arrivalTimes.Length - 1)];

        Debug.Log($"⏳ Round {currentRound} (4명 고정) / 목표 시간: {arrivalTime}초");

        foreach (var obj in Enemies)
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            CowBoy_control cowboy = obj.GetComponent<CowBoy_control>();

            if (enemy != null) enemy.ResetEnemy();
            obj.SetActive(true);

            if (cowboy != null)
            {
                // 거리 / 시간 = 속도
                Vector3 targetPos = cowboy.GetTargetPosition();
                float distance = Vector3.Distance(obj.transform.position, targetPos);

                float moveSpeed = 0f;
                if (arrivalTime > 0) moveSpeed = distance / arrivalTime;

                cowboy.SetMoveSpeed(moveSpeed);
            }
        }
    }

    // ----------------------------------------------------
    //  ⭐ 여기가 핵심 수정됨 (4명 기준)
    // ----------------------------------------------------
    public void EnemyHit(Enemy enemyHit)
    {
        if (_correctSequence == null) return;

        // 이미 4명을 다 잡았으면 더 이상 진행 안 함
        if (_sequenceIndex >= MaxEnemies) return;

        int requiredIndex = _correctSequence[_sequenceIndex];
        int enemyIndex = enemyHit.ColorIndex;

        if (enemyIndex == requiredIndex)
        {
            // ✅ 정답!
            enemyHit.DieSuccess();
            _sequenceIndex++; // 0 -> 1 -> 2 -> 3 -> 4(끝)

            // 4마리를 다 잡았으면 승리!
            if (_sequenceIndex >= MaxEnemies)
            {
                Debug.Log("🎉 4명 처치 완료! 라운드 클리어!");
                _roundManager.OnAllEnemiesDefeated();
            }
        }
        else
        {
            // ❌ 오답!
            Debug.LogWarning($"땡! 틀렸습니다. (필요: {requiredIndex}, 맞음: {enemyIndex})");
        }
    }

    // ----------------------------------------------------
    //  혹시 모를 상황 대비: 살아있는 적이 있는지 확인
    // ----------------------------------------------------
    public void CheckAllEnemiesDead()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf) return;
        }
        // 다 죽었으면 승리 처리
        _roundManager.OnAllEnemiesDefeated();
    }

    // ----------------------------------------------------
    //  미션 성공 여부 반환 (4명 다 잡았니?)
    // ----------------------------------------------------
    public bool IsMissionComplete()
    {
        return _sequenceIndex >= MaxEnemies;
    }

    public void SetRound(int round)
    {
        currentRound = round;
    }

    public void OnEnemyArrivedAtRiver()
    {
        Debug.Log("🚨 적이 강을 건넜습니다! GAME OVER");
        _roundManager.TriggerGameOver();
    }
}