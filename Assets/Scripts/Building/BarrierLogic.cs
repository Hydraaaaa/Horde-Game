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

    public bool vital;

    public float IntervalLengthInSeconds = 60;
    public float currentIntervalTime;
    public int DamageIncreasePerInterval = 5;
    public int CurrentDamagePerTick = 0;

    public GameObject UI;
    public GameObject P1UI;
    public GameObject P2UI;

    public Text CostRepair;
    public Text CostUpgrade;

    public Text CostRepair1;
    public Text CostUpgrade1;
    public Text CostRepair2;
    public Text CostUpgrade2;

    public float interactTime = 0.01f;
    public float currentTime = 0;

    // Use this for initialization
    void Start ()
    {
        GameObjectManager.instance = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameObjectManager>();
        currentIntervalTime = IntervalLengthInSeconds;

        UI = Instantiate(UI);
        UI = UI.transform.GetChild(0).gameObject;
        UI = UI.transform.GetChild(0).gameObject;
        CostRepair = UI.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        CostUpgrade = UI.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        Cost = Information.Cost.Level1;

        // Get X        UI                          X                       Text 2
        CostRepair1 = P1UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        // Get Y        UI                          Y                       Text 2
        CostUpgrade1 = P1UI.transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();

        // Get X        UI                          X                       Text 2
        CostRepair2 = P2UI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        // Get Y        UI                          Y                       Text 2
        CostUpgrade2 = P2UI.transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();


        //CostRepair2 = P2UI.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        //CostUpgrade2 = P2UI.transform.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();

        //// Make player 1's ui piece
        //P1UI = Instantiate(UIOrigin);

        //P1UI.layer = LayerMask.NameToLayer("P1UI");
        //P1UI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        //P1UI.GetComponent<Canvas>().planeDistance = 1;
        //P1UI.GetComponent<Canvas>().worldCamera = manager.camera1.GetComponent<Camera>();


        //// Make player 2's ui piece
        //P2UI = Instantiate(UIOrigin);
        //P2UI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        //P2UI.GetComponent<Canvas>().planeDistance = 1;
        //P2UI.layer = LayerMask.NameToLayer("P2UI");
        //P2UI.GetComponent<Canvas>().worldCamera = manager.camera2.GetComponent<Camera>();

        UI.SetActive(false);
        P1UI.SetActive(false);
        P2UI.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (currentTime < interactTime)
            currentTime += Time.deltaTime;


        //float dist1 = 0;
        P1UI.GetComponent<Canvas>().worldCamera = GameObjectManager.instance.players[0].camera.GetComponent<Camera>();
        P2UI.GetComponent<Canvas>().worldCamera = GameObjectManager.instance.players[1].camera.GetComponent<Camera>();

        if (UI != null && Camera.main != null)
        {
            //UI.transform.position = manager.camera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
            //P1UI.transform.GetChild(0).transform.localPosition = manager.camera1.GetComponent<Camera>().WorldToScreenPoint(transform.position);
            //P2UI.transform.GetChild(0).transform.localPosition = manager.camera2.GetComponent<Camera>().WorldToScreenPoint(transform.position);

            //P1UI.transform.LookAt(manager.camera1.transform.position);
            //P1UI.transform.Rotate(0, 180, 0);
            //P2UI.transform.LookAt(manager.camera2.transform.position);
            //P2UI.transform.Rotate(0, 180, 0);

            Debug.DrawLine(transform.position, P1UI.transform.GetChild(0).transform.localPosition);
            Debug.DrawLine(transform.position, P2UI.transform.GetChild(0).transform.localPosition);

            CostRepair.text = ("Cost: " + (Cost / 2).ToString());
            CostUpgrade.text = ("Cost: " + Cost.ToString());

            CostRepair1.text = ("Cost: " + (Cost / 2).ToString());
            CostUpgrade1.text = ("Cost: " + Cost.ToString());
            CostRepair2.text = ("Cost: " + (Cost / 2).ToString());
            CostUpgrade2.text = ("Cost: " + Cost.ToString());
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
                    if (GameObjectManager.instance.enemySpawners.Count > 0)
                    {
                        GameObjectManager.instance.enemySpawners[0].SetActive(true);
                        GameObjectManager.instance.enemySpawners[0].GetComponent<EnemySpawner>().enabled = true;
                    }
                }
                else
                {
                    if (GameObjectManager.instance.enemySpawners.Count > 0)
                    {
                        GameObjectManager.instance.enemySpawners[0].SetActive(false);
                        GameObjectManager.instance.enemySpawners[0].GetComponent<EnemySpawner>().enabled = false;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            // If player 1 interacts
            if (col.gameObject == GameObjectManager.instance.players[0].gameObject)
            {
                P1UI.SetActive(false);
            }
            // If player 2 interacts
            if (col.gameObject == GameObjectManager.instance.players[1].gameObject)
            {
                P2UI.SetActive(false);
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            // If there is atleased one player
            if (GameObjectManager.instance != null && GameObjectManager.instance.players.Count > 0)
            {                
                // If player 1 interacts
                if (col.gameObject == GameObjectManager.instance.players[0].gameObject)
                {
                    P1UI.SetActive(true);

                    if (currentTime >= interactTime)
                    {
                        if (Input.GetButtonUp("Joy1XButton"))
                        {
                            RepairBarrier(GameObjectManager.instance.players[0].gameObject.GetComponent<BarrierPlayersideLogic>());
                        }
                        if (Input.GetButtonUp("Joy1YButton"))
                        {
                            UpgradeBarrier(GameObjectManager.instance.players[0].gameObject.GetComponent<BarrierPlayersideLogic>());
                        }
                    }
                }

            }
            
            // If there is more than one player
            if (GameObjectManager.instance != null && GameObjectManager.instance.players.Count > 1)
            {
                // If player 2 interacts
                if (col.gameObject == GameObjectManager.instance.players[1].gameObject)
                {
                    P2UI.SetActive(true);

                    if (currentTime >= interactTime)
                    {
                        if (Input.GetButtonDown("Joy2XButton"))
                        {
                            RepairBarrier(GameObjectManager.instance.players[1].gameObject.GetComponent<BarrierPlayersideLogic>());
                        }
                        if (Input.GetButtonDown("Joy2YButton"))
                        {
                            UpgradeBarrier(GameObjectManager.instance.players[1].gameObject.GetComponent<BarrierPlayersideLogic>());
                        }
                    }
                }
            }
        }
    }

    public void UpgradeBarrier(BarrierPlayersideLogic playerRes)
    {
        Debug.Log("Upgrading! " + Cost);

        if (playerRes.Resources >= Cost)
        {
            playerRes.Resources -= Cost;
            currentTime = 0;

            Level++;
            switch (Level)
            {
                case 0:
                    Cost = Information.Cost.Level1;
                    GetComponent<Health>().health = Information.Health.Level1;
                    GetComponent<Health>().maxHealth = Information.Health.Level1;
                    return;
                case 1:
                    Cost = Information.Cost.Level2;
                    GetComponent<Health>().health = Information.Health.Level2;
                    GetComponent<Health>().maxHealth = Information.Health.Level2;
                    return;
                case 2:
                    Cost = Information.Cost.Level3;
                    GetComponent<Health>().health = Information.Health.Level3;
                    GetComponent<Health>().maxHealth = Information.Health.Level3;
                    return;
                case 3:
                    Cost = Information.Cost.Level4;
                    GetComponent<Health>().health = Information.Health.Level4;
                    GetComponent<Health>().maxHealth = Information.Health.Level4;
                    return;
                case 4:
                    Cost = Information.Cost.Level5;
                    GetComponent<Health>().health = Information.Health.Level5;
                    GetComponent<Health>().maxHealth = Information.Health.Level5;
                    return;
            }
        }
    }

    public void RepairBarrier(BarrierPlayersideLogic playerRes)
    {
        Debug.Log("Repairing! " + Cost / 2);

        if (playerRes.Resources >= (Cost / 2))
        {
            playerRes.Resources -= (Cost / 2);
            currentTime = 0;

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
