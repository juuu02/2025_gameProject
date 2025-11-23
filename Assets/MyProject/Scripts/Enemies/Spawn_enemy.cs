using System.Collections.Generic;
using UnityEngine;

public class Spawn_enemy : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public Vector3 positionA = new Vector3(5.89f, 5.54f, 14.76f);
    public Vector3 positionB = new Vector3(-8.6f, 5.86f, 3.84f);
    public Vector3 positionC = new Vector3(0f, 5.86f, 3.84f);
    public Vector3 positionD = new Vector3(-0.5f, 5.86f, -10.5f);

    // -------------------------------
    // 랜덤 셔플 함수
    // -------------------------------
    private GameObject[] GetShuffledEnemyArray()
    {
        List<GameObject> enemyList = new List<GameObject>(enemyPrefab);
        int n = enemyList.Count;

        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            GameObject value = enemyList[k];
            enemyList[k] = enemyList[n];
            enemyList[n] = value;
        }

        return enemyList.ToArray();
    }

    // -------------------------------
    // 핵심: 인스턴스 생성 + EnemyManager에 Clone 전달
    // -------------------------------
    public void SpawnAllEnemies()
    {
        Vector3[] spawnPositions = new Vector3[] { positionA, positionB, positionC, positionD };

        // 섞인 프리팹 목록
        GameObject[] shuffledEnemyArray = GetShuffledEnemyArray();

        // 실제 Clone들을 저장할 배열
        GameObject[] spawnedEnemies = new GameObject[shuffledEnemyArray.Length];

        for (int i = 0; i < shuffledEnemyArray.Length; i++)
        {
            GameObject enemyToSpawn = shuffledEnemyArray[i];
            Vector3 spawnPos = (i < spawnPositions.Length) ? spawnPositions[i] : spawnPositions[0];

            // Instantiate Clone 생성
            GameObject clone = Instantiate(enemyToSpawn, spawnPos, transform.rotation);

            // Clone 저장
            spawnedEnemies[i] = clone;
        }

        // ⭐ EnemyManager에게 Clone 배열 넘기기
        EnemyManager manager = FindFirstObjectByType<EnemyManager>();
        if (manager != null)
        {
            manager.SetupEnemiesWithClones(spawnedEnemies);
        }
        else
        {
            Debug.LogError("❌ EnemyManager를 찾을 수 없습니다!");
        }
    }
}
