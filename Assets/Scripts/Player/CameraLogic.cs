﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    public GameObjectManager gameObjectManager;
    
    public float minDistance = 10;
    public float maxDistance = 100;
    public float zoomOutMultiplier;

    public float lerpSpeed;

    void Start()
    {
        transform.eulerAngles = new Vector3(45, 45, 0);

        if (gameObjectManager.players.Count > 0)
        {
            Vector3 avgPos = Vector3.zero;

            for (int i = 0; i < gameObjectManager.players.Count; i++)
                avgPos += gameObjectManager.players[i].gameObject.transform.position;

            avgPos /= gameObjectManager.players.Count;

            float longestDistance = 0;

            for (int i = 0; i < gameObjectManager.players.Count; i++)
            {
                if (Vector3.Distance(avgPos, gameObjectManager.players[i].gameObject.transform.position) > longestDistance)
                    longestDistance = Vector3.Distance(avgPos, gameObjectManager.players[i].gameObject.transform.position);
            }

            float cameraDistance = Mathf.Clamp(longestDistance * zoomOutMultiplier, minDistance, maxDistance);

            transform.position = avgPos - transform.forward * cameraDistance;
        }
    }
	
	void Update ()
    {
        transform.eulerAngles = new Vector3(45, 45, 0);

        if (gameObjectManager.players.Count > 0)
        {
            Vector3 avgPos = Vector3.zero;

            for (int i = 0; i < gameObjectManager.players.Count; i++)
                avgPos += gameObjectManager.players[i].gameObject.transform.position;

            avgPos /= gameObjectManager.players.Count;

            float longestDistance = 0;

            for (int i = 0; i < gameObjectManager.players.Count; i++)
            {
                if (Vector3.Distance(avgPos, gameObjectManager.players[i].gameObject.transform.position) > longestDistance)
                    longestDistance = Vector3.Distance(avgPos, gameObjectManager.players[i].gameObject.transform.position);
            }

            float cameraDistance = Mathf.Clamp(longestDistance * zoomOutMultiplier, minDistance, maxDistance);

            transform.position = Vector3.Lerp(transform.position, avgPos - transform.forward * cameraDistance, Time.deltaTime * lerpSpeed);
        }
    }
}
