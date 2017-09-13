using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public new GameObject camera;
    public float moveSpeed;

    CharacterController controller;
    
    Vector3 direction;


    void Start()
    {
        controller = GetComponent<CharacterController>();
	}
	
	void Update()
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

        controller.Move(direction);
        
        camera.GetComponent<CameraMovement>().UpdatePosition();
    }
}
