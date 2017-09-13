using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretAI : MonoBehaviour
{
    [System.Serializable]
    // Stats for each level of the turret
    public struct LevelInformation
    {
        public int TotalResourceCost;
        public int TotalDamagePerShot;
        public int TotalLifetime;
    }

    // Structs for current level and all future levels
    public LevelInformation[] AllLevelStats;
    public LevelInformation CurrentLevelStats;
    
    // Active times for turret
    public float curActiveTime;
    public bool isActive;

    // Public list of all players in range of the turret
    public List<GameObject> playersInRange;

    // References
    public GameObject HorizontalRotator;
    public GameObject VerticalRotator;
    public GameObject[] TurretBarrels;

    // UI References
    public GameObject P1UIPiece;
    public GameObject P2UIPiece;

    // Controller References
    public GameObject ActivePiece1;
    public GameObject InactivePiece1;
    public GameObject UIPieceThumbstick1;

    public GameObject ActivePiece2;
    public GameObject InactivePiece2;
    public GameObject UIPieceThumbstick2;

    public GameObject Target;

    public delegate void Attack();
    public Attack turretAttack;

    // UI Information
    public bool P1Interacting = false;
    public bool P2Interacting = false;
    public bool P1RepairPage = true;
    public bool P2RepairPage = true;
    public float viewRange = 4f;

    // Turret Information
    public float maxTargetRange = 5f;
    public float targetsDistance;

    public float attackInterval = 0.75f;
    private float curAttackInterval;

    public int Level = 0;

    bool firstRun = true;

    bool[] useController;

	// Use this for initialization
	void Start ()
    {       
        turretAttack = VerticalRotator.GetComponent<TurretRifle>().Attack;
        useController = new bool[]
        {
            GameObjectManager.instance.players[0].gameObject.GetComponent<PlayerMovScript>().useController,
            GameObjectManager.instance.players[1].gameObject.GetComponent<PlayerMovScript>().useController
        };
    }
    
    void Awake()
    {        
        // Turn off all Gun barrels on startup
        TurretBarrels[0].SetActive(false);
        TurretBarrels[1].SetActive(false);
        TurretBarrels[2].SetActive(false);
        TurretBarrels[3].SetActive(false);
        TurretBarrels[4].SetActive(false);
        TurretBarrels[5].SetActive(false);
        TurretBarrels[6].SetActive(false);
        TurretBarrels[7].SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (firstRun)
        {
            FirstRun();
            firstRun = false;

            CurrentLevelStats.TotalResourceCost = AllLevelStats[0].TotalResourceCost;
        }
        
        // Show the necessary barrels for the current level of the gun
        ShowBarrels();

        // Run the Turret's AI
        TurretsAI();

        // Update / Run the Turret's UI
        TurretsUI();
    }

    void FirstRun()
    {
        if (P1UIPiece == null || P2UIPiece == null)
        {
            GameObjectManager.instance.HUD.GetComponent<HUDScript>().GrabTurretsUI(gameObject);
        }

        //                                  InteractUI           Active
        ActivePiece1 = P1UIPiece.transform.GetChild(0).gameObject;
        //                                  InteractUI           Inactive
        InactivePiece1 = P1UIPiece.transform.GetChild(1).gameObject;

        //                                  InteractUI           Active
        ActivePiece2 = P2UIPiece.transform.GetChild(0).gameObject;
        //                                 InteractUI           Inactive
        InactivePiece2 = P2UIPiece.transform.GetChild(1).gameObject;

        if (useController[0])
            UIPieceThumbstick1 = ActivePiece1.transform.GetChild(4).gameObject;
        if (useController[1])
            UIPieceThumbstick2 = ActivePiece2.transform.GetChild(4).gameObject;

        // Disable all Active UI
        ActivePiece1.SetActive(false);
        InactivePiece1.SetActive(false);
        ActivePiece2.SetActive(false);
        InactivePiece2.SetActive(false);
    }

    void TurretsUI()
    {
        // Make sure the referances to the player are still useable
        for (int i = 0; i < playersInRange.Count; i++)
        {
            if (playersInRange[i] == null || playersInRange[i].GetComponent<ReviveSystem>().NeedRes)
            {
                playersInRange.RemoveAt(i);
            }
        }
        //foreach (GameObject player in playersInRange)
        //{
        //    if (player == null || player.GetComponent<ReviveSystem>().NeedRes)
        //        playersInRange.Remove(player);
        //}

        if (GameObjectManager.instance.players[0].camera != null)
        {
            // P1UIPiece.transform.position = Camera.main.WorldToScreenPoint(transform.position);

            if (P1Interacting)
            {
                if (useController[0])
                {
                    // Update whether or not the turret is active
                    if (isActive)
                        ActivePiece1.transform.GetChild(1).GetComponent<Text>().text = "Active";
                    else
                        ActivePiece1.transform.GetChild(1).GetComponent<Text>().text = "Inactive";

                    // Update the Page the player is on
                    if (P1RepairPage)
                        ActivePiece1.transform.GetChild(2).GetComponent<Text>().text = "Repairing";
                    else
                        ActivePiece1.transform.GetChild(2).GetComponent<Text>().text = "Upgrading";

                    // Update the UI costs
                    if (P1RepairPage)
                        ActivePiece1.transform.GetChild(3).GetComponent<Text>().text = "Cost: " + (CurrentLevelStats.TotalResourceCost / 2);
                    else
                        ActivePiece1.transform.GetChild(3).GetComponent<Text>().text = "Cost: " + CurrentLevelStats.TotalResourceCost;
                }
                else
                {
                    // Update whether or not the turret is active
                    if (isActive)
                        ActivePiece1.transform.GetChild(0).GetComponent<Text>().text = "Active";
                    else
                        ActivePiece1.transform.GetChild(0).GetComponent<Text>().text = "Inactive";
                    
                    // Update prices
                    ActivePiece1.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "Cost: " + (CurrentLevelStats.TotalResourceCost / 2);
                    ActivePiece1.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "Cost: " + CurrentLevelStats.TotalResourceCost;
                }
            }
        }
        if (GameObjectManager.instance.players[1].camera != null)
        {
            // P1UIPiece.transform.position = Camera.main.WorldToScreenPoint(transform.position);

            if (P2Interacting)
            {
                if (useController[1])
                {
                    // Update whether or not the turret is active
                    if (isActive)
                        ActivePiece2.transform.GetChild(1).GetComponent<Text>().text = "Active";
                    else
                        ActivePiece2.transform.GetChild(1).GetComponent<Text>().text = "Inactive";

                    // Update the Page the player is on
                    if (P2RepairPage)
                        ActivePiece2.transform.GetChild(2).GetComponent<Text>().text = "Repairing";
                    else
                        ActivePiece2.transform.GetChild(2).GetComponent<Text>().text = "Upgrading";

                    // Update the UI costs
                    if (P2RepairPage)
                        ActivePiece2.transform.GetChild(3).GetComponent<Text>().text = "Cost: " + (CurrentLevelStats.TotalResourceCost / 2);
                    else
                        ActivePiece2.transform.GetChild(3).GetComponent<Text>().text = "Cost: " + CurrentLevelStats.TotalResourceCost;
                }
                else
                {
                    // Update whether or not the turret is active
                    if (isActive)
                        ActivePiece2.transform.GetChild(0).GetComponent<Text>().text = "Active";
                    else
                        ActivePiece2.transform.GetChild(0).GetComponent<Text>().text = "Inactive";

                    // Update prices
                    ActivePiece2.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "Cost: " + (CurrentLevelStats.TotalResourceCost / 2);
                    ActivePiece2.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "Cost: " + CurrentLevelStats.TotalResourceCost;
                }
            }
        }
    }

    void TurretsAI()
    {
        // If the turret is turned on
        if (isActive)
        {
            // Set the color of the drawn Line to Green
            VerticalRotator.GetComponent<LineRenderer>().startColor = new Color(0, 1, 0, 1);

            // If the turret is targeting something
            if (Target != null)
            {
                // If the turret cant attack, update the interval
                if (curAttackInterval < attackInterval)
                    curAttackInterval += Time.deltaTime;
                else
                {
                    // Otherwise, attack the target
                    curAttackInterval = 0;
                    turretAttack();
                }

                // Turn the turret in the direction of the target
                FollowTarget();

                // Calculate the enem's distance from the turret
                targetsDistance = Vector3.Distance(Target.transform.position, transform.position);

                Debug.DrawLine(transform.position, Target.transform.position, Color.red);

                // If the target dies
                if (Target.GetComponent<Health>().health <= 0)
                {
                    // Remove the referance
                    Target = null;
                }

                // If the enemy goes out of range
                if (targetsDistance > maxTargetRange)
                {
                    // Remove the referance
                    Target = null;
                }

            }
            else
            {
                // there is no target so dont bother rotating
                VerticalRotator.transform.rotation = Quaternion.Euler(0, VerticalRotator.transform.rotation.y, VerticalRotator.transform.rotation.z);
            }
        }
        else
        {
            // Set the color of the laser to red, rotate the turret to look at the floor
            VerticalRotator.GetComponent<LineRenderer>().startColor = new Color(0.5f, 0, 0, 1);

            // Slowly lerp to look at the floor
            VerticalRotator.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(22, VerticalRotator.transform.rotation.y, VerticalRotator.transform.rotation.z), Time.deltaTime * 500);
        }
    }

    void ShowBarrels()
    {
        switch (Level)
        {
            case 1:
                TurretBarrels[0].SetActive(true);
                TurretBarrels[1].SetActive(true);
                break;
            case 2:
                TurretBarrels[2].SetActive(true);
                TurretBarrels[3].SetActive(true);
                break;
            case 3:
                TurretBarrels[4].SetActive(true);
                TurretBarrels[5].SetActive(true);
                break;
            case 4:
                TurretBarrels[6].SetActive(true);
                TurretBarrels[7].SetActive(true);
                break;
            default:
                break;
        }
    }

    private void FollowTarget()
    {
        Vector3 dir = Target.transform.position - transform.position;
        HorizontalRotator.transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 500);
        HorizontalRotator.transform.localRotation = Quaternion.Euler(0, HorizontalRotator.transform.rotation.eulerAngles.y, 0);

        VerticalRotator.transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 500);
        VerticalRotator.transform.localRotation = Quaternion.Euler(VerticalRotator.transform.rotation.eulerAngles.x, 0, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.GetComponent<ReviveSystem>() != null)
            {
                playersInRange.Add(col.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (firstRun)
        {
            FirstRun();
            firstRun = false;

            CurrentLevelStats.TotalResourceCost = AllLevelStats[0].TotalResourceCost;
        }

        // If the object that entered the collider is an enemy
        if (col.CompareTag("Enemy"))
        {
            EnemyConnects(col);
        }
        // If a player interacts with the trigger
        else if (col.CompareTag("Player"))
        {
            if (col.GetComponent<ReviveSystem>() != null)
            {
                if (col.GetComponent<ReviveSystem>().NeedRes == false)
                {
                    if (Vector3.Distance(col.transform.position, gameObject.transform.position) < 3f)
                        PlayerConnects(col);
                    else
                    {
                        if (col.GetComponent<PlayerMovScript>().playerNumber == 1)
                        {
                            ActivePiece1.SetActive(false);
                            InactivePiece1.SetActive(false);
                            P1Interacting = false;
                            P1RepairPage = true;

                        }
                        if (col.GetComponent<PlayerMovScript>().playerNumber == 2)
                        {
                            ActivePiece2.SetActive(false);
                            InactivePiece2.SetActive(false);
                            P2Interacting = false;
                            P2RepairPage = true;
                        }
                    }
                }
            }
        }
    }

    void EnemyConnects(Collider col)
    {
        int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
        layermask = 1 << LayerMask.NameToLayer("Enemy");
        layermask = ~layermask;

        float dist = Vector3.Distance(col.transform.position, transform.position);

        // If the target is within the maximum distance
        if (dist < maxTargetRange)
        {
            // If there is no target currently
            if (Target == null)
            {
                targetsDistance = 0;

                // If the turret has line of sight
                if (!Physics.Linecast(VerticalRotator.transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
                {
                    Target = col.gameObject;
                }
            }
            // If there is a target (Options 2 3, and 4) (Closest, Weakest, and Closest & Weakest)
            else
            {
                // If the turret has line of sight
                if (!Physics.Linecast(VerticalRotator.transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
                {
                    // If the new target is closer
                    if (Vector3.Distance(transform.position, col.transform.position) < Vector3.Distance(transform.position, Target.transform.position))
                    {
                        // Change the target
                        Target = col.gameObject;
                    }

                    //// If new target has less health
                    //if (col.GetComponent<Health>().health < Target.GetComponent<Health>().health)
                    //{

                    //}

                    //// If the new target is closer than the old target and has less health
                    //if (Vector3.Distance(transform.position, col.transform.position) < Vector3.Distance(transform.position, Target.transform.position) &&
                    //    col.GetComponent<Health>().health < Target.GetComponent<Health>().health)
                    //{
                    //    Target = col.gameObject;
                    //}
                }
            }
        }
    }

    void PlayerConnects(Collider col)
    {
        PlayerMovScript LocalMovRef = col.GetComponent<PlayerMovScript>();

        Debug.DrawLine(transform.position, col.transform.position, Color.red);

        int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
        layermask = 1 << LayerMask.NameToLayer("Enemy");
        layermask = ~layermask;

        // If the player is in view of the control panel
        if (Physics.Linecast(transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
        {
            // If the player 1 isnt ineracting yet, show the first panel
            if (P1Interacting != true)
            {
                if (LocalMovRef.playerNumber == 1)
                {
                    ActivePiece1.SetActive(false);
                    InactivePiece1.SetActive(true);
                }
            }
            // Otherwise show the second panel
            else
            {
                if (LocalMovRef.playerNumber == 1)
                {
                    ActivePiece1.SetActive(true);
                    InactivePiece1.SetActive(false);
                }
            }

            // If the player 2 isnt ineracting yet, show the first panel
            if (P2Interacting != true)
            {
                if (LocalMovRef.playerNumber == 2)
                {
                    ActivePiece2.SetActive(false);
                    InactivePiece2.SetActive(true);
                }
            }
            // Otherwise show the second panel
            else
            {
                if (LocalMovRef.playerNumber == 2)
                {
                    ActivePiece2.SetActive(true);
                    InactivePiece2.SetActive(false);
                }
            }

            // If there is a referance to the manager and there are still players alive
            if (GameObjectManager.instance != null && GameObjectManager.instance.players.Count > 0)
            {
                // If controller
                if (GameObjectManager.instance.players[LocalMovRef.playerNumber - 1].gameObject.GetComponent<PlayerMovScript>().useController)
                {
                    // If the interacting player presses the X button
                    if (Input.GetButtonDown(LocalMovRef.playerBeginning + "XButton"))
                    {
                        // Then if they are player 1
                        if (LocalMovRef.playerNumber == 1)
                        {
                            if (P1Interacting)
                            {
                                if (P1RepairPage)
                                    RepairBarrier(GameObjectManager.instance.players[0].gameObject.GetComponent<BarrierPlayersideLogic>());
                                else
                                    UpgradeBarrier(GameObjectManager.instance.players[0].gameObject.GetComponent<BarrierPlayersideLogic>());
                            }
                            else
                            {
                                P1Interacting = true;
                            }
                        }
                        // If they're player 2
                        if (LocalMovRef.playerNumber == 2)
                        {
                            if (P2Interacting)
                            {
                                if (P2RepairPage)
                                    RepairBarrier(GameObjectManager.instance.players[1].gameObject.GetComponent<BarrierPlayersideLogic>());
                                else
                                    UpgradeBarrier(GameObjectManager.instance.players[1].gameObject.GetComponent<BarrierPlayersideLogic>());
                            }
                            else
                            {
                                P2Interacting = true;
                            }
                        }
                    }
                    else if (Input.GetButtonDown(LocalMovRef.playerBeginning + "BButton"))
                    {
                        if (LocalMovRef.playerNumber == 1)
                        {
                            P1Interacting = false;
                        }
                        else if (LocalMovRef.playerNumber == 2)
                        {
                            P2Interacting = false;
                        }
                    }
                    // if the dpad is being pressed vertically
                    else if (Mathf.Abs(Input.GetAxis(LocalMovRef.playerBeginning + "DVertical")) > 0.75f)
                    {
                        // If pressed up
                        if (Input.GetAxis(LocalMovRef.playerBeginning + "DVertical") > 0.75f)
                        {
                            if (LocalMovRef.playerNumber == 1)
                                P1RepairPage = true;
                            else
                                P2RepairPage = true;
                        }
                        // Otherwise its pressed down
                        else
                        {
                            if (LocalMovRef.playerNumber == 1)
                                P1RepairPage = false;
                            else
                                P2RepairPage = false;
                        }
                    }
                }
                else
                {
                    // PC Control

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Then if they are player 1
                        if (LocalMovRef.playerNumber == 1)
                        {
                            if (P1Interacting)
                            {
                                RepairBarrier(GameObjectManager.instance.players[0].gameObject.GetComponent<BarrierPlayersideLogic>());
                            }
                            else
                            {
                                P1Interacting = true;
                            }
                        }
                        // If they're player 2
                        if (LocalMovRef.playerNumber == 2)
                        {
                            if (P2Interacting)
                            {
                                RepairBarrier(GameObjectManager.instance.players[1].gameObject.GetComponent<BarrierPlayersideLogic>());
                            }
                            else
                            {
                                P2Interacting = true;
                            }
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        if (LocalMovRef.playerNumber == 1)
                        {
                            P1Interacting = false;
                        }
                        else if (LocalMovRef.playerNumber == 2)
                        {
                            P2Interacting = false;
                        }
                    }
                    // if the dpad is being pressed vertically
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (LocalMovRef.playerNumber == 1)
                            UpgradeBarrier(GameObjectManager.instance.players[0].gameObject.GetComponent<BarrierPlayersideLogic>());
                        if (LocalMovRef.playerNumber == 2)
                            UpgradeBarrier(GameObjectManager.instance.players[1].gameObject.GetComponent<BarrierPlayersideLogic>());
                    }
                }
            }
        }
        // The player isnt in view of the control panel
        else
        {
            // Hide the UI pieces
            if (LocalMovRef.playerNumber == 1)
            {
                ActivePiece1.SetActive(false);
                InactivePiece1.SetActive(false);
                P1Interacting = false;
                P1RepairPage = true;

                Debug.Log("Cant See Him!");
            }
            else if (LocalMovRef.playerNumber == 2)
            {
                ActivePiece2.SetActive(false);
                InactivePiece2.SetActive(false);
                P2Interacting = false;
                P2RepairPage = true;

                Debug.Log("Cant See Him!");
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playersInRange.Remove(col.gameObject);

            if (col.GetComponent<PlayerMovScript>().playerNumber == 1)
            {
                ActivePiece1.SetActive(false);
                InactivePiece1.SetActive(false);
            }
            if (col.GetComponent<PlayerMovScript>().playerNumber == 2)
            {
                ActivePiece2.SetActive(false);
                InactivePiece2.SetActive(false);
            }

            if (playersInRange.Count <= 0)
            {
                P1Interacting = false;
                P1RepairPage = true;

                P2Interacting = false;
                P2RepairPage = true;
            }
        }
    }

    public void UpgradeBarrier(BarrierPlayersideLogic playerRes)
    {
        if (Level < AllLevelStats.Length)
        {
            if (playerRes.Resources >= AllLevelStats[Level + 1].TotalResourceCost)
            {
                playerRes.Resources -= AllLevelStats[Level + 1].TotalResourceCost;

                Level++;
                CurrentLevelStats.TotalDamagePerShot = AllLevelStats[Level].TotalDamagePerShot;
                CurrentLevelStats.TotalResourceCost = AllLevelStats[Level].TotalResourceCost;
                CurrentLevelStats.TotalLifetime = AllLevelStats[Level].TotalLifetime;
                isActive = true;
                Debug.Log("UPGRADED TO LEVEL " + Level + " Cost_: " + CurrentLevelStats.TotalResourceCost + " Damage_: " + CurrentLevelStats.TotalDamagePerShot + " Lifetime_: " + CurrentLevelStats.TotalLifetime);
            }
            else
            {
                Debug.Log("More Money Needed");
            }
        }
        else
        {
            Debug.Log("Already At Max Level");
        }
    }

    public void RepairBarrier(BarrierPlayersideLogic playerRes)
    {
        if (playerRes.Resources >= (CurrentLevelStats.TotalResourceCost / 2))
        {
            // Half the total resource cost
            playerRes.Resources -= (CurrentLevelStats.TotalResourceCost / 2);

            // Set Current Lifetime back to max lifetime
            CurrentLevelStats.TotalLifetime = AllLevelStats[Level].TotalLifetime;
            isActive = true;

            Debug.Log("Repaired the Barrier!");
        }
    }
}
