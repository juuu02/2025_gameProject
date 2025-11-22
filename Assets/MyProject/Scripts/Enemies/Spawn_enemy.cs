using System.Collections;
using UnityEngine;

public class Spawn_enemy : MonoBehaviour
{
    public GameObject[] enemy;
    public float spawnInterval = 2.0f;
    public Vector3 positionA = new Vector3(5.89f, 5.54f, 14.76f);
    public Vector3 positionB = new Vector3(-8.6f, 5.86f, 3.84f);
    private int index = 0;

    Vector3 GetRandomPosition()
    {
        Vector3[] positions = new Vector3[] { positionA, positionB };
        int randomIndex = Random.Range(0, positions.Length);
        return positions[randomIndex];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        while(true)
        {
            Vector3 spawnPosition = GetRandomPosition();
            Instantiate(enemy[index], spawnPosition, transform.rotation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
