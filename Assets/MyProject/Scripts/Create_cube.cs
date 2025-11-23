using System.Collections.Generic;
using UnityEngine;

public class Create_cube : MonoBehaviour
{
    // 큐브 프리팹 배열
    public GameObject[] cubePrefabs;
    private GameObject[] shuffledCubeArray;
    private int cube_count = 0;
    private Vector3 spawnPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shuffledCubeArray = GetSuffledCubeArray();
        spawnPosition = new Vector3(-5.0f, 7.0f, -60.0f);
        for (int i = 0; i < cubePrefabs.Length; i++)
        {
            SpawnCube(spawnPosition);
            spawnPosition.x -= 2.0f;
        }
            
    }
    void SpawnCube(Vector3 pos)
    {
        GameObject cubeToSpawn = shuffledCubeArray[cube_count];

        GameObject cube = GameObject.Instantiate(cubeToSpawn);

        cube.transform.position = pos;
        cube_count++;
    }

    private GameObject[] GetSuffledCubeArray()
    {
        List<GameObject> cubeList = new List<GameObject>(cubePrefabs);
        int n = cubeList.Count-1;
        int k = Random.Range(0, n + 1);

        // 요소를 교환(Swap)
        GameObject value = cubeList[k];
        cubeList[k] = cubeList[n];
        cubeList[n] = value;
        return cubeList.ToArray();
    }
}
