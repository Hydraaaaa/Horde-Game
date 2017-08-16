using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [HideInInspector]
    public GameObjectManager gameObjectManager;
    public GameObject spawnPrefab;

    public float cooldown;
    float currentCooldown;
    public int amount;

    public float tickTime  = 20f;
    public float curTickTime = 0f;
    public int increasePerTick = 1;

    public bool EnviroSpawner = false;
    bool EnviroInitSpawnDone = false;
	void Start ()
    {
        if (GetComponent<Renderer>() != null)
            GetComponent<Renderer>().enabled = false;
	}

    void Update()
    {
        if (!EnviroSpawner)
        {
            if (gameObjectManager != null)
            {
                currentCooldown -= Time.deltaTime;

                if (currentCooldown <= 0)
                {
                    currentCooldown = cooldown;

                    for (int i = 0; i < amount; i++)
                    {
                        Vector3 spawnPos = new Vector3
                        (
                            transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                            transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                            transform.position.z + Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2)
                        );
                        GameObject newlySpawned = Instantiate(spawnPrefab, spawnPos, transform.rotation);

                        if (newlySpawned.GetComponent<EnemyNavigation>() != null)
                            newlySpawned.GetComponent<EnemyNavigation>().EndPos = gameObjectManager.endPos;
                    }
                }

                curTickTime += Time.deltaTime;
                if (curTickTime > tickTime)
                {
                    curTickTime = 0;
                    amount += increasePerTick;
                }
            }
        }
        else
        {
            if (!EnviroInitSpawnDone)
            {
                EnviroInitSpawnDone = true;

                for (int i = 0; i < amount; i++)
                {
                    Vector3 spawnPos = new Vector3
                    (
                        transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                        transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                        transform.position.z + Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2)
                    );
                    GameObject newlySpawned = Instantiate(spawnPrefab, spawnPos, transform.rotation);

                    if (newlySpawned.GetComponent<EnemyNavigation>() != null)
                    {
                        newlySpawned.GetComponent<EnemyNavigation>().EnvironmentZombie = true;
                        Quaternion rot = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                        newlySpawned.transform.rotation = rot;
                    }
                }
            }
        }
    }

}
