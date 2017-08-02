using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(LineRenderer))]
public class Tracer : NetworkBehaviour
{
    [HideInInspector][SyncVar] public Vector3 startPos;
    [HideInInspector][SyncVar] public Vector3 endPos;

	void Start ()
    {
        Debug.Log("Start");
        LineRenderer renderer = GetComponent<LineRenderer>();
        renderer.SetPosition(0, startPos);
        renderer.SetPosition(1, endPos);
    }
	
	void Update ()
    {
		
	}
}
