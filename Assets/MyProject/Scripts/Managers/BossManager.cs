using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossManager : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Spawn_Boss spawner;
    public GameObject GameOverPanel;

    private BossHealth bossHealth;
    private bool _bossActive = false;

    private void Start()
    {
        if (playerHealth != null)
            playerHealth.OnPlayerDeath += HandlePlayerDeath;

        if (spawner != null)
            spawner.SpawnBoss();

        _bossActive = true;
    }

    public void RegisterBoss(BossHealth health)
    {
        bossHealth = health;

        if (bossHealth != null)
        {
            bossHealth.OnBossDeath += HandleBossDeath;
            Debug.Log("✅ Boss 등록 성공");
        }
        else
        {
            Debug.LogError("❌ RegisterBoss: BossHealth null");
        }
    }

    private void HandleBossDeath()
    {
        if (!_bossActive) return;
        _bossActive = false;

        Debug.Log("🔥 보스 사망 → 다음 라운드로 진행!");
        RoundManager.NextRoundNumber++;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    private void HandlePlayerDeath()
    {
        if (!_bossActive) return;
        _bossActive = false;

        Debug.Log("💀 플레이어 사망 → 게임 오버 시퀀스 시작");
        StartCoroutine(BossGameOverRoutine());
    }

    private IEnumerator BossGameOverRoutine()
    {
        // 1.0초 대기 (플레이어 사망 애니메이션 등)
        yield return new WaitForSeconds(1.0f);

        if (GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);
        }

        // 게임 시간 정지 (You Died 효과)
        Time.timeScale = 0f;
        Debug.Log("💀 게임 오버 처리 완료: UI 표시 및 시간 정지");
    }
}
