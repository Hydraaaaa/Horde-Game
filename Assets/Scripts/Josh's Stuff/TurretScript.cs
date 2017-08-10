using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [System.Serializable]
    public struct ResourceCost
    {
        public int Level1;
        public int Level2;
        public int Level3;
        public int Level4;
        public int Level5;
    };

    [System.Serializable]
    public struct DamagePerLevel
    {
        public int Level1;
        public int Level2;
        public int Level3;
        public int Level4;
        public int Level5;
    }

    [System.Serializable]
    public struct LifetimePerLevel
    {
        public int Level1;
        public int Level2;
        public int Level3;
        public int Level4;
        public int Level5;
    }

    [System.Serializable]
    public struct LevelInformation
    {
        public DamagePerLevel Damage;
        public ResourceCost Cost;
        public LifetimePerLevel lifetime;
    }

    [System.Serializable]
    public struct TurretInformation
    {
        public int Cost;
        public int Level;
        public int DPS;
        public float activeTime;
        public float curActiveTime;
        public bool active;
    }

    public GameObject GamepadTransform;
    public GameObject UIPiece;
    public TurretRef UIPieceInfo;

    public GameObject UIPieceThumbstick;
    private GameObject ActivePiece;
    private GameObject InactivePiece;

    public int MaxLevel = 2;
    public LevelInformation LevInformation;
    public TurretInformation[] TurInformation = new TurretInformation[3];

    public GameObject[] Turrets;

    private GameObjectManager manager;

    public bool Interacting = false;
    public bool RepairPage = true;
    public float viewRange = 4f;

    // Use this for initialization
    void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameObjectManager>();
        UIPiece = Instantiate(UIPiece) as GameObject;
        UIPieceInfo = UIPiece.GetComponent<TurretRef>();
        UIPiece = UIPiece.transform.GetChild(0).gameObject;
        ActivePiece = UIPiece.transform.GetChild(0).gameObject;
        InactivePiece = UIPiece.transform.GetChild(1).gameObject;
        UIPieceThumbstick = ActivePiece.transform.GetChild(0).transform.GetChild(1).gameObject;

        ActivePiece.SetActive(false);
        InactivePiece.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Camera.main != null)
        {
            if (InactivePiece.active)
            {
                UIPiece.transform.position = Camera.main.WorldToScreenPoint(GamepadTransform.transform.position);
            }
            UIPieceInfo.Turret1.transform.position = Camera.main.WorldToScreenPoint(Turrets[0].transform.position);
            UIPieceInfo.Turret2.transform.position = Camera.main.WorldToScreenPoint(Turrets[1].transform.position);
            UIPieceInfo.Turret3.transform.position = Camera.main.WorldToScreenPoint(Turrets[2].transform.position);
            UIPieceInfo.Turret4.transform.position = Camera.main.WorldToScreenPoint(GamepadTransform.transform.position);


            if (RepairPage)
            {
                UIPieceThumbstick.transform.rotation = Quaternion.Euler(0, 0, 0);
                UIPieceInfo.Repairing = true;
            }
            else
            {
                UIPieceThumbstick.transform.rotation = Quaternion.Euler(180, 0, 0);
                UIPieceInfo.Repairing = false;
            }
        }


        for (int i = 0; i < TurInformation.Length; i++)
        {
            if (TurInformation[i].curActiveTime > 0)
            {
                TurInformation[i].curActiveTime -= Time.deltaTime;
                TurInformation[i].active = true;

                string yes = "Active";
                string no = "Inactive";

                switch (i)
                {
                    case 0:
                        if (TurInformation[i].active)
                        {
                            UIPieceInfo.Active1.text = yes;
                        }
                        else
                        {
                            UIPieceInfo.Active1.text = no;
                        }
                        break;
                    case 1:
                        if (TurInformation[i].active)
                        {
                            UIPieceInfo.Active1.text = yes;
                        }
                        else
                        {
                            UIPieceInfo.Active1.text = no;
                        }
                        break;
                    case 2:
                        if (TurInformation[i].active)
                        {
                            UIPieceInfo.Active1.text = yes;
                        }
                        else
                        {
                            UIPieceInfo.Active1.text = no;
                        }
                        break;
                    case 3:
                        if (TurInformation[i].active)
                        {
                            UIPieceInfo.Active1.text = yes;
                        }
                        else
                        {
                            UIPieceInfo.Active1.text = no;
                        }
                        break;
                }
            }
            else
            {
                TurInformation[i].active = false;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {

        // If a player interacts with the trigger
        if (col.tag == "Player")
        {
            Debug.DrawLine(transform.position, col.transform.position, Color.red);

            int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
            layermask = 1 << LayerMask.NameToLayer("Enemy");
            layermask = ~layermask;

            // If the player is in view of the control panel
            if (Physics.Linecast(transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
            {
                // If the player isnt ineracting yet, show the first panel
                if (Interacting != true)
                {
                    ActivePiece.SetActive(false);
                    InactivePiece.SetActive(true);
                }
                // Otherwise show the second panel
                else
                {
                    ActivePiece.SetActive(true);
                    InactivePiece.SetActive(false);
                }

                // If there is a referance to the manager and there is still players alive
                if (manager != null && manager.players.Count > 0)
                {
                    // If player 1 interacts
                    if (col.gameObject == manager.players[0])
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            if (RepairPage)
                                RepairBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 0);
                            else
                                UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 0);
                        }
                        if (Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            if (RepairPage)
                                RepairBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 1);
                            else
                                UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 1);
                        }
                        if (Input.GetKeyDown(KeyCode.Alpha3))
                        {
                            if (RepairPage)
                                RepairBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 2);
                            else
                                UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 2);
                        }

                        if (Input.GetButtonDown("Joy1YButton"))
                        {
                            if (Interacting == false)
                            {
                                Interacting = true;
                            }
                            else
                            {
                                if (RepairPage)
                                    RepairBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 0);
                                else
                                    UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 0);
                            }
                        }
                        if (Input.GetButtonDown("Joy1XButton"))
                        {
                            if (Interacting == false)
                            {
                                Interacting = true;
                            }
                            else
                            {
                                if (RepairPage)
                                    RepairBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 1);
                                else
                                    UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 1);
                            }
                        }
                        if (Input.GetButtonDown("Joy1AButton"))
                        {
                            if (Interacting == false)
                            {
                                Interacting = true;
                            }
                            else
                            {
                                if (RepairPage)
                                    RepairBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 2);
                                else
                                    UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 2);
                            }
                        }
                        if (Input.GetButtonDown("Joy1BButton"))
                        {
                            Interacting = false;
                        }

                        // if the dpad is being pressed vertically
                        if (Mathf.Abs(Input.GetAxis("Joy1DVertical")) > 0.75f)
                        {
                            // If pressed up
                            if (Input.GetAxis("Joy1DVertical") > 0.75f)
                            {
                                RepairPage = true;
                            }
                            // If pressed down
                            else
                            {
                                RepairPage = false;
                            }
                        }
                    }

                    // If player 2 interacts
                    else if (col.gameObject == manager.players[1])
                    {
                        if (Input.GetButtonDown("Joy2YButton"))
                        {
                            if (Interacting == false)
                            {
                                Interacting = true;
                            }
                            else
                            {
                                if (RepairPage)
                                    RepairBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 0);
                                else
                                    UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 0);
                            }
                        }
                        if (Input.GetButtonDown("Joy2XButton"))
                        {
                            if (Interacting == false)
                            {
                                Interacting = true;
                            }
                            else
                            {
                                if (RepairPage)
                                    RepairBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 0);
                                else
                                    UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 0);
                            }
                        }
                        if (Input.GetButtonDown("Joy2AButton"))
                        {
                            if (Interacting == false)
                            {
                                Interacting = true;
                            }
                            else
                            {
                                if (RepairPage)
                                    RepairBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 0);
                                else
                                    UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 0);
                            }
                        }
                        if (Input.GetButtonDown("Joy2BButton"))
                        {
                            Interacting = false;
                        }
                    }
                }
            }
            // The player isnt in view of the control panel
            else
            {
                Debug.Log("Cant See Him!");
                // Hide the UI pieces
                ActivePiece.SetActive(false);
                InactivePiece.SetActive(false);
                Interacting = false;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            ActivePiece.SetActive(false);
            InactivePiece.SetActive(false);
        }
    }

    public void UpgradeBarrier(BarrierPlayersideLogic playerRes, int turretNumber)
    {
        if (TurInformation[turretNumber].Level < MaxLevel)
        {
            if (playerRes.Resources >= TurInformation[turretNumber].Cost)
            {
                playerRes.Resources = playerRes.Resources - TurInformation[turretNumber].Cost;

                switch (TurInformation[turretNumber].Level)
                {
                    case 0:
                        // Set Level
                        TurInformation[turretNumber].Level++;
                        // Set Cost
                        TurInformation[turretNumber].Cost = LevInformation.Cost.Level1;
                        // Set Damage
                        TurInformation[turretNumber].DPS = LevInformation.Damage.Level1;
                        // Set Max Lifetime
                        TurInformation[turretNumber].activeTime = LevInformation.lifetime.Level1;
                        // Set Current Lifetime
                        TurInformation[turretNumber].curActiveTime = TurInformation[turretNumber].activeTime;
                        Debug.Log("Upgraded to lvl 1");

                        // UI Upgrades
                        UIPieceInfo.Cost1.text = ("Cost: " + LevInformation.Cost.Level1.ToString());
                        break;
                    case 1:
                        // Set Level
                        TurInformation[turretNumber].Level++;
                        // Set Cost
                        TurInformation[turretNumber].Cost = LevInformation.Cost.Level2;
                        // Set Damage
                        TurInformation[turretNumber].DPS = LevInformation.Damage.Level2;
                        // Set Max Lifetime
                        TurInformation[turretNumber].activeTime = LevInformation.lifetime.Level2;
                        // Set Current Lifetime
                        TurInformation[turretNumber].curActiveTime = TurInformation[turretNumber].activeTime;
                        Debug.Log("Upgraded to lvl 2");
                        break;
                    case 2:
                        // Set Level
                        TurInformation[turretNumber].Level++;
                        // Set Cost
                        TurInformation[turretNumber].Cost = LevInformation.Cost.Level3;
                        // Set Damage
                        TurInformation[turretNumber].DPS = LevInformation.Damage.Level3;
                        // Set Max Lifetime
                        TurInformation[turretNumber].activeTime = LevInformation.lifetime.Level3;
                        // Set Current Lifetime
                        TurInformation[turretNumber].curActiveTime = TurInformation[turretNumber].activeTime;
                        Debug.Log("Upgraded to lvl 3");
                        break;
                    case 3:
                        // Set Level
                        TurInformation[turretNumber].Level++;
                        // Set Cost
                        TurInformation[turretNumber].Cost = LevInformation.Cost.Level4;
                        // Set Damage
                        TurInformation[turretNumber].DPS = LevInformation.Damage.Level4;
                        // Set Max Lifetime
                        TurInformation[turretNumber].activeTime = LevInformation.lifetime.Level4;
                        // Set Current Lifetime
                        TurInformation[turretNumber].curActiveTime = TurInformation[turretNumber].activeTime;
                        Debug.Log("Upgraded to lvl 4");
                        break;
                    case 4:
                        // Set Level
                        TurInformation[turretNumber].Level++;
                        // Set Cost
                        TurInformation[turretNumber].Cost = LevInformation.Cost.Level5;
                        // Set Damage
                        TurInformation[turretNumber].DPS = LevInformation.Damage.Level5;
                        // Set Max Lifetime
                        TurInformation[turretNumber].activeTime = LevInformation.lifetime.Level5;
                        // Set Current Lifetime
                        TurInformation[turretNumber].curActiveTime = TurInformation[turretNumber].activeTime;
                        Debug.Log("Upgraded to lvl 5");
                        break;
                }
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

    public void RepairBarrier(BarrierPlayersideLogic playerRes, int turretNumber)
    {
        if (playerRes.Resources >= (TurInformation[turretNumber].Cost / 2))
        {
            playerRes.Resources = playerRes.Resources - (TurInformation[turretNumber].Cost / 2);

            // Set Current Lifetime back to max lifetime
            TurInformation[turretNumber].curActiveTime = TurInformation[turretNumber].activeTime;
            Debug.Log("Upgraded to lvl 1");
        }
    }
}
