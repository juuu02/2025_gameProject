using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Managers")]
    public ColorSequenceManager ColorManager;   // 큐브 색상 관리
    public EnemyManager EnemyManager;           // 적 생성/관리 (나중에 만들 스크립트)

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

        // ♻ 색상 재셔플
        ColorManager.ShuffleAndApply();

        // ♻ 적 리셋 / 재배치
        if (EnemyManager != null)
            EnemyManager.SetupEnemies(CurrentRound);

        // UI 표시 (라운드 번호)
        RoundUI();
    }

    public void EndRound()
    {
        if (!_roundActive) return;

        Debug.Log($"[RoundManager] 라운드 종료: {CurrentRound}");

        _roundActive = false;

        // 라운드 증가
        CurrentRound++;

        // 다음 라운드 시작
        StartRound();
    }

    private void RoundUI()
    {
        Debug.Log($"현재 라운드: {CurrentRound}");
        // 나중에 화면에 숫자 표시 가능
    }

    // EnemyManager가 모든 적 사망 시 호출
    public void OnAllEnemiesDefeated()
    {
        EndRound();
    }
}
