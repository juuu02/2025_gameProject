using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Spawn_Boss spawner;

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

        // ⭐ BossScene에서는 RoundManager가 씬에 없음 → GameScene에서 찾도록 로직 추가
        RoundManager rm = FindAnyObjectByType<RoundManager>();

        if (rm != null)
        {
            // ✔ Round 증가
            rm.CurrentRound++;
        }
        else
        {
            Debug.LogWarning("⚠️ RoundManager를 찾지 못했습니다. GameScene에서 새로 시작합니다.");
        }

        // ✔ GameScene으로 이동 (RoundManager가 StartSequence로 다음 라운드 준비)
        SceneManager.LoadScene("GameScene");
    }

    private void HandlePlayerDeath()
    {
        if (!_bossActive) return;
        _bossActive = false;

        Debug.Log("💀 플레이어 사망 → GameOverScene 로드");
        SceneManager.LoadScene("GameOverScene");
    }
}
