using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_enemy : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public float spawnInterval = 2.0f;
    public Vector3 positionA = new Vector3(5.89f, 5.54f, 14.76f);
    public Vector3 positionB = new Vector3(-8.6f, 5.86f, 3.84f);
    public Vector3 positionC = new Vector3(0f, 5.86f, 3.84f);
    public Vector3 positionD = new Vector3(-0.5f, 5.86f, -10.5f);

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

    public void SpawnAllEnemies()
    {
        // 1. 모든 스폰 위치를 배열에 저장합니다.
        Vector3[] spawnPositions = new Vector3[] { positionA, positionB, positionC, positionD };

        // 2. 중복 없는 랜덤 순서로 섞인 적 프리팹 배열을 가져옵니다.
        GameObject[] shuffledEnemyArray = GetShuffledEnemyArray();

        // 3. 섞인 적 배열의 길이만큼 반복합니다.
        for (int i = 0; i < shuffledEnemyArray.Length; i++)
        {
            GameObject enemyToSpawn = shuffledEnemyArray[i];

            // 4. 스폰 위치를 배열 순서대로 사용하거나 랜덤으로 선택합니다. (기존 코드를 따름)
            Vector3 spawnPos = (i < spawnPositions.Length) ? spawnPositions[i] : spawnPositions[0];

            Instantiate(enemyToSpawn, spawnPos, transform.rotation);
        }
    }
}
