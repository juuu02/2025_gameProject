using System.Collections.Generic;
using UnityEngine;

public class Spawn_enemy : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public Vector3 positionA = new Vector3(5.89f, 5.54f, 14.76f);
    public Vector3 positionB = new Vector3(-8.6f, 5.86f, 3.84f);
    public Vector3 positionC = new Vector3(0f, 5.86f, 3.84f);
    public Vector3 positionD = new Vector3(-0.5f, 5.86f, -10.5f);

    public float targetXMin = -21.0f;
    public float targetXMax = 21.0f;
    public float targetZ = -50.0f;

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
    // 핵심 수정: void -> GameObject[] 로 변경
    // 매니저가 부르면 적을 만들고, 그 적들을 "리턴" 해줍니다.
    // -------------------------------
    public GameObject[] SpawnAllEnemies()
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
            float randomTargetX = Random.Range(targetXMin, targetXMax);
            Vector3 targetVector = new Vector3(randomTargetX, spawnPos.y, targetZ);

            CowBoy_control control = clone.GetComponent<CowBoy_control>();
            if (control != null)
            {
                // 목표 위치만 설정 (속도는 아직!)
                control.SetDestinationTarget(targetVector);
            }

            // Clone 저장
            spawnedEnemies[i] = clone;
        }

        // ⭐ 변경됨: 직접 EnemyManager를 부르지 않고, RoundManager에게 배열을 줍니다.
        return spawnedEnemies;
    }
}