using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    public Vector3 pos;
    public bool localPos;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (localPos)
            transform.localPosition = pos;
        else
            transform.position = pos;
	}
}
