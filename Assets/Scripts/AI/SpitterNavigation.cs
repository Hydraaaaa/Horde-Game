using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SpitterNavigation : MonoBehaviour
{
    public enum Type { SURVIVOR, PLAYER, BARRICADE, DEFENSES, TERRAIN, NULL };

    public Animator anim;

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

    // Agent Data
    public NavMeshAgent agent;
    public NavMeshPath path;
    public Vector3 TargetPos = Vector3.zero;
    public GameObject EndPos;

    // Follow Bools
    public bool followPlayer = false;
    public bool followSurvivor = false;
    public bool playerGrabbed = false;

    // Gameobject Target Refs
    public GameObject player;
    public GameObject survivor;
    public GameObject barricade;
    public GameObject Tongue;
    public GameObject HoldPoint;
    public AcidScript spitRef;

    // Transforms
    //public Transform HoldingPos;

    // Speeds
    public float turningSpeed = 4.0f;
    public float acceleration = 4.0f;
    public float speed = 4.0f;

    // Damages
    public int damage;
    public int grabDamage;
    public int acidDamage;

    // Attack Ranges
    public float attackRange;
    public float maxGrabRange;
    public float currentDist;

    public float struggleVal;
    public float strugglePerHit = 1.5f;
    public float struggleLossPerFrame = 0.1f;
    public float struggleMax = 10;

    // Cooldowns
    [Tooltip("Time in seconds between shots")]
    public float pullSpeed;
    public float cooldown;
    public float currentCooldown;

    public float grabCooldown;
    public float currentGrabCooldown;

    public float acidAOECooldown;
    public float currentAcidAOECooldown;

    public bool EnvironmentZombie = false;
    public bool OldSpitter = false;

    Vector3 pDir;


    void Start()
    {
        Physics.IgnoreCollision(GetComponent<SphereCollider>(), GameObjectManager.instance.endPos.GetComponent<BoxCollider>());

        path = new NavMeshPath();

        if (EnvironmentZombie)
        {
            EndPos = gameObject;
            TargetPos = EndPos.transform.position;
        }
        else
        {
            // Set the zombies target to the end of the map
            TargetPos = EndPos.transform.position;
        }
        // Get reference to the zombies agent
        agent = GetComponent<NavMeshAgent>();
        agent.CalculatePath(TargetPos, path);
    }

    void Update()
    {
        if (currentGrabCooldown > 0)
            currentGrabCooldown -= Time.deltaTime;
        
        // Update the time for the acid attack
        if (currentAcidAOECooldown > 0)
        {
            currentAcidAOECooldown -= Time.deltaTime;
        }
        // Deal AOE Acid damage
        else
        {
            currentAcidAOECooldown = acidAOECooldown;
            spitRef.AttackAllInRange(acidDamage);
        }

        Debug.DrawLine(transform.position, TargetPos);

        if (Mathf.Abs(GetComponent<NavMeshAgent>().velocity.x) > 0 &&
            Mathf.Abs(GetComponent<NavMeshAgent>().velocity.z) > 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }

        // Attack Cooldown
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        if (agent != null)
        {
            if (!playerGrabbed)
            {
                // anim.SetBool("Charging", false);

                // Setting speed and turning speed
                agent.angularSpeed = turningSpeed;
                agent.acceleration = acceleration;
                agent.speed = speed;

                // Rotating towards movement direction
                Vector3 dir = this.GetComponent<NavMeshAgent>().velocity;

                // If the agent is moving
                if (dir != Vector3.zero)
                {
                    // Rotate in the direction of the velocity
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(dir),
                        Time.deltaTime * (turningSpeed * 2)
                    );
                }

                // If the agent dosent have a path to follow
                if (!agent.hasPath)
                {
                    //TargetPos = EndPos.transform.position;
                    agent.SetDestination(TargetPos);
                }
                // If the agent isnt under attack
                if (GetComponent<Health>().Attacker == null)
                {
                    // If the agent is following a player
                    if (player != null)
                        PlayerNotNull();

                    // If the agent is following a Survivor
                    else if (survivor != null)
                        SurvivorNotNull();

                    // If the agent is following a Barricade
                    else if (barricade != null)
                        BarricadeNotNull();

                    // If the agent isnt following anything
                    else
                    {
                        TargetPos = EndPos.transform.position;
                        agent.SetDestination(TargetPos);
                    }
                }
                else
                {
                    BeingAttacked();
                }
                // Set Start
                Tongue.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                // Set End
                Tongue.GetComponent<LineRenderer>().SetPosition(1, transform.position);
            }
            else
            {
                ContinueGrab();
            }
        }
        // If the agent has a path to follow
        if (agent.hasPath)
        {
            agent.SetDestination(TargetPos);
            currentDist = Vector3.Distance(transform.position, TargetPos);
        }
    }

    void BeingAttacked()
    {
        // If the Attacker is within attack range
        if (Vector3.Distance(transform.position, GetComponent<Health>().Attacker.transform.position) < attackRange)
        {
            if (GetComponent<Health>().Attacker.GetComponent<Health>().health > 0)
            {
                Attack(player);

                if (GetComponent<Health>().Attacker.GetComponent<Health>().health <= 0)
                {
                    // then remove the player reference so it dosent keep tracking to them
                    followPlayer = false;
                    player = null;
                    TargetPos = EndPos.transform.position;
                    agent.SetDestination(TargetPos);
                    GetComponent<Health>().Attacker = null;
                }
            }
            else
            {
                // then remove the player reference so it dosent keep tracking to them
                followPlayer = false;
                player = null;
                TargetPos = EndPos.transform.position;
                agent.SetDestination(TargetPos);
                GetComponent<Health>().Attacker = null;
            }
        }

        if (GetComponent<Health>().Attacker != null)
        {
            // Target pos becomes the attackers position
            TargetPos = GetComponent<Health>().Attacker.transform.position;
            agent.SetDestination(TargetPos);
        }
    }

    void PlayerNotNull()
    {
        // If player is out of range
        if (Vector3.Distance(transform.position, player.transform.position) > GetComponent<SphereCollider>().radius * 3)
        {
            // then remove the player reference so it dosent keep tracking to them
            followPlayer = false;
            player = null;
            return;
        }
        else
        {
            if (OldSpitter)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > (GetComponent<SphereCollider>().radius / 4) * 3)
                {
                    // Set the target position of the AI to the position of the player
                    TargetPos = player.transform.position;
                    agent.SetDestination(TargetPos);
                }
                else
                {
                    // Set the target position so the AI stands still when in range
                    TargetPos = transform.position;
                    agent.SetDestination(TargetPos);
                }
            }
            else
            {
                // Set the target position of the AI to the position of the player
                TargetPos = player.transform.position;
                agent.SetDestination(TargetPos);
            }
        }

        // if the player is in range for a spit attack
        if (Vector3.Distance(transform.position, player.transform.position) < maxGrabRange 
            && playerGrabbed == false)
        {
            int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
            layermask = 1 << LayerMask.NameToLayer("Enemy");
            layermask = ~layermask;

            // If the player is visible
            if (Physics.Linecast(transform.position, player.transform.position, layermask))
            {
                if (currentGrabCooldown <= 0)
                {
                    StartGrab();
                    playerGrabbed = true;
                }
            }
        }
        // If the player is within attack range
        else if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            // If the player needs to be revived
            if (player.GetComponent<ReviveSystem>().NeedRes == true)
            {
                // then remove the player reference so it dosent keep tracking to them
                followPlayer = false;
                player = null;
                return;
            }

            // If the player is still alive
            if (player.GetComponent<Health>().health > 0)
            {
                Attack(player);

                // If the player dies from the attack
                if (player.GetComponent<Health>().health <= 0)
                {
                    // then remove the player reference so it dosent keep tracking to them
                    followPlayer = false;
                    player = null;
                    TargetPos = EndPos.transform.position;
                    agent.SetDestination(TargetPos);
                    return;
                }
            }
            // If the player is dead
            else
            {
                // then remove the player reference so it dosent keep tracking to them
                followPlayer = false;
                player = null;
                TargetPos = EndPos.transform.position;
                agent.SetDestination(TargetPos);
                return;
            }
        }

        // if the player object is turned off
        if (player.activeSelf == false)
        {
            // Tell the AI to travel where the player was so it can track to last known position
            TargetPos = player.transform.position;

            // then remove the player reference so it dosent keep tracking to them
            followPlayer = false;
            player = null;
            return;
        }
    }

    void StartGrab()
    {
        currentGrabCooldown = grabCooldown;

        // Set Start
        Tongue.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        // Set End
        Tongue.GetComponent<LineRenderer>().SetPosition(1, player.transform.position);

        Vector3 dir = player.transform.position - transform.position;
        Quaternion rot = transform.rotation;
        rot.SetLookRotation(new Vector3(dir.x, rot.eulerAngles.y, dir.z));

        transform.rotation = rot;

        //player.GetComponent<PlayerMovScript>().incapacitated = true;
        //Debug.Log("Incapacitated the player");

        // Stop the agent from moving
        TargetPos = transform.position;
        agent.SetDestination(TargetPos);
    }

    void ContinueGrab()
    {
        int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
        layermask = 1 << LayerMask.NameToLayer("Enemy");
        //layermask = ~layermask;

        Debug.DrawLine(transform.position, player.transform.position - (player.transform.position - transform.position).normalized * 2, Color.red);
        // If the player is visible
        if (!Physics.Linecast(transform.position, player.transform.position - ((player.transform.position - transform.position).normalized / 2) * 1.1f))
        {
            // Set Start
            Tongue.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            // Set End
            Tongue.GetComponent<LineRenderer>().SetPosition(1, player.transform.position);

            // Stop the agent from moving
            TargetPos = transform.position;
            agent.SetDestination(TargetPos);

            // Is the player further away from the agent than the last frame?
            if (Vector3.Distance(HoldPoint.transform.position, player.transform.position) - currentDist > 0.1f)
            {
                if (currentDist != 0)
                {
                    // then remove the player reference so it dosent keep tracking to them
                    followPlayer = false;
                    player.GetComponent<PlayerMovScript>().incapacitationLevel = 0;
                    Debug.Log("Removed Incapacitated from the player");

                    player = null;
                    playerGrabbed = false;
                    currentGrabCooldown = grabCooldown;
                    return;
                }
            }
            if (Vector3.Distance(HoldPoint.transform.position, player.transform.position) > 1.1f)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, HoldPoint.transform.position, Time.deltaTime * pullSpeed);
                player.GetComponent<PlayerMovScript>().incapacitationLevel = 1;
            }
            else
            {
                if (OldSpitter)
                {
                    player.GetComponent<PlayerMovScript>().incapacitationLevel = 2;

                    if (struggleVal > 0)
                        struggleVal -= struggleLossPerFrame;

                    // If the player is struggling
                    if (Input.GetButtonDown(player.GetComponent<PlayerMovScript>().playerBeginning + "XButton"))
                    {
                        Debug.Log("Struggling");
                        struggleVal += strugglePerHit;
                    }                    

                    if (player.GetComponent<PlayerMovScript>().playerNumber == 1)
                    {
                        GameObjectManager.instance.HUD.GetComponent<HUDScript>().P1Struggling = true;
                        GameObjectManager.instance.HUD.GetComponent<HUDScript>().minSP1 = struggleVal;
                        GameObjectManager.instance.HUD.GetComponent<HUDScript>().maxSP1 = struggleMax;
                    }
                    if (player.GetComponent<PlayerMovScript>().playerNumber == 2)
                    {
                        GameObjectManager.instance.HUD.GetComponent<HUDScript>().P2Struggling = true;
                        GameObjectManager.instance.HUD.GetComponent<HUDScript>().minSP2 = struggleVal;
                        GameObjectManager.instance.HUD.GetComponent<HUDScript>().maxSP2 = struggleMax;
                    }

                    if (struggleVal >= struggleMax)
                    {
                        if (player.GetComponent<PlayerMovScript>().playerNumber == 1)
                        {
                            GameObjectManager.instance.HUD.GetComponent<HUDScript>().P1Struggling = false;
                        }
                        if (player.GetComponent<PlayerMovScript>().playerNumber == 2)
                        {
                            GameObjectManager.instance.HUD.GetComponent<HUDScript>().P2Struggling = false;
                        }

                        // then remove the player reference so it dosent keep tracking to them
                        followPlayer = false;
                        player.GetComponent<PlayerMovScript>().incapacitationLevel = 0;
                        Debug.Log("Player Broke Free: Removed Incapacitated from the player");

                        player = null;
                        playerGrabbed = false;
                        currentGrabCooldown = grabCooldown;
                        struggleVal = 0;
                        
                        return;
                    }

                    player.transform.LookAt(transform);

                    player.GetComponent<PlayerMovScript>().incapacitationLevel = 2;
                    player.transform.position = HoldPoint.transform.position;

                    // If the player is within attack range
                    if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                    {
                        // If the player needs to be revived
                        if (player.GetComponent<ReviveSystem>().NeedRes == true)
                        {
                            // then remove the player reference so it dosent keep tracking to them
                            followPlayer = false;
                            player.GetComponent<PlayerMovScript>().incapacitationLevel = 0;
                            Debug.Log("Player needs Revive: Removed Incapacitated from the player");

                            player = null;
                            playerGrabbed = false;
                            currentGrabCooldown = grabCooldown;
                            return;
                        }

                        // If the player is still alive
                        if (player.GetComponent<Health>().health > 0)
                        {
                            Attack(player);

                            // If the player dies from the attack
                            if (player.GetComponent<Health>().health <= 0)
                            {
                                // then remove the player reference so it dosent keep tracking to them
                                followPlayer = false;
                                player.GetComponent<PlayerMovScript>().incapacitationLevel = 0;
                                Debug.Log("Removed Incapacitated from the player");

                                player = null;
                                playerGrabbed = false;
                                TargetPos = EndPos.transform.position;
                                agent.SetDestination(TargetPos);
                                currentGrabCooldown = grabCooldown;
                                return;
                            }
                        }
                        // If the player is dead
                        else
                        {
                            // then remove the player reference so it dosent keep tracking to them
                            followPlayer = false;
                            player.GetComponent<PlayerMovScript>().incapacitationLevel = 0;
                            Debug.Log("Removed Incapacitated from the player");

                            player = null;
                            playerGrabbed = false;
                            TargetPos = EndPos.transform.position;
                            agent.SetDestination(TargetPos);
                            currentGrabCooldown = grabCooldown;
                            return;
                        }
                    }

                    // if the player object is turned off
                    if (player.activeSelf == false)
                    {
                        // Tell the AI to travel where the player was so it can track to last known position
                        TargetPos = player.transform.position;

                        // then remove the player reference so it dosent keep tracking to them
                        followPlayer = false;
                        player.GetComponent<PlayerMovScript>().incapacitationLevel = 0;
                        Debug.Log("Removed Incapacitated from the player");

                        player = null;
                        playerGrabbed = false;
                        TargetPos = EndPos.transform.position;
                        agent.SetDestination(TargetPos);
                        currentGrabCooldown = grabCooldown;
                        return;
                    }
                }
                else
                {
                    // then remove the player reference so it dosent keep tracking to them
                    followPlayer = false;
                    player.GetComponent<PlayerMovScript>().incapacitationLevel = 0;
                    Debug.Log("Removed Incapacitated from the player");

                    playerGrabbed = false;
                    currentGrabCooldown = grabCooldown;
                }
            }
        }
        currentDist = Vector3.Distance(HoldPoint.transform.position, player.transform.position);
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
        // If they ran into another enemy, dont bother continuing
        if (col.CompareTag("Enemy"))
        {
            return;
        }
        //// If they found a barricade
        //if (col.CompareTag(TypeTags.BarricadeTag))
        //{
        //    CheckForBarricade(col);
        //    return;
        //}

        // If the grabber isnt pre occupied with grabbing someone
        if (!playerGrabbed)
        {
            // If they found a player
            if (col.CompareTag(TypeTags.PlayerTag))
            {
                CheckForPlayer(col);
                return;
            }
            // If they found a survivor
            if (col.CompareTag(TypeTags.SurvivorTag))
            {
                CheckForSurvivor(col);
                return;
            }
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

    void CheckForPlayer(Collider col)
    {
        if (col.GetComponent<ReviveSystem>().NeedRes != true && col.GetComponent<Health>().health > 0)
        {
            // Ignore see through things
            int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
            layermask = 1 << LayerMask.NameToLayer("Enemy");
            layermask = ~layermask;

            // Dont ignore terrain
            layermask = 1 << LayerMask.NameToLayer("Terrain");

            if (!Physics.Linecast(transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
            {

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
        }
    }

    void CheckForSurvivor(Collider col)
    {
        int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
        layermask = 1 << LayerMask.NameToLayer("Enemy");
        layermask = ~layermask;

        // Can they see a Survivor && is the survivor in range
        if (Physics.Linecast(this.transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
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
        if (obj != null)
        {
            if (currentCooldown <= 0)
            {
                Debug.Log("Attacked");
                currentCooldown = cooldown;
                obj.GetComponent<Health>().Damage(damage);
            }
        }
    }
}