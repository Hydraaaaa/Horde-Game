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
    public GameObject[] UIPiece;

    // Controller References
    public GameObject[] ActivePiece;
    public GameObject[] InactivePiece;
    public GameObject[] UIPieceThumbstick;

    public GameObject Target;

    public delegate void Attack();
    public Attack turretAttack;

    // UI Information
    public bool[] interacting;
    public bool[] repairPage;

    public float viewRange = 4f;

    // Turret Information
    public float maxTargetRange = 5f;
    public float targetsDistance;

    public float attackInterval = 0.75f;
    private float curAttackInterval;

    public int Level = 0;

    bool firstRun;

    bool[] useController;

	// Use this for initialization
	void Start ()
    {       
        turretAttack = VerticalRotator.GetComponent<TurretRifle>().Attack;

        useController = new bool[GameObjectManager.instance.players.Count];
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            useController[i] = GameObjectManager.instance.players[i].gameObject.GetComponent<PlayerMovScript>().useController;
        UIPiece = new GameObject[GameObjectManager.instance.players.Count];
        ActivePiece = new GameObject[GameObjectManager.instance.players.Count];
        InactivePiece = new GameObject[GameObjectManager.instance.players.Count];
        UIPieceThumbstick = new GameObject[GameObjectManager.instance.players.Count];
        interacting = new bool[GameObjectManager.instance.players.Count];
        repairPage = new bool[GameObjectManager.instance.players.Count];
    }
    
    void Awake()
    {
        firstRun = true;
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
        bool isNull = false;
        for (int i = 0; i < UIPiece.Length; i++)
            if (UIPiece[i] == null)
                isNull = true;

        if (isNull)
            GameObjectManager.instance.HUD.GetComponent<HUDScript>().GrabTurretsUI(gameObject);

        for (int i = 0; i < UIPiece.Length; i++)
        {
            //                                  InteractUI           Active
            ActivePiece[i] = UIPiece[i].transform.GetChild(0).gameObject;
            //                                  InteractUI           Inactive
            InactivePiece[i] = UIPiece[i].transform.GetChild(1).gameObject;
        }

        for (int i = 0; i < useController.Length; i++)
        {
            if (useController[i])
                UIPieceThumbstick[i] = ActivePiece[i].transform.GetChild(4).gameObject;
        }

        // Disable all Active UI
        for (int i = 0; i < UIPiece.Length; i++)
        {
            ActivePiece[i].SetActive(false);
            InactivePiece[i].SetActive(false);
        }
    }

    void TurretsUI()
    {
        // Make sure the references to the player are still useable
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

        for (int i = 0; i < interacting.Length; i++)
        {
            if (GameObjectManager.instance.players[i].camera != null)
            {
            // UIPiece[i].transform.position = Camera.main.WorldToScreenPoint(transform.position);

                if (interacting[i])
                {
                    if (useController[i])
                    {
                        // Update whether or not the turret is active
                        if (isActive)
                            ActivePiece[i].transform.GetChild(1).GetComponent<Text>().text = "Active";
                        else
                            ActivePiece[i].transform.GetChild(1).GetComponent<Text>().text = "Inactive";

                        // Update the Page the player is on
                        if (repairPage[i])
                            ActivePiece[i].transform.GetChild(2).GetComponent<Text>().text = "Repairing";
                        else
                            ActivePiece[i].transform.GetChild(2).GetComponent<Text>().text = "Upgrading";

                        // Update the UI costs
                        if (repairPage[i])
                            ActivePiece[i].transform.GetChild(3).GetComponent<Text>().text = "Cost: " + (CurrentLevelStats.TotalResourceCost / 2);
                        else
                            ActivePiece[i].transform.GetChild(3).GetComponent<Text>().text = "Cost: " + CurrentLevelStats.TotalResourceCost;
                    }
                    else
                    {
                        // Update whether or not the turret is active
                        if (isActive)
                            ActivePiece[i].transform.GetChild(0).GetComponent<Text>().text = "Active";
                        else
                            ActivePiece[i].transform.GetChild(0).GetComponent<Text>().text = "Inactive";

                        // Update prices
                        ActivePiece[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "Cost: " + (CurrentLevelStats.TotalResourceCost / 2);
                        ActivePiece[i].transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "Cost: " + CurrentLevelStats.TotalResourceCost;
                    }
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
                    // Remove the reference
                    Target = null;
                }

                // If the enemy goes out of range
                if (targetsDistance > maxTargetRange)
                {
                    // Remove the reference
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
                    if (Vector3.Distance(col.transform.position, gameObject.transform.position) < viewRange)
                        PlayerConnects(col);
                    else
                    {
                        int playerNum = col.GetComponent<PlayerMovScript>().playerNumber - 1;

                        ActivePiece[playerNum].SetActive(false);
                        InactivePiece[playerNum].SetActive(false);
                        interacting[playerNum] = false;
                        repairPage[playerNum] = true;
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
        int playerNum = LocalMovRef.playerNumber - 1;

        Debug.DrawLine(transform.position, col.transform.position, Color.red);

        int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
        layermask += 1 << LayerMask.NameToLayer("Enemy");
        layermask += 1 << LayerMask.NameToLayer("Pickup");
        layermask = ~layermask;

        // If the player is in view of the control panel
        if (Physics.Linecast(transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
        {
            if (interacting[playerNum])
            {
                ActivePiece[playerNum].SetActive(false);
                InactivePiece[playerNum].SetActive(true);
            }
            else
            {
                ActivePiece[playerNum].SetActive(true);
                InactivePiece[playerNum].SetActive(false);
            }

            // If there is a reference to the manager and there are still players alive
            if (GameObjectManager.instance != null && GameObjectManager.instance.players.Count > 0)
            {
                // If controller
                if (GameObjectManager.instance.players[playerNum].gameObject.GetComponent<PlayerMovScript>().useController)
                {
                    // If the interacting player presses the X button
                    if (Input.GetButtonDown(LocalMovRef.playerBeginning + "XButton"))
                    {
                        if (interacting[playerNum])
                        {
                            if (repairPage[playerNum])
                                RepairBarrier(GameObjectManager.instance.players[playerNum].gameObject.GetComponent<BarrierPlayersideLogic>());
                            else
                                UpgradeBarrier(GameObjectManager.instance.players[playerNum].gameObject.GetComponent<BarrierPlayersideLogic>());
                        }
                        else
                        {
                            interacting[playerNum] = true;
                        }
                    }
                    else if (Input.GetButtonDown(LocalMovRef.playerBeginning + "BButton"))
                    {
                        interacting[playerNum] = false;
                    }
                    // if the dpad is being pressed vertically
                    else if (Mathf.Abs(Input.GetAxis(LocalMovRef.playerBeginning + "DVertical")) > 0.75f)
                    {
                        // If pressed up
                        if (Input.GetAxis(LocalMovRef.playerBeginning + "DVertical") > 0.75f)
                        {
                            repairPage[playerNum] = true;
                        }
                        // Otherwise its pressed down
                        else
                        {
                            repairPage[playerNum] = false;
                        }
                    }
                }
                else
                {
                    // PC Control

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (interacting[playerNum])
                        {
                            RepairBarrier(GameObjectManager.instance.players[playerNum].gameObject.GetComponent<BarrierPlayersideLogic>());
                        }
                        else
                        {
                            interacting[playerNum] = true;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        interacting[playerNum] = false;
                    }
                    // if the dpad is being pressed vertically
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        UpgradeBarrier(GameObjectManager.instance.players[playerNum].gameObject.GetComponent<BarrierPlayersideLogic>());
                    }
                }
            }
        }
        // The player isnt in view of the control panel
        else
        {
            // Hide the UI pieces
            ActivePiece[playerNum].SetActive(false);
            InactivePiece[playerNum].SetActive(false);
            interacting[playerNum] = false;
            repairPage[playerNum] = true;

            Debug.Log("Cant See Him!");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && col.GetComponent<ReviveSystem>() != null)
        {
            int playerNum = col.GetComponent<PlayerMovScript>().playerNumber - 1;
            playersInRange.Remove(col.gameObject);
            
            ActivePiece[playerNum].SetActive(false);
            InactivePiece[playerNum].SetActive(false);

            if (playersInRange.Count <= 0)
            {
                for (int i = 0; i < interacting.Length; i++)
                {
                    interacting[i] = false;
                    repairPage[i] = true;
                }
            }
        }
    }

    public void UpgradeBarrier(BarrierPlayersideLogic playerRes)
    {
        if (Level < AllLevelStats.Length)
        {
            if (playerRes.Resources >= AllLevelStats[Level + 1].TotalResourceCost)
            {
                ScoreManager.instance.TurretUpgrade(playerRes.gameObject);
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
            ScoreManager.instance.TurretRepair(playerRes.gameObject);

            // Set Current Lifetime back to max lifetime
            CurrentLevelStats.TotalLifetime = AllLevelStats[Level].TotalLifetime;
            isActive = true;

            Debug.Log("Repaired the Barrier!");
        }
    }
}
