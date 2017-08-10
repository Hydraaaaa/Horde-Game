using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSource : MonoBehaviour
{
    public int civilians;
    int currentCivilians;
    
    public float spawnInterval;
    float spawnTimer;

    bool active;

	void Start ()
    {
        spawnTimer = 0;
        active = false;
	}
	
	void Update ()
    {
        spawnTimer -= Time.deltaTime;
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
