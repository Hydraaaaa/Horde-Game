using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchedGrenade : MonoBehaviour
{
    [Tooltip("Time until explosion")]
    public float time;

    public float damage;

    public GameObject[] particleEffects;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        time -= Time.deltaTime;
        if (time <= 0)
            Explode();
	}

    public void Explode()
    {
        foreach (GameObject particleEffect in particleEffects)
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
