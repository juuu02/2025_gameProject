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

    public float targetXMin = -21.0f;
    public float targetXMax = 21.0f;
    public float targetZ = -50.0f;

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
        Vector3[] spawnPositions = new Vector3[] { positionA, positionB, positionC, positionD };

        GameObject[] shuffledEnemyArray = GetShuffledEnemyArray();

        for (int i = 0; i < shuffledEnemyArray.Length; i++)
        {
            GameObject enemyToSpawn = shuffledEnemyArray[i];

            Vector3 spawnPos = (i < spawnPositions.Length) ? spawnPositions[i] : spawnPositions[0];

            GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPos, transform.rotation);

            float randomTargetX = Random.Range(targetXMin, targetXMax);

            Vector3 targetVector = new Vector3(randomTargetX, spawnPos.y, targetZ);

            CowBoy_control control = spawnedEnemy.GetComponent<CowBoy_control>();

            if (control != null)
            {
                control.SetDestinationTarget(targetVector);
            }
        }
    }
}
