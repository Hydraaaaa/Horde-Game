using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject camera;
    public float moveSpeed;

    CharacterController controller;


	void Start()
    {
        controller = GetComponent<CharacterController>();
	}
	
	void Update()
    {
        Vector3 movement = Vector3.zero;

        Vector3 forward = Vector3.Cross(camera.transform.right, Vector3.up);

        if (Input.GetKey(KeyCode.W))
            movement += forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            movement -= forward * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            movement -= camera.transform.right * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            movement += camera.transform.right * moveSpeed * Time.deltaTime;

        controller.Move(movement);

        camera.GetComponent<CameraMovement>().UpdatePosition();
    }
}
