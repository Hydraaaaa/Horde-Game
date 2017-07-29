using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
	
	void Update ()
    {
        transform.position = target.transform.position + offset;
	}
}
