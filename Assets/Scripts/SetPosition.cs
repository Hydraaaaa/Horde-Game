using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
	
	void Update ()
    {
        if (target != null)
            transform.position = target.transform.position + offset;
	}
}
