using System.Collections;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static int NextRoundNumber = 1;

    [Header("Managers")]
    public ColorSequenceManager ColorManager;
    public EnemyManager EnemyManager;
    // public Spawn_enemy Spawner; // ❌ 더 이상 여기서 소환하지 않으므로 변수 필요 없음 (삭제 가능)

    public TextMeshProUGUI RoundText;
    public PlayerStartSequence StartSequence;
    public Transform PlayerStartPoint;
    public GameObject GameOverPanel;

    [Header("Round Info")]
    public int CurrentRound;

    private bool _roundActive = false;
    private bool _isGameOver = false;
    void Awake()
    {
        // 🔥 다음 라운드 번호로 현재 라운드를 설정
        CurrentRound = NextRoundNumber;
    }

    void Start()
    {
        // 1. 첫 라운드 번호 설정
        EnemyManager.SetRound(CurrentRound);

        // 2. 오프닝 시퀀스 시작
        StartSequence.PlayStartSequence();
    }

    // ==============================================================
    //  본 게임 시작 (StartSequence가 끝난 뒤 호출됨)
    // ==============================================================
    public void StartRound()
    {
        _roundActive = true;
        _isGameOver = false;

        // ❌ [삭제됨] 여기서 적을 소환하면 2배로 나오므로 삭제!
        // GameObject[] newEnemies = Spawner.Make_Enemies(); 

        // ✅ [추가됨] 대신, 이미 화면에 소환되어 있는 적들을 찾습니다.
        Enemy[] foundEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        // Enemy 컴포넌트 배열을 GameObject 배열로 변환
        GameObject[] enemyObjects = new GameObject[foundEnemies.Length];
        for (int i = 0; i < foundEnemies.Length; i++)
        {
            enemyObjects[i] = foundEnemies[i].gameObject;
        }

        // 2. 찾은 적들을 EnemyManager에게 넘김 (속도 계산 및 설정)
        if (enemyObjects.Length > 0)
        {
            EnemyManager.SetupEnemiesWithClones(enemyObjects);
            Debug.Log($"🚀 이미 소환된 {enemyObjects.Length}명의 적을 찾아서 설정했습니다.");
        }
        else
        {
            Debug.LogWarning("⚠️ 씬에 적이 없습니다! 다른 곳에서 소환이 제대로 되었는지 확인하세요.");
        }

        // 3. 정답 색상 순서 생성 및 전달
        int[] correctSequence = ColorManager.GetShuffledIndices();
        EnemyManager.SetCorrectSequence(correctSequence);

        UpdateRoundUI();
    }

    // ==============================================================
    //  라운드 종료 및 다음 라운드 이동
    // ==============================================================
    public void OnAllEnemiesDefeated()
    {
        if (!_roundActive || _isGameOver) return;

        Debug.Log("🎉 라운드 클리어!");
        _roundActive = false;

        StartCoroutine(EndRoundRoutine());
    }

    private IEnumerator EndRoundRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        ProceedToNextRound();
    }

    private void ProceedToNextRound()
    {
        CurrentRound++;
        NextRoundNumber = CurrentRound;

        if (IsBossRound(CurrentRound))
        {
            LoadBossScene();
            return;
        }

        // 다음 라운드 정보 입력
        EnemyManager.SetRound(CurrentRound);

        StartSequence.ResetWeaponAmmo();
        StartSequence.PlayStartSequence();
    }

    // ==============================================================
    //  게임 오버 / 탄약 체크
    // ==============================================================
    public void CheckAmmoAndGameOver(int currentAmmo)
    {
        if (currentAmmo > 0) return;
        StartCoroutine(AmmoCheckRoutine());
    }

    private IEnumerator AmmoCheckRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (EnemyManager.IsMissionComplete()) yield break;
        TriggerGameOver();
    }

    public void TriggerGameOver()
    {
        if (!_roundActive || _isGameOver) return;

        _roundActive = false;
        _isGameOver = true;
        Debug.Log("💀 Game Over Triggered");

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // ==============================================================
    //  유틸리티
    // ==============================================================
    private void UpdateRoundUI()
    {
        if (RoundText != null)
            RoundText.text = $"Round {CurrentRound}";
    }

    private bool IsBossRound(int round)
    {
        return round % 2 == 0;
    }

    private void LoadBossScene()
    {
        Debug.Log("😈 보스 씬으로 이동합니다...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("BossScene");
    }
}