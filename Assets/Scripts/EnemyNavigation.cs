using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    NavMeshAgent agent;

    public Vector3 TargetPos = Vector3.zero;
    public GameObject EndPos;

    public bool followPlayer = false;
    public GameObject player;
    public float turningSpeed = 1.0f;
    public float acceleration = 1.0f;

    void Start()
    {
        // Set the zombies target to the end of the map
        TargetPos = EndPos.transform.position;

        // Get reference to the zombies agent
        agent = GetComponent<NavMeshAgent>();
        agent.ResetPath();

        player = GameObject.FindGameObjectWithTag("Player 1");
    }

    void Update()
    {
        agent.angularSpeed = turningSpeed;
        agent.acceleration = acceleration;

        // If the agent dosent have a path to follow
        if (!agent.hasPath)
        {
            TargetPos = EndPos.transform.position;
        }

        if (player != null)
        {
            // If the AI loses sight of the player
            if (!Physics.Linecast(this.transform.position, player.transform.position))
            {
                // Tell the AI to travel where the player was
                TargetPos = player.transform.position;

                // then remove the player so it dosent keep tracking to them
                player = null;
            }
        }

        // If the agent has a path to follow
        if (agent.hasPath)
        {
            agent.SetDestination(TargetPos);
        }
    }

    void OnTriggerStay(Collider col)
    {
        // Can they see the player && are the players in range
        if (Physics.Linecast(this.transform.position, col.transform.position))
        {
            // If this player is closer than the other player in the scene
            if (player == null)
            {
                player = col.gameObject;
                followPlayer = true;
            }
            else if (Vector3.Distance(this.transform.position, player.transform.position) > 
                    Vector3.Distance(this.transform.position, col.transform.position))
            {
                player = col.gameObject;
                followPlayer = true;
            }
        }
    }
}
