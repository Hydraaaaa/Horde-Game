using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [HideInInspector] public GameObject gameObjectManager;
    public GameObject spawnPrefab;

    public float cooldown;
    float currentCooldown;
    public int amount;

	void Start ()
    {
        if (GetComponent<Renderer>() != null)
            GetComponent<Renderer>().enabled = false;
	}
	
	void Update ()
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
                Instantiate(spawnPrefab, spawnPos, transform.rotation);
            }
        }
	}
}
