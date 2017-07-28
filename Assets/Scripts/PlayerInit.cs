using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
	void Start ()
    {
        Transform camera = transform.GetChild(0);
        camera.SetParent(null);

        Destroy(this);
	}
}
