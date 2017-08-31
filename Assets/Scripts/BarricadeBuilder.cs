using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeBuilder : MonoBehaviour
{
    public int StartHeight = 15;
    public int speed = 1;
    private float EndHeight;
    public float DeathHeight = -2f;
    public bool Built = false;

    public float wantedX = 7;
    public float wantedZ = 0.5f;

    public GameObject Barrier;
    public GameObject Holo;
    public Health health;

    // Use this for initialization
    void Start()
    {
        EndHeight = Barrier.transform.localPosition.y;
        Barrier.transform.localPosition = new Vector3(Barrier.transform.localPosition.x, StartHeight, Barrier.transform.localPosition.z);
        health = GetComponent<Health>();
        Holo.transform.localScale = new Vector3(wantedX, Holo.transform.localScale.y, wantedZ);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Built == true)
        {
            Holo.GetComponent<MeshRenderer>().enabled = false;
            // Vector3 endpos = new Vector3(Barrier.transform.position.x, EndHeight, Barrier.transform.position.z);
            if (Barrier.transform.localPosition.y - speed >= EndHeight)
                Barrier.transform.localPosition = new Vector3(Barrier.transform.localPosition.x, Barrier.transform.localPosition.y - speed, Barrier.transform.localPosition.z);
            else
            {
                if (Barrier.transform.localScale.x < wantedX)
                {
                    Barrier.transform.localScale = new Vector3(Barrier.transform.localScale.x + 0.25f, Barrier.transform.localScale.y, Barrier.transform.localScale.z);
                }
                if (Barrier.transform.localScale.z > wantedZ)
                {                    
                    Barrier.transform.localScale = new Vector3(Barrier.transform.localScale.x, Barrier.transform.localScale.y, Barrier.transform.localScale.z - 0.1f);
                }
            }
        }
        else
        {
            Barrier.transform.localPosition = new Vector3(Barrier.transform.localPosition.x, StartHeight, Barrier.transform.localPosition.z);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && Built == false)
        {
            if (GameObjectManager.instance != null && GameObjectManager.instance.players.Count > 0)
            {
                if (Input.GetButtonDown("Joy1XButton"))
                {
                    Built = true;
                }
                if (Input.GetButtonDown("Joy2XButton"))
                {
                    Built = true;
                }
            }
        }
    }
}
