using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSpawner : MonoBehaviour
{
    [HideInInspector]
    public GameObjectManager gameObjectManager;
    public GameObject spawnPrefab;
    
    public int amount;

    void Start ()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = new Vector3
            (
                transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                transform.position.z + Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2)
            );
            GameObject newlySpawned = Instantiate(spawnPrefab, spawnPos, transform.rotation);
            gameObjectManager.civilians.Add(newlySpawned);

            if (newlySpawned.GetComponent<CivilianNavigation>() != null)
                newlySpawned.GetComponent<CivilianNavigation>().gameObjectManager = gameObjectManager;
        }

        Destroy(gameObject);
    }
}
