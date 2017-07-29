using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInit : NetworkBehaviour
{
    public GameObject cameraPrefab;

	void Start ()
    {
        if (isLocalPlayer)
        {
            GameObject camera = Instantiate(cameraPrefab);
            camera.GetComponent<CameraMovement>().player = gameObject;
            GetComponent<PlayerMovement>().camera = camera;
        }

        transform.GetChild(0).SetParent(null);

        Destroy(this);
	}
}
