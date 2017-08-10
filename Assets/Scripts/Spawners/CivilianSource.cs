using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSource : MonoBehaviour
{
    public GameObject civilianPrefab;
    public GameObjectManager manager;

    public int civilians;
    int currentCivilians;
    
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
        if (active && manager != null)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0 && currentCivilians > 0)
            {
                spawnTimer = spawnInterval;
                currentCivilians--;

                GameObject newlySpawned = Instantiate(civilianPrefab, transform.position, transform.rotation);
                if (newlySpawned.GetComponent<CivilianNavigation>() != null)
                    newlySpawned.GetComponent<CivilianNavigation>().manager = manager;
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
