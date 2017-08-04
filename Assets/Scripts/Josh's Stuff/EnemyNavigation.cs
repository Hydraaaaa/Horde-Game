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


    public  NavMeshAgent agent;

    public Vector3 TargetPos = Vector3.zero;
    public GameObject EndPos;

    public NavMeshPath path;

    public bool followPlayer = false;
    public bool followSurvivor = false;

    public GameObject player;
    public GameObject survivor;
    public GameObject barricade;

    public float turningSpeed = 1.0f;
    public float acceleration = 1.0f;

    public int damage;
    public float attackRange;

    [Tooltip("Time in seconds between shots")]
    public float cooldown;
    float currentCooldown;

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
        // Attack Cooldown
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        if (agent != null)
        {
            // Setting speed and turning speed
            agent.angularSpeed = turningSpeed;
            agent.acceleration = acceleration;

            // Rotating towards movement direction
            Vector3 dir = this.GetComponent<NavMeshAgent>().velocity;

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * turningSpeed
                );
            }

            // If the agent dosent have a path to follow
            if (!agent.hasPath)
            {
                //TargetPos = EndPos.transform.position;
                agent.SetDestination(TargetPos);
            }

            // If the agent is following a player
            if (player != null)
                PlayerNotNull();

            // If the agent is following a Survivor
            if (survivor != null)
                SurvivorNotNull();

            // If the agent is following a Barricade
            if (barricade != null)
                BarricadeNotNull();

            if (barricade == null &&
                survivor == null &&
                player == null)
            {
                TargetPos = EndPos.transform.position;
                agent.SetDestination(TargetPos);
            }

            // If the agent has a path to follow
            if (agent.hasPath)
            {
                agent.SetDestination(TargetPos);
            }
        }
    }

    void PlayerNotNull()
    {
        // Grab the players layer
        int layermask = 1 << LayerMask.NameToLayer("Seethrough");
        layermask = ~layermask;

        //// If the AI loses sight of the player
        //if (Physics.Linecast(this.transform.position, player.transform.position, layermask, QueryTriggerInteraction.Ignore))
        //{
        //    // Tell the AI to travel where the player was so it can track to last known position
        //    TargetPos = player.transform.position;

        //    // then remove the player reference so it dosent keep tracking to them
        //    followPlayer = false;
        //    player = null;

        //    colo = Color.green;
        //}
        //// Else if the player is visible
        //else
        //{
        //    Debug.DrawLine(transform.position, player.transform.position, new Color(1, 0, 0));
        //    // Set the target position of the AI to the position of the player
        //    TargetPos = player.transform.position;
        //    agent.SetDestination(TargetPos);
        //}

        if (Vector3.Distance(transform.position, player.transform.position) > GetComponent<SphereCollider>().radius * 3)
        {
            // then remove the player reference so it dosent keep tracking to them
            followPlayer = false;
            player = null;
        }
        else
        {
            // Set the target position of the AI to the position of the player
            TargetPos = player.transform.position;
            agent.SetDestination(TargetPos);
        }
        // If the player is within attack range
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            if (player.GetComponent<Health>().health > 0)
            {
                Attack(player);

                if (player.GetComponent<Health>().health <= 0)
                {
                    // then remove the player reference so it dosent keep tracking to them
                    followPlayer = false;
                    player = null;
                    TargetPos = EndPos.transform.position;
                    agent.SetDestination(TargetPos);
                }
            }
            else
            {
                // then remove the player reference so it dosent keep tracking to them
                followPlayer = false;
                player = null;
                TargetPos = EndPos.transform.position;
                agent.SetDestination(TargetPos);
            }
        }

        // if the player object is turned off
        else if (player.activeSelf == false)
        {
            // Tell the AI to travel where the player was so it can track to last known position
            TargetPos = player.transform.position;

            // then remove the player reference so it dosent keep tracking to them
            followPlayer = false;
            player = null;
        }
    }

    void SurvivorNotNull()
    {
        // If the player is within attack range
        if (Vector3.Distance(transform.position, survivor.transform.position) < attackRange)
        {
            Attack(survivor);
        }

        if (survivor.GetComponent<Health>().health <= 0)
        {
            // Remove the barricade referance and start going to exit again
            survivor = null;
            TargetPos = EndPos.transform.position;
        }
        else
        {
            TargetPos = survivor.transform.position;
            agent.SetDestination(TargetPos);
        }
    }

    void BarricadeNotNull()
    {
        // If the player is within attack range
        if (Vector3.Distance(transform.position, barricade.transform.position) < attackRange)
        {
            Attack(barricade);
        }

        // If the barricade is dead
        if (barricade.GetComponent<Health>().health <= 0)
        {
            // Remove the barricade referance and start going to exit again
            barricade = null;
            TargetPos = EndPos.transform.position;
        }
        else
        {
            TargetPos = barricade.transform.position;
            agent.SetDestination(TargetPos);
        }
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
        if (barricade == null)
        {
            barricade = col.gameObject;
            player = null;
            survivor = null;
            followPlayer = false;
            TargetPos = col.transform.position;
            agent.SetDestination(TargetPos);

        }
    }

    void CheckForTerrain(Collider col)
    {

    }

    void CheckForDefenses(Collider col)
    {

    }

    void CheckForPlayer(Collider col)
    {
        int layermask = 1 << 9;
        layermask = ~layermask;

        if (survivor == null && barricade == null)
        {

            if (!Physics.Linecast(transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Can see player");
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
    }

    void CheckForSurvivor(Collider col)
    {
        int layermask = 1 << 9;
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

    void Attack(GameObject obj)
    {
        if (currentCooldown <= 0)
        {
            currentCooldown = cooldown;
            obj.GetComponent<Health>().Damage(damage);
        }
    }
}
