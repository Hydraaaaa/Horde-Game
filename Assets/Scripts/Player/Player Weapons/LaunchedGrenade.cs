using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchedGrenade : MonoBehaviour
{
    [Tooltip("Time until explosion")]
    public float time;

    public int damage;

    public GameObject[] particleEffects;

    List<GameObject> targets;

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

        foreach (GameObject target in targets)
            target.GetComponent<Health>().Damage(damage);

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>())
            targets.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject);
    }
}
