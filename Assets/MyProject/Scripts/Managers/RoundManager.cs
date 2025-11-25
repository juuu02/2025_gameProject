using System.Collections; // 🔥 코루틴 사용을 위해 필수!
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    [Header("Managers")]
    public ColorSequenceManager ColorManager;
    public EnemyManager EnemyManager;
    public TextMeshProUGUI RoundText;
    public PlayerStartSequence StartSequence;
    public Transform PlayerStartPoint;

    [Header("Round Info")]
    public int CurrentRound = 1;

    private bool _roundActive = false;

    void Start()
    {
        // 첫 라운드 시작
        StartSequence.PlayStartSequence();
    }

    public void StartRound()
    {
        _roundActive = true;

        // 셔플된 정답 가져오기 (이미 StartSequence에서 섞었음)
        int[] correctSequence = ColorManager.GetShuffledIndices();
        EnemyManager.SetCorrectSequence(correctSequence);

        UpdateRoundUI();
    }

    // 적을 다 잡았을 때 호출되는 함수
    public void OnAllEnemiesDefeated()
    {
        // 라운드가 이미 끝났으면 중복 실행 방지
        if (!_roundActive) return;

        // 🔥 라운드 상태를 '비활성'으로 먼저 바꿔서 추가 이벤트를 막음
        _roundActive = false;

        // 바로 이동하지 않고, 딜레이 코루틴 시작
        StartCoroutine(EndRoundRoutine());
    }

    // 🔥 1초 대기 후 넘어가는 코루틴
    private IEnumerator EndRoundRoutine()
    {
        // 여기서 1초 동안 현재 위치에서 대기합니다.
        yield return new WaitForSeconds(2.0f);

        // 1초 뒤에 실제 종료 로직 실행
        ProceedToNextRound();
    }

    // 실제 다음 라운드 준비 (기존 EndRound 로직)
    private void ProceedToNextRound()
    {
        // 다음 라운드 번호 증가
        CurrentRound++;

        // 무기 리셋
        StartSequence.ResetWeaponAmmo();

        // 다시 시작 시퀀스(위치 이동, 카운트다운) 실행
        StartSequence.PlayStartSequence();
    }

    private void UpdateRoundUI()
    {
        if (RoundText != null)
            RoundText.text = $"Round {CurrentRound}";
    }
}