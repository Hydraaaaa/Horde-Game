using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    public GameObject cameraPrefab;

	void Start ()
    {
        //GameObject camera = Instantiate(cameraPrefab);
        //camera.GetComponent<CameraMovement>().player = gameObject;
        //GetComponent<PlayerMovement>().camera = camera;

        transform.GetChild(0).SetParent(null);

        Destroy(this);
	}
}
