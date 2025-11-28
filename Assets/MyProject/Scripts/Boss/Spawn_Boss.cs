using System.Collections.Generic;
using UnityEngine;

public class Spawn_Boss : MonoBehaviour
{
    public GameObject Boss; // Boss Prefab
    public Vector3 positionA = new Vector3(5.89f, 5.54f, 14.76f);
    public Vector3 positionB = new Vector3(-8.6f, 5.86f, 3.84f);
    public Vector3 positionC = new Vector3(0f, 5.86f, 3.84f);
    public Vector3 positionD = new Vector3(-0.5f, 5.86f, -10.5f);

    public float targetXMin = -21.0f;
    public float targetXMax = 21.0f;
    public float targetZ = -50.0f;
    public void SpawnBoss()
    {
        Vector3[] spawnPositions = new Vector3[] { positionA, positionB, positionC, positionD };

        Vector3 spawnPos = spawnPositions[Random.Range(0, 4)];

        // Instantiate Clone 생성
        GameObject clone = Instantiate(Boss, spawnPos, transform.rotation);
        float randomTargetX = Random.Range(targetXMin, targetXMax);
        Vector3 targetVector = new Vector3(randomTargetX, spawnPos.y, targetZ);
        Boss_control control = clone.GetComponent<Boss_control>();
        if (control != null)
        {
            control.SetDestinationTarget(targetVector);
        }
        

        //// ⭐ EnemyManager에게 Clone 배열 넘기기
        //BossManager manager = FindFirstObjectByType<BossManager>();
        //if (manager != null)
        //{
        //    manager.SetupEnemiesWithClones();
        //}
        //else
        //{
        //    Debug.LogError("❌ EnemyManager를 찾을 수 없습니다!");
        //}
    }
}
