using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    NavMeshAgent agent;

    public GameObject player;

	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update ()
    {
        agent.SetDestination(player.transform.position);
	}
}
