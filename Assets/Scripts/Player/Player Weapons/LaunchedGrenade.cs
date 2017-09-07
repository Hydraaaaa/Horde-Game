using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchedGrenade : MonoBehaviour
{
    [Tooltip("Time until explosion")]
    public float time;

    public int damage;
    public float range;

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

        foreach (Collider col in Physics.OverlapSphere(transform.position, range / 2))
        {
            if (col.GetComponent<Health>() != null)
            {
                int calculatedDamage = Mathf.FloorToInt((range - Vector3.Distance(transform.position, col.ClosestPoint(transform.position))) / range * damage);
                col.GetComponent<Health>().Damage(calculatedDamage);
            }
        }

        Destroy(gameObject);
    }
}
