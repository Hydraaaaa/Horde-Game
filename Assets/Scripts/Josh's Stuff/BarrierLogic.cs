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

    public int Health = 0;
    public int Cost = 0;
    public int Level = 0;

    public LevelInformation Information;

    public KeyCode interOne;
    public KeyCode interTwo;

    private GameObject player1;
    private GameObject player2;

	// Use this for initialization
	void Start ()
    {
        player1 = GameObject.FindGameObjectWithTag("Player 1");
        player2 = GameObject.FindGameObjectWithTag("Player 2");
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerStay(Collider col)
    {
        // If player 1 interacts
        if (col.tag == "Player 1")
        {
            if (Input.GetKeyDown(interOne))
            {
                UpgradeBarrier(player1.GetComponent<BarrierPlayersideLogic>());
            }
        }

        // If player 2 interacts
        if (col.tag == "Player 2")
        {
            if (Input.GetKeyDown(interTwo))
            {
                UpgradeBarrier(player2.GetComponent<BarrierPlayersideLogic>());
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
                    Health = Information.Health.Level1;
                    break;
                case 1:
                    Cost = Information.Cost.Level2;
                    Health = Information.Health.Level2;
                    break;
                case 2:
                    Cost = Information.Cost.Level3;
                    Health = Information.Health.Level3;
                    break;
                case 3:
                    Cost = Information.Cost.Level4;
                    Health = Information.Health.Level4;
                    break;
                case 4:
                    Cost = Information.Cost.Level5;
                    Health = Information.Health.Level5;
                    break;
            }
        }
    }
}
