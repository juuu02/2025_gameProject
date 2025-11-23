using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] Enemies;   // Spawn된 Enemy Clone들
    private int _sequenceIndex = 0;

    private ColorSequenceManager _colorManager;

    private RoundManager _roundManager;   // 라운드 종료 호출용

    private void Awake()
    {
        _colorManager = FindFirstObjectByType<ColorSequenceManager>();
        _roundManager = FindFirstObjectByType<RoundManager>();

        if (_colorManager == null)
            Debug.LogError("❌ EnemyManager: ColorSequenceManager를 찾을 수 없습니다!");
        if (_roundManager == null)
            Debug.LogError("❌ EnemyManager: RoundManager를 찾을 수 없습니다!");
    }

    // ⭐ Spawn_enemy에서 호출됨
    public void SetupEnemiesWithClones(GameObject[] clones)
    {
        Enemies = clones;
        SetupEnemies();
    }

    // ⭐ 색 적용 + 정답 순서 리셋
    private void SetupEnemies()
    {
        _sequenceIndex = 0;

        if (Enemies == null || Enemies.Length != 4)
        {
            Debug.LogError("❌ EnemyManager: Enemies 배열이 비어있거나 4개가 아닙니다!");
            return;
        }

        Color[] colors = _colorManager.GetShuffledColors();

        for (int i = 0; i < Enemies.Length; i++)
        {
            GameObject enemyObj = Enemies[i];
            Enemy enemy = enemyObj.GetComponent<Enemy>();

            enemyObj.SetActive(true);
            enemy.ResetEnemy();

            // 정답용 색 저장
            enemy.EnemyColor = colors[i];

            // 실제 외형 색 적용
            ApplyColorToEnemy(enemyObj, colors[i]);

            // 정답 순서 인덱스 부여
            enemy.OrderIndex = i;
        }
    }

    private void ApplyColorToEnemy(GameObject enemyObj, Color c)
    {
        SkinnedMeshRenderer mesh = enemyObj.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh != null)
        {
            mesh.material.color = c;
            return;
        }

        Renderer r = enemyObj.GetComponentInChildren<Renderer>();
        if (r != null)
            r.material.color = c;
    }

    // ⭐ 총알이 Enemy 맞으면 호출됨
    public void EnemyHit(Enemy enemyHit)
    {
        Color requiredColor = _colorManager.GetColorAtIndex(_sequenceIndex);

        if (enemyHit.EnemyColor == requiredColor)
        {
            // 정답
            enemyHit.DieSuccess();
            _sequenceIndex++;

            if (_sequenceIndex >= 4)
            {
                Debug.Log("🎉 모든 순서를 정확히 맞췄습니다!");
                _roundManager.OnAllEnemiesDefeated();
            }
        }
     
    }

    // Enemy가 개별적으로 죽을 때마다 호출될 수도 있음
    public void CheckAllEnemiesDead()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy.activeSelf)
                return;
        }

        _roundManager.OnAllEnemiesDefeated();
    }
}
