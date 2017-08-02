using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    public Camera SingleScreenCam;

    public GameObject player1;
    public GameObject player2;


    public bool HorizontalSplit = true;

    public float screenResX;
    public float screenResY;

    public float halfScreen;
    public float quaterScreen;
    public float cameraDistance;
    public float minimumCamDistance = 10;

    // Use this for initialization
    void Start ()
    {
        screenResX = Screen.currentResolution.width;
        screenResY = Screen.currentResolution.height;

        // the beginning of the second quater and end of the third quarter of the screen
        halfScreen = screenResX / 2;
        quaterScreen = halfScreen / 2;
	}
	
	// Update is called once per frame
	void Update ()
    {
        SingleScreenCam.transform.eulerAngles = new Vector3(45, 45, 0);
        cameraDistance = minimumCamDistance + Vector3.Distance(player1.transform.position, player2.transform.position);
        SingleScreenCam.transform.position = ((player1.transform.position + player2.transform.position) / 2) - (SingleScreenCam.transform.forward * cameraDistance);

    }
}
