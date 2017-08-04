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
    public GameObject ActivePiece;
    public GameObject InactivePiece;

    public int MaxLevel = 2;
    public LevelInformation LevInformation;
    public TurretInformation[] TurInformation = new TurretInformation[3];

    private GameObjectManager manager;

    public bool Interacting = false;
    public bool RepairPage = true;
    public bool playerInRange = false;
    public float viewRange = 4f;

    // Use this for initialization
    void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameObjectManager>();
        UIPiece = Instantiate(UIPiece) as GameObject;
        UIPiece = UIPiece.transform.GetChild(0).gameObject;
        ActivePiece = UIPiece.transform.GetChild(0).gameObject;
        InactivePiece = UIPiece.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Interacting != true)
        {
            ActivePiece.SetActive(false);
            InactivePiece.SetActive(true);
        }
        else
        {
            ActivePiece.SetActive(true);
            InactivePiece.SetActive(false);
        }

        UIPiece.transform.position = Camera.main.WorldToScreenPoint(GamepadTransform.transform.position);
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag != "Player")
        {
            for (int i = 0; i < TurInformation.Length; i++)
            {
                if (TurInformation[i].active)
                {
                    col.GetComponent<Health>().health -= TurInformation[i].DPS;
                }

                if (TurInformation[i].curActiveTime > 0)
                {
                    TurInformation[i].curActiveTime -= Time.deltaTime;
                }
                else
                {
                    TurInformation[i].active = false;
                }
            }
        }

        if (col.tag == "Player")
        {
            if (manager != null && manager.players.Count > 0)
            {
                // If player 1 interacts
                if (col.gameObject == manager.players[0])
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 0);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 1);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 2);
                    }

                    if (Input.GetButton("Joy1YButton"))
                    {
                        if (Interacting == false)
                        {
                            Interacting = true;
                        }
                        else
                        {
                            UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 0);
                        }
                    }
                    if (Input.GetButton("Joy1XButton"))
                    {
                        if (Interacting == false)
                        {
                            Interacting = true;
                        }
                        else
                        {
                            UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 1);
                        }
                    }
                    if (Input.GetButton("Joy1AButton"))
                    {
                        if (Interacting == false)
                        {
                            Interacting = true;
                        }
                        else
                        {
                            UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 2);
                        }
                    }
                    if (Input.GetButton("Joy1BButton"))
                    {
                        Interacting = false;
                    }
                }

                // If player 2 interacts
                else if (col.gameObject == manager.players[1])
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 0);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 1);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>(), 2);
                    }

                    if (Input.GetButton("Joy2YButton"))
                    {
                        if (Interacting == false)
                        {
                            Interacting = true;
                        }
                        else
                        {
                            UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 0);
                        }
                    }
                    if (Input.GetButton("Joy2XButton"))
                    {
                        if (Interacting == false)
                        {
                            Interacting = true;
                        }
                        else
                        {
                            UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 1);
                        }
                    }
                    if (Input.GetButton("Joy2AButton"))
                    {
                        if (Interacting == false)
                        {
                            Interacting = true;
                        }
                        else
                        {
                            UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>(), 2);
                        }
                    }
                    if (Input.GetButton("Joy2BButton"))
                    {
                        Interacting = false;
                    }
                }
            }
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
}
