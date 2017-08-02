using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogic : MonoBehaviour
{
    public float ControllerMenuAxisSensitivity = 0.75f;
    public float ControllerNextInputCheckTimeGap = 0.2f;
    public float timeBetweenMovement;

    public string[] Joysticks;

    public Vector2 p1Axis;
    public float p1Trigg;

	// Use this for initialization
	void Start ()
    {
        Joysticks = Input.GetJoystickNames();
	}

    bool hasShot = false;

    // Update is called once per frame
    void Update ()
    {



        // Left Thumbstick
        if (timeBetweenMovement < ControllerNextInputCheckTimeGap)
        {
            timeBetweenMovement += Time.deltaTime;
        }
        else if (timeBetweenMovement >= ControllerNextInputCheckTimeGap)
        {
            CheckAxis();
        }

        // Right Trigger
        if (Input.GetAxis("Joy1Shoot") < 0.3f && Input.GetAxis("Joy1Shoot") > -0.3f)
        {
            hasShot = false;
        }

        if (Input.GetAxis("Joy1Shoot") < -0.7f && !hasShot)
        {
            Debug.Log("Bang!");
            hasShot = true;
        }
    }

    void CheckAxis()
    {
        p1Axis = new Vector2(Input.GetAxis("Joy1Horizontal"), Input.GetAxis("Joy1Vertical"));
        p1Trigg = (Input.GetAxis("Joy1Shoot"));
        if (Input.GetAxis("Joy1Horizontal") != 0)
        {
            if (Input.GetAxis("Joy1Horizontal") >= ControllerMenuAxisSensitivity)
            {
                Debug.Log("Right");
                timeBetweenMovement = 0;
            }
            else if (Input.GetAxis("Joy1Horizontal") <= -ControllerMenuAxisSensitivity)
            {
                Debug.Log("Left");
                timeBetweenMovement = 0;
            }
        }
        if (Input.GetAxis("Joy1Vertical") != 0)
        {
            if (Input.GetAxis("Joy1Vertical") >= ControllerMenuAxisSensitivity)
            {
                Debug.Log("Up");
                timeBetweenMovement = 0;
            }
            else if (Input.GetAxis("Joy1Vertical") <= -ControllerMenuAxisSensitivity)
            {
                Debug.Log("Down");
                timeBetweenMovement = 0;
            }
        }
    }
}
