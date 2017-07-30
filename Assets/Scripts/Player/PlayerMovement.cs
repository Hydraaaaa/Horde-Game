using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    public GameObject camera;
    public float moveSpeed;

    CharacterController controller;

    [SyncVar]
    Vector3 direction;


    void Start()
    {
        controller = GetComponent<CharacterController>();
	}
	
	void Update()
    {
        if (isLocalPlayer)
        {
            direction = Vector3.zero;

            Vector3 forward = Vector3.Cross(camera.transform.right, Vector3.up);

            if (Input.GetKey(KeyCode.W))
                direction += forward * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                direction -= forward * moveSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.A))
                direction -= camera.transform.right * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                direction += camera.transform.right * moveSpeed * Time.deltaTime;

            CmdSyncMovement(direction);
        }

        controller.Move(direction);

        if (isLocalPlayer)
            camera.GetComponent<CameraMovement>().UpdatePosition();
    }

    [Command]
    void CmdSyncMovement(Vector3 dir)
    {
        direction = dir;
    }
}
