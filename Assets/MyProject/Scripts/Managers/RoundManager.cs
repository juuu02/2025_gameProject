using System.Collections;
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
    public GameObject GameOverPanel;

    [Header("Round Info")]
    public int CurrentRound = 1;

    private bool _roundActive = false;
    private bool _isGameOver = false;

    void Start()
    {
        StartSequence.PlayStartSequence();
    }

    public void StartRound()
    {
        _roundActive = true;
        _isGameOver = false;

        int[] correctSequence = ColorManager.GetShuffledIndices();
        EnemyManager.SetCorrectSequence(correctSequence);

        UpdateRoundUI();
    }

    public void OnAllEnemiesDefeated()
    {
        if (!_roundActive || _isGameOver) return;

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

        if (IsBossRound(CurrentRound))
        {
            LoadBossScene();
            return;
        }

        StartSequence.ResetWeaponAmmo();
        StartSequence.PlayStartSequence();
    }

    // ------------------- 수정된 부분 ---------------------
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
    // -----------------------------------------------------

    public void TriggerGameOver()
    {
        if (!_roundActive || _isGameOver) return;

        _roundActive = false;
        _isGameOver = true;

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2f);

        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    private void UpdateRoundUI()
    {
        if (RoundText != null)
            RoundText.text = $"Round {CurrentRound}";
    }

    private bool IsBossRound(int round)
    {
        return round % 4 == 0;
    }

    private void LoadBossScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("BossScene");
    }
}
