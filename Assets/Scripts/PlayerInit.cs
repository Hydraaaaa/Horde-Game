using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
	void Start ()
    {
        Transform camera = transform.GetChild(0);
        camera.GetComponent<CameraMovement>().player = gameObject;
        camera.SetParent(null);

        GetComponent<PlayerMovement>().camera = camera.gameObject;

        Destroy(this);
	}
}
