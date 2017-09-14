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
            Instantiate(particleEffect, transform.position, particleEffect.transform.rotation);

        Collider[] cols = Physics.OverlapSphere(transform.position, range / 2);
        foreach (Collider col in cols)
        {
            if (col.isTrigger)
                continue;

            if (col.GetComponent<Health>() != null)
            {
                int calculatedDamage = Mathf.FloorToInt((range - Vector3.Distance(transform.position, col.ClosestPoint(transform.position))) / range * damage);
                col.GetComponent<Health>().Damage(calculatedDamage);
            }
        }

        Destroy(gameObject);
    }
}
