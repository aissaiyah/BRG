using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManagerScript : MonoBehaviour
{
    public GameObject[] Spawners;
    public GameObject pelletPrefab;
    public float spawnInterval = 3f;

    void Start()
    {
        StartCoroutine(SpawnPellet());
    }

    IEnumerator SpawnPellet()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (Spawners.Length > 0)
            {
                int randomIndex = Random.Range(0, Spawners.Length);
                GameObject randomSpawner = Spawners[randomIndex];
                Instantiate(pelletPrefab, randomSpawner.transform.position, Quaternion.identity);
            }
        }
    }
}