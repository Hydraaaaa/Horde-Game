using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSource : MonoBehaviour
{
    public GameObject civilianPrefab;

    public int civilians;
    [HideInInspector] public int currentCivilians;
    
    public float spawnInterval;
    float spawnTimer;

    public bool active;

	void Start ()
    {
        currentCivilians = civilians;
        spawnTimer = 0;
        active = true;
	}
	
	void Update ()
    {
        if (active && GameObjectManager.instance != null)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0 && currentCivilians > 0)
            {
                spawnTimer = spawnInterval;
                currentCivilians--;

                GameObject newlySpawned = Instantiate(civilianPrefab, transform.position, transform.rotation);
            }
        }
    }

    public void SetSpawning(bool spawn)
    {
        active = spawn;
    }

    public void Reset()
    {
        currentCivilians = civilians;
        spawnTimer = spawnInterval;
    }
}
