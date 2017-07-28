using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public float interval;
    float currentInterval;
    void Start()
    {
        currentInterval = interval;
    }

	void Update ()
    {
        currentInterval -= Time.deltaTime;
        if (currentInterval <= 0)
        {
            currentInterval = interval;
            GameObject newEnemy = Instantiate(enemy, transform.position, transform.rotation);
            newEnemy.GetComponent<EnemyNavigation>().player = player;
        }
	}
}
