﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLogic : MonoBehaviour
{
    public GameObject trackingPlayer = null;

    public int ResourceGain = 0;

    public int TrackingTo = -1;
    public float TrackSpeed = 5f;
    public float ScaleSpeed = 1f;
    public float rotationSpeed = 0.5f;

    public GameObject p1;
    public GameObject p2;

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (trackingPlayer != null)
        {
            // Lerp to the player
            Vector3 lerpPos = Vector3.Lerp(transform.position, trackingPlayer.transform.position, Time.deltaTime * TrackSpeed);
            lerpPos.y = transform.position.y;
            transform.position = lerpPos;

            // Lerp the scale of the object when it reaches the player
            Vector3 tempV = transform.localScale;
            tempV = new Vector3(tempV.x - ScaleSpeed, tempV.y - ScaleSpeed, tempV.z - ScaleSpeed);
            transform.localScale = tempV;

            // If the object has crossed the threshhold
            if (tempV.x <= 0 &&
                tempV.y <= 0 &&
                tempV.z <= 0)
            {
                // Give the resources and delete the object
                if (trackingPlayer.GetComponent<BarrierPlayersideLogic>().ResourceCap >= trackingPlayer.GetComponent<BarrierPlayersideLogic>().Resources + ResourceGain)
                {
                    trackingPlayer.GetComponent<BarrierPlayersideLogic>().Resources += ResourceGain;
                    ScoreManager.instance.ScrapPickup(trackingPlayer);
                }
                DestroyImmediate(this.gameObject);
            }
        }

        else
        {
            transform.Rotate(rotationSpeed, 0, 0);
            p1.transform.Rotate(rotationSpeed, rotationSpeed, rotationSpeed);
            p2.transform.Rotate(-(rotationSpeed / 2), -(rotationSpeed / 2), -(rotationSpeed / 2));
        }
    }

    void OnTriggerEnter(Collider col)
    {

        if (trackingPlayer == null && col.CompareTag("Player"))
        {
            if (col.gameObject == GameObjectManager.instance.players[0].gameObject)
            {
                trackingPlayer = col.gameObject;
                TrackingTo = 1;
            }
            else if (col.gameObject == GameObjectManager.instance.players[1].gameObject)
            {
                trackingPlayer = col.gameObject;
                TrackingTo = 2;
            }
        }
    }
}
