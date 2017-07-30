using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class EnemyNavigation : MonoBehaviour
{
    public enum Type { SURVIVOR, PLAYER, BARRICADE, DEFENSES, TERRAIN, NULL };

    [Serializable]
    public struct ObjTags
    {
        public string BarricadeTag;
        public string SurvivorTag;
        public string PlayerTag;
        public string DefensesTag;
        public string TerrainTag;
    }

    public ObjTags TypeTags;
    public Type[] Priority = new Type[5];


    private NavMeshAgent agent;

    public Vector3 TargetPos = Vector3.zero;
    public GameObject EndPos;

    public NavMeshPath path;

    public bool followPlayer = false;
    public bool followSurvivor = false;

    public GameObject player;
    public GameObject survivor;

    public float turningSpeed = 1.0f;
    public float acceleration = 1.0f;

    void Start()
    {
        path = new NavMeshPath();
        // Set the zombies target to the end of the map
        TargetPos = EndPos.transform.position;

        // Get reference to the zombies agent
        agent = GetComponent<NavMeshAgent>();
        agent.CalculatePath(TargetPos, path);

        //player = GameObject.FindGameObjectWithTag("Player 1");
    }

    void Update()
    {
        agent.angularSpeed = turningSpeed;
        agent.acceleration = acceleration;

        // If the agent dosent have a path to follow
        if (!agent.hasPath)
        {
            TargetPos = EndPos.transform.position;
            agent.SetDestination(TargetPos);
        }

        // If the agent is following a player
        if (player != null)
            PlayerNotNull();

        if (survivor != null)
            SurvivorNotNull();

        // If the agent has a path to follow
        if (agent.hasPath)
        {
            agent.SetDestination(TargetPos);
        }
    }

    void PlayerNotNull()
    {
        int layermask = 1 << 10;
        layermask = ~layermask;

        if (!player.activeSelf)
        {
            // Tell the AI to travel where the player was so it can track to last known position
            TargetPos = player.transform.position;

            // then remove the player reference so it dosent keep tracking to them
            followPlayer = false;
            player = null;
        }
        // If the AI loses sight of the player
        else if (Physics.Linecast(this.transform.position, player.transform.position, layermask, QueryTriggerInteraction.Ignore))
        {
            // Tell the AI to travel where the player was so it can track to last known position
            TargetPos = player.transform.position;

            // then remove the player reference so it dosent keep tracking to them
            followPlayer = false;
            player = null;
        }
        // Else if the player is visible
        else
        {
            // Set the target position of the AI to the position of the player
            TargetPos = player.transform.position;
            agent.SetDestination(TargetPos);
        }
    }

    void SurvivorNotNull()
    {

    }

    void OnTriggerStay(Collider col)
    {
        bool checkedAndFound = false;

        // Check the tag against an enum of types
        for (int i = 0; i < Priority.Length; i++)
        {
            switch (Priority[i])
            {
                case Type.BARRICADE:
                    if (col.tag == TypeTags.BarricadeTag)
                    {
                        CheckForBarricade(col);
                        checkedAndFound = true;
                    }
                    break;
                case Type.DEFENSES:
                    if (col.tag == TypeTags.DefensesTag)
                    {
                        CheckForDefenses(col);
                        checkedAndFound = true;
                    }
                    break;
                case Type.PLAYER:
                    if (col.tag == TypeTags.PlayerTag)
                    {
                        CheckForPlayer(col);
                        checkedAndFound = true;
                    }
                    break;
                case Type.SURVIVOR:
                    if (col.tag == TypeTags.SurvivorTag)
                    {
                        CheckForSurvivor(col);
                        checkedAndFound = true;
                    }
                    break;
                case Type.TERRAIN:
                    if (col.tag == TypeTags.TerrainTag)
                    {
                        CheckForTerrain(col);
                        checkedAndFound = true;
                    }
                    break;
            }
            if (checkedAndFound)
                break;
        }
    }

    void CheckForBarricade(Collider col)
    {

    }

    void CheckForTerrain(Collider col)
    {

    }

    void CheckForDefenses(Collider col)
    {

    }

    void CheckForPlayer(Collider col)
    {
        int layermask = 1 << 10;
        layermask = ~layermask;
        if (!Physics.Linecast(this.transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
        {
            // If this player is closer than the other player in the scene
            if (player == null)
            {
                player = col.gameObject;
                followPlayer = true;
                TargetPos = col.transform.position;
                agent.SetDestination(TargetPos);
            }
            else if (Vector3.Distance(this.transform.position, player.transform.position) >
                    Vector3.Distance(this.transform.position, col.transform.position))
            {
                player = col.gameObject;
                followPlayer = true;
                TargetPos = col.transform.position;
                agent.SetDestination(TargetPos);
            }
        }
    }

    void CheckForSurvivor(Collider col)
    {
        int layermask = 1 << 10;
        layermask = ~layermask;
        // Can they see a Survivor && is the survivor in range
        if (!Physics.Linecast(this.transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
        {
            // If the agent isnt following a survivor already
            if (survivor == null)
            {
                // Remove any prior priority to player tracking
                player = null;
                followPlayer = false;

                // Set the survivor reference
                survivor = col.gameObject;
                followSurvivor = true;

                // Set survivor position
                TargetPos = col.transform.position;
                agent.SetDestination(TargetPos);
            }
            // If there is another survivor within range of the agent
            else if (Vector3.Distance(this.transform.position, survivor.transform.position) >
                    Vector3.Distance(this.transform.position, col.transform.position))
            {
                survivor = col.gameObject;
                followSurvivor = true;
                TargetPos = col.transform.position;
                agent.SetDestination(TargetPos);
            }
        }
    }
}
