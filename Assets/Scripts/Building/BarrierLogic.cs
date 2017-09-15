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

    public GameObject controllerUI;
    public GameObject keyboardUI;
    public GameObject[] UI;

    public Text[] costRepair;
    public Text[] costUpgrade;

    public float interactTime = 0.01f;
    public float currentTime = 0;

    // Use this for initialization
    void Start ()
    {
        currentIntervalTime = IntervalLengthInSeconds;

        Cost = Information.Cost.Level1;

        UI = new GameObject[GameObjectManager.instance.players.Count];
        for (int i = 0; i < UI.Length; i++)
        {
            UI[i] = Instantiate(controllerUI, transform.position, controllerUI.transform.rotation) as GameObject;
            UI[i].transform.SetParent(transform);
        }
        
        costRepair = new Text[GameObjectManager.instance.players.Count];
        costUpgrade = new Text[GameObjectManager.instance.players.Count];

        for (int i = 0; i < UI.Length; i++)
        {
            // Get X        UI                          X                       Text 2
            costRepair[i] = UI[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            // Get Y        UI                          Y                       Text 2
            costUpgrade[i] = UI[i].transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        }


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

        for (int i = 0; i < UI.Length; i++)
            UI[i].SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (currentTime < interactTime)
            currentTime += Time.deltaTime;


        //float dist1 = 0;
        for (int i = 0; i < UI.Length; i++)
            UI[i].GetComponent<Canvas>().worldCamera = GameObjectManager.instance.players[i].camera.GetComponent<Camera>();

        if (UI != null && Camera.main != null)
        {
            //UI.transform.position = manager.camera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
            //P1UI.transform.GetChild(0).transform.localPosition = manager.camera1.GetComponent<Camera>().WorldToScreenPoint(transform.position);
            //P2UI.transform.GetChild(0).transform.localPosition = manager.camera2.GetComponent<Camera>().WorldToScreenPoint(transform.position);

            //P1UI.transform.LookAt(manager.camera1.transform.position);
            //P1UI.transform.Rotate(0, 180, 0);
            //P2UI.transform.LookAt(manager.camera2.transform.position);
            //P2UI.transform.Rotate(0, 180, 0);

            for (int i = 0; i < UI.Length; i++)
            {
                Debug.DrawLine(transform.position, UI[i].transform.GetChild(0).transform.localPosition);

                costRepair[i].text = ("Cost: " + (Cost / 2).ToString());
                costUpgrade[i].text = ("Cost: " + Cost.ToString());

                UI[i].transform.GetChild(0).position = GameObjectManager.instance.players[i].camera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
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
            UI[col.GetComponent<PlayerMovScript>().playerNumber - 1].SetActive(false);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            int playerNum = col.GetComponent<PlayerMovScript>().playerNumber - 1;

            UI[playerNum].SetActive(true);

            if (currentTime >= interactTime)
            {
                if (Input.GetButtonUp("Joy" + (playerNum + 1) + "XButton"))
                {
                    RepairBarrier(GameObjectManager.instance.players[playerNum].gameObject.GetComponent<BarrierPlayersideLogic>());
                    ScoreManager.instance.BarrierRepair(GameObjectManager.instance.players[playerNum].gameObject);
                }
                if (Input.GetButtonUp("Joy" + (playerNum + 1) + "YButton"))
                {
                    UpgradeBarrier(GameObjectManager.instance.players[playerNum].gameObject.GetComponent<BarrierPlayersideLogic>());
                    ScoreManager.instance.BarrierUpgrade(GameObjectManager.instance.players[playerNum].gameObject);
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
