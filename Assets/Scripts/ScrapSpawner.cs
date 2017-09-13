using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapSpawner : MonoBehaviour
{
    public GameObject ResourcePrefab;
    public float cooldown;
    float currCooldown;
    public int spawnCountMin = 1;
    public int spawnCountMax = 4;

    public float tickTime = 30f;
    public float currTickTime = 0f;
    
    
	// Use this for initialization
	void Start ()
    {
	    if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().enabled = false;
        }	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currCooldown <= 0)
        {
            currCooldown = cooldown;

            float amount = Random.Range(spawnCountMin, spawnCountMax);

            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnPos = new Vector3
                (
                    transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                    transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                    transform.position.z + Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2)
                );
                Instantiate(ResourcePrefab, spawnPos, transform.rotation);
            }
        }
    }
}
