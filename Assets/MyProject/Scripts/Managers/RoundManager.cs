using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Managers")]
    public ColorSequenceManager ColorManager;
    public EnemyManager EnemyManager;

    [Header("Round Info")]
    public int CurrentRound = 1;

    private bool _roundActive = false;

    void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        Debug.Log($"[RoundManager] 라운드 시작: {CurrentRound}");

        _roundActive = true;

        // 1) 큐브 색상(인덱스) 섞기
        ColorManager.ShuffleAndApply();

        // 2) 섞인 인덱스 배열 가져오기  (예: [3,0,2,1])
        int[] correctSequence = ColorManager.GetShuffledIndices();

        // 3) EnemyManager에게 정답 전달
        EnemyManager.SetCorrectSequence(correctSequence);

        RoundUI();
    }

    public void EndRound()
    {
        if (!_roundActive) return;

        Debug.Log($"[RoundManager] 라운드 종료: {CurrentRound}");

        _roundActive = false;

        CurrentRound++;

        StartRound();
    }

    private void RoundUI()
    {
        Debug.Log($"현재 라운드: {CurrentRound}");
    }

    // EnemyManager가 모든 적 사망 시 호출
    public void OnAllEnemiesDefeated()
    {
        EndRound();
    }
}
