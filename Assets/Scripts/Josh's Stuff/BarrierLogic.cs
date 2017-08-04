using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public KeyCode interOne;
    public KeyCode interTwo;

    private GameObjectManager manager;

    public bool vital;

	// Use this for initialization
	void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameObjectManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (manager != null && manager.players.Count > 0)
            {
                // If player 1 interacts
                if (col.gameObject == manager.players[0])
                {
                    if (Input.GetKeyDown(interOne))
                    {
                        UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>());
                    }
                    else if (Input.GetButton("Joy1XButton"))
                    {
                        UpgradeBarrier(manager.players[0].GetComponent<BarrierPlayersideLogic>());
                    }
                }

                // If player 2 interacts
                if (col.gameObject == manager.players[1])
                {
                    if (Input.GetKeyDown(interTwo))
                    {
                        UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>());
                    }
                    else if (Input.GetButton("Joy2XButton"))
                    {
                        UpgradeBarrier(manager.players[1].GetComponent<BarrierPlayersideLogic>());
                    }
                }
            }
        }
    }

    public void UpgradeBarrier(BarrierPlayersideLogic playerRes)
    {
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
}
