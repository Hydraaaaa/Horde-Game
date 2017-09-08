using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidScript : MonoBehaviour
{
    public List<GameObject> AllNear;

	// Update is called once per frame
	void Update ()
    {
	    	
	}

    public void AttackAllInRange(int damage)
    {
        for (int i = 0; i < AllNear.Count; i++)
        {
            if (AllNear[i].GetComponent<Health>() != null)
            AllNear[i].GetComponent<Health>().Damage(damage);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //bool notAdded = true;

            for (int i = 0; i < AllNear.Count; i++)
            {
                if (AllNear[i] == col.gameObject)
                {
                    return;
                }
            }

            AllNear.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        for (int i = 0; i < AllNear.Count; i++)
        {
            if (AllNear[i] == col.gameObject)
            {
                AllNear.RemoveAt(i);
            }
        }
    }
}
