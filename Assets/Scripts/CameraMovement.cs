using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;

    public float cameraAngle;
    public float cameraDistance;
    public float mouseOffsetScale;

	void Start ()
    {
        UpdatePosition();
	}
	
	public void UpdatePosition()
    {
        transform.eulerAngles = new Vector3(cameraAngle, 45, 0);

        //Debug.Log(Input.mousePosition);
        Vector2 mouseOffset2D = new Vector2(
                                (Input.mousePosition.x - Screen.width / 2) / Screen.width,
                                (Input.mousePosition.y - Screen.height / 2) / Screen.height);

        Vector3 mouseLookOffset =   Vector3.Cross(transform.right, Vector3.up) * mouseOffset2D.y +
                                    transform.right * mouseOffset2D.x;


        transform.position = player.transform.position - (transform.forward * cameraDistance) + mouseLookOffset * mouseOffsetScale;
	}
}
