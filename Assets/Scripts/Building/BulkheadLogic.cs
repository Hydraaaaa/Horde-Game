using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulkheadLogic : MonoBehaviour
{
    public float speed;
    public bool IsOpen = false;
    public GameObject topDoor;
    public GameObject bottomDoor;
    public Camera BulkheadCam;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Health>().OnDie = Open;
        BulkheadCam = Instantiate(BulkheadCam);
        BulkheadCam.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            IsOpen = !IsOpen;
            GetComponent<NavMeshObstacle>().enabled = !GetComponent<NavMeshObstacle>().enabled;
        }

        if (IsOpen)
        {
            topDoor.transform.localPosition = Vector3.Lerp(topDoor.transform.localPosition, new Vector3(-0.01990428f, 3.2f, 0), Time.deltaTime * speed);
            bottomDoor.transform.localPosition = Vector3.Lerp(bottomDoor.transform.localPosition, new Vector3(-0.04789639f, -0.73f, 0), Time.deltaTime * speed);

            if (!GetComponent<BarrierLogic>().vital)
                BulkheadCam.gameObject.SetActive(true);
        }
        else
        {
            topDoor.transform.localPosition = Vector3.Lerp(topDoor.transform.localPosition, new Vector3(-0.01990428f, 1.972778f, 0), Time.deltaTime * speed);
            bottomDoor.transform.localPosition = Vector3.Lerp(bottomDoor.transform.localPosition, new Vector3(-0.04789639f, 0.7159657f, 0), Time.deltaTime * speed);

            if (!GetComponent<BarrierLogic>().vital)
                BulkheadCam.gameObject.SetActive(false);
        }
    }

    public void Open()
    {
        IsOpen = true;
        GetComponent<NavMeshObstacle>().enabled = false;
        foreach (BoxCollider col in GetComponents<BoxCollider>())
        {
            col.enabled = false;
        }
    }
}
