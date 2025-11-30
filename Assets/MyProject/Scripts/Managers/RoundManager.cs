using System.Collections; // 🔥 코루틴 사용을 위해 필수!
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static int NextRoundNumber = 1;

    [Header("Managers")]
    public ColorSequenceManager ColorManager;
    public EnemyManager EnemyManager;
    public TextMeshProUGUI RoundText;
    public PlayerStartSequence StartSequence;
    public Transform PlayerStartPoint;

    [Header("Round Info")]
    public int CurrentRound;

    private bool _roundActive = false;
    void Awake()
    {
        // 🔥 다음 라운드 번호로 현재 라운드를 설정
        CurrentRound = NextRoundNumber;

        // DontDestroyOnLoad 코드는 제거
    }

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
        NextRoundNumber = CurrentRound;

        if (IsBossRound(CurrentRound))
        {
            LoadBossScene();
            return;   // 다음 로직 진행하지 않음
        }

        // 무기 리셋
        StartSequence.ResetWeaponAmmo();

        // 다시 시작 시퀀스(위치 이동, 카운트다운) 실행
        StartSequence.PlayStartSequence();
    }


    // 🔥 [추가] 총알을 쏠 때마다(또는 0발일 때) 이 함수를 호출해줘!
    public void CheckAmmoAndGameOver(int currentAmmo)
    {
        // 1. 총알이 남아있으면 게임 오버 아님 -> 통과
        if (currentAmmo > 0) return;

        // 2. 총알이 0발인데, 이미 미션을 성공했다면? -> 통과 (OnAllEnemiesDefeated가 처리함)
        if (EnemyManager.IsMissionComplete()) return;

        // 3. 총알도 없고, 미션도 성공 못했음 -> 게임 오버!
        TriggerGameOver();
    }

    // 🔥 [추가] 게임 오버 실행 함수
    private void TriggerGameOver()
    {
        if (!_roundActive) return;
        _roundActive = false;

        Debug.Log("💀 GAME OVER: 총알이 다 떨어졌습니다!");

        // 여기에 게임 오버 UI 띄우기 or 재시작 로직 넣기
        // 예: UIManager.ShowGameOverPopup();
    }
    private void UpdateRoundUI()
    {
        if (RoundText != null)
            RoundText.text = $"Round {CurrentRound}";
    }

    // 보스 라운드 판별 (4, 8, 12 ...)
    private bool IsBossRound(int round)
    {
        return round % 2 == 0;
    }

    // 보스 씬 로드
    private void LoadBossScene()
    {
        Debug.Log("🔥 BossScene으로 이동!");

        UnityEngine.SceneManagement.SceneManager.LoadScene("BossScene");
    }
}