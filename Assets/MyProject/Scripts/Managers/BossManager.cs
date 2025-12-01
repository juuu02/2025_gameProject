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
        RoundManager.NextRoundNumber++;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    private void HandlePlayerDeath()
    {
        if (!_bossActive) return;
        _bossActive = false;

        Debug.Log("💀 플레이어 사망 → GameOverScene 로드");
        SceneManager.LoadScene("GameOverScene");
    }
}
