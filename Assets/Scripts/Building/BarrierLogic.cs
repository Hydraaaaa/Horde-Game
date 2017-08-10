using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrierLogic : MonoBehaviour
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
    public struct HealthPerLevel
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
        public HealthPerLevel Health;
        public ResourceCost Cost;
    }

    public int Cost = 0;
    public int Level = 0;

    public LevelInformation Information;
    private GameObjectManager manager;

    public bool vital;

    public float IntervalLengthInSeconds = 60;
    public float currentIntervalTime;
    public int DamageIncreasePerInterval = 5;
    public int CurrentDamagePerTick = 0;

    public GameObject UI;
    public Text CostRepair;
    public Text CostUpgrade;

	// Use this for initialization
	void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameObjectManager>();
        currentIntervalTime = IntervalLengthInSeconds;
        UI = Instantiate(UI);
        UI = UI.transform.GetChild(0).gameObject;
        CostRepair = UI.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        CostUpgrade = UI.transform.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();

        Cost = Information.Cost.Level1;
    }

    // Update is called once per frame
    void Update ()
    {
        float dist1 = 0;

        if (UI != null && Camera.main != null)
        {
            UI.SetActive(false);
            UI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            CostRepair.text = ("Cost: " + (Cost / 2).ToString());
            CostUpgrade.text = ("Cost: " + Cost.ToString());
        }

        if (manager.players.Count > 0)
        {
            dist1 = Vector3.Distance(transform.position, manager.players[0].transform.position);

            if (dist1 < 2)
            {
                UI.SetActive(true);
            }
        }
        if (manager.players.Count > 1)
        {
            dist1 = Vector3.Distance(transform.position, manager.players[0].transform.position);

            if (dist1 < 2)
            {
                UI.SetActive(true);
            }
        }

        if (!vital)
        {
            if (currentIntervalTime > 0)
                currentIntervalTime -= Time.deltaTime;
            else
            {
                currentIntervalTime = IntervalLengthInSeconds;
                CurrentDamagePerTick += DamageIncreasePerInterval;
            }

            if (GetComponent<Health>() != null)
            {
                GetComponent<Health>().Damage(CurrentDamagePerTick);

                if (GetComponent<Health>().health <= 0)
                {
                    if (manager.enemySpawners.Count > 0)
                        manager.enemySpawners[0].SetActive(true);
                }
                else
                {
                    if (manager.enemySpawners.Count > 0)
                        manager.enemySpawners[0].SetActive(false);
                }
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            // If there is atleased one player
            if (manager != null && manager.players.Count > 0)
            {                
                // If player 1 interacts
                if (col.gameObject == manager.players[0])
                { 
                    if (Input.GetButton("Joy1XButton"))
                    {
                        RepairBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>());
                    }
                    if (Input.GetButton("Joy1YButton"))
                    {
                        UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>());
                    }
                }

            }
            
            // If there is more than one player
            if (manager != null && manager.players.Count > 1)
            {
                // If player 2 interacts
                if (col.gameObject == manager.players[1])
                {
                    if (Input.GetButton("Joy2XButton"))
                    {
                        RepairBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>());
                    }
                    if (Input.GetButton("Joy2YButton"))
                    {
                        UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>());
                    }
                }
            }
        }
    }

    public void UpgradeBarrier(BarrierPlayersideLogic playerRes)
    {
        Debug.Log("Upgrading!");

        if (playerRes.Resources >= Cost)
        {
            playerRes.Resources -= Cost;

            Level++;
            switch (Level)
            {
                case 0:
                    Cost = Information.Cost.Level1;
                    GetComponent<Health>().health = Information.Health.Level1;
                    GetComponent<Health>().maxHealth = Information.Health.Level1;
                    break;
                case 1:
                    Cost = Information.Cost.Level2;
                    GetComponent<Health>().health = Information.Health.Level2;
                    GetComponent<Health>().maxHealth = Information.Health.Level2;
                    break;
                case 2:
                    Cost = Information.Cost.Level3;
                    GetComponent<Health>().health = Information.Health.Level3;
                    GetComponent<Health>().maxHealth = Information.Health.Level3;
                    break;
                case 3:
                    Cost = Information.Cost.Level4;
                    GetComponent<Health>().health = Information.Health.Level4;
                    GetComponent<Health>().maxHealth = Information.Health.Level4;
                    break;
                case 4:
                    Cost = Information.Cost.Level5;
                    GetComponent<Health>().health = Information.Health.Level5;
                    GetComponent<Health>().maxHealth = Information.Health.Level5;
                    break;
            }
        }
    }

    public void RepairBarrier(BarrierPlayersideLogic playerRes)
    {
        Debug.Log("Repairing!");

        if (playerRes.Resources >= (Cost / 2))
        {
            playerRes.Resources -= (Cost / 2);

            switch (Level)
            {
                case 0:
                    Cost = Information.Cost.Level1;
                    GetComponent<Health>().health = Information.Health.Level1;
                    GetComponent<Health>().maxHealth = Information.Health.Level1;
                    break;
                case 1:
                    Cost = Information.Cost.Level2;
                    GetComponent<Health>().health = Information.Health.Level2;
                    GetComponent<Health>().maxHealth = Information.Health.Level2;
                    break;
                case 2:
                    Cost = Information.Cost.Level3;
                    GetComponent<Health>().health = Information.Health.Level3;
                    GetComponent<Health>().maxHealth = Information.Health.Level3;
                    break;
                case 3:
                    Cost = Information.Cost.Level4;
                    GetComponent<Health>().health = Information.Health.Level4;
                    GetComponent<Health>().maxHealth = Information.Health.Level4;
                    break;
                case 4:
                    Cost = Information.Cost.Level5;
                    GetComponent<Health>().health = Information.Health.Level5;
                    GetComponent<Health>().maxHealth = Information.Health.Level5;
                    break;
            }
        }
    }
}
