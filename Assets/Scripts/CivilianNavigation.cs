using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class CivilianNavigation : MonoBehaviour
{
    public GameObjectManager gameObjectManager;
    UnityEngine.AI.NavMeshAgent agent;

    public float speed;

	void Start ()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = speed;
	}
	
	void Update ()
    {
        if (gameObjectManager != null)
        {
            agent.SetDestination(gameObjectManager.endPos.transform.position);
        }
	}
}
