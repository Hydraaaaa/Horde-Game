using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerPrefab;
    public GameObject HUDPrefab;
    public GameObject WinGUIPrefab;
    public GameObject LoseGUIPrefab;

    [HideInInspector] public GameObject camera;
    public GameObject camera1;
    public GameObject camera2;

    [HideInInspector] public GameObject endPos;
    [HideInInspector] public GameObject civilianDestination;
    public List<GameObject> playerStarts;
    public List<GameObject> players;
    public List<GameObject> enemySpawners;
    public List<GameObject> enemies;
    public List<GameObject> civilianSpawners;
    public List<GameObject> civilians;
    public List<GameObject> barricades;
    public List<GameObject> vitalBarricades;
    public List<GameObject> turrets;
    
    [Tooltip("This is a percentage")][Range(0, 100)]
    public float civiliansRequired;
    [Tooltip("Time in seconds until certain bulkhead doors open")]
    public float time;

    [HideInInspector] public float timer;
    [HideInInspector] public int initialCivilians;
    [HideInInspector] public int civiliansEscaped;
    [HideInInspector] public int playersEscaped;

    [HideInInspector] public bool playing;
    public bool trySplitScreen = false;
    void Awake()
    {
        playing = false;
    }

    void OnEnable()
    {
        timer = time;
        civiliansEscaped = 0;

        playerStarts = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player Start"));
        SpawnCamera();
        SpawnPlayers();
        GetEnemySpawners();
        GetCivilianSpawners();
        GetCivilians();
        GetBarricades();
        SpawnHUD();
        GetTurrets();
        GetCivilianDestination();
        GetEndPos();

        StartCoroutine(GetInitialCivilians());

        playing = true;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            foreach (GameObject barricade in vitalBarricades)
            {
                barricade.GetComponent<BulkheadLogic>().Open();
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }

        int playerCount = 0;
        foreach (GameObject player in players)
        {
            if (player != null && !player.GetComponent<Health>().NeedRes)
                playerCount++;
        }

        if (playerCount == 0 && playing)
            GameOver();

        if (players.Count > 0)
        if (players[0].GetComponent<Health>().health <= 0 || players[0] != null)
        {
            GameObject.Find("HPBar" + 1).GetComponent<Image>().fillAmount = 0;
            GameObject.Find("HealthCount" + 1).GetComponent<Text>().text = "0";
        }
        if (players.Count > 1)
        if (players[1].GetComponent<Health>().health <= 0 || players[1] != null)
        {
            GameObject.Find("HPBar" + 2).GetComponent<Image>().fillAmount = 0;
            GameObject.Find("HealthCount" + 2).GetComponent<Text>().text = "0";
        }
    }

    IEnumerator GetInitialCivilians()
    {
        yield return new WaitForSeconds(1);
        initialCivilians = civilians.Count;
        foreach (GameObject spawner in civilianSpawners)
        {
            if (spawner.GetComponent<CivilianSource>() != null)
                initialCivilians += spawner.GetComponent<CivilianSource>().civilians;
        }
    }

    public void SpawnCamera()
    {

        if (trySplitScreen)
        {
            camera1 = Instantiate(camera1);
            camera2 = Instantiate(camera2);
        }
        else
        {
            camera = Instantiate(cameraPrefab);
            camera.GetComponent<CameraLogic>().gameObjectManager = this;
        }
    }

    public void SpawnPlayers()
    {
        int playerCount = GetComponent<GameManager>().playerCount;

        players = new List<GameObject>();

        // CameraLogic cameraScript = camera.GetComponent<CameraLogic>();

        if (playerStarts.Count != 0)
        {
            for (int i = 0; i < playerCount; i++)
            {
                int playerStartIndex = i % playerStarts.Count;

                GameObject playerInstance = Instantiate(playerPrefab,
                                                        playerStarts[playerStartIndex].transform.position,
                                                        playerStarts[playerStartIndex].transform.rotation);

                playerInstance.GetComponent<PlayerMovScript>().playerNumber = i + 1;
                playerInstance.GetComponent<PlayerInfoScript>().PlayerNo = i + 1;

                if (i == 0)
                    playerInstance.GetComponent<LineRenderer>().startColor = new Color(0.08f, 0.68f, 0.74f, 1);
                if (i == 1)
                    playerInstance.GetComponent<LineRenderer>().startColor = new Color(0.74f, 0.08f, 0.36f, 1);
                if (i == 2)
                    playerInstance.GetComponent<LineRenderer>().startColor = new Color(1, 0, 1, 1);
                if (i == 3)
                    playerInstance.GetComponent<LineRenderer>().startColor = new Color(0, 1, 1, 1);

                players.Add(playerInstance);
            }
        }
        else
            Debug.Log("No player starts found");

        if (trySplitScreen)
        {
            camera1.GetComponent<CameraMovement>().player = players[0];
            camera1.GetComponent<AudioListener>().enabled = true;
            camera2.GetComponent<CameraMovement>().player = players[1];
            //camera2.GetComponent<AudioListener>().enabled = true;
        }
    }

    public void GetEnemySpawners()
    {
        enemySpawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy Spawner"));
        foreach (GameObject enemySpawner in enemySpawners)
        {
            enemySpawner.GetComponent<EnemySpawner>().gameObjectManager = this;
        }
    }

    public void GetCivilianSpawners()
    {
        List<GameObject> localCivilianSpawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("Civilian Spawner"));
        foreach (GameObject civilianSpawner in localCivilianSpawners)
        {
            if (civilianSpawner.GetComponent<CivilianSpawner>() != null)
                civilianSpawner.GetComponent<CivilianSpawner>().manager = this;

            if (civilianSpawner.GetComponent<CivilianSource>() != null)
            {
                civilianSpawner.GetComponent<CivilianSource>().manager = this;
                civilianSpawners.Add(civilianSpawner);
            }
        }
    }

    void GetBarricades()
    {
        barricades = new List<GameObject>(GameObject.FindGameObjectsWithTag("Barricade"));
        vitalBarricades = new List<GameObject>();
        foreach (GameObject barricade in barricades)
        {
            if (barricade.GetComponent<BarrierLogic>().vital)
                vitalBarricades.Add(barricade);
        }
    }

    void GetCivilians()
    {
        civilians = new List<GameObject>(GameObject.FindGameObjectsWithTag("Civilian"));
    }

    void GetEndPos()
    {
        endPos = GameObject.FindGameObjectWithTag("End Position");
        GameObject.FindGameObjectWithTag("End Position").GetComponent<Endpoint>().manager = this;
    }

    void GetCivilianDestination()
    {
        civilianDestination = GameObject.FindGameObjectWithTag("CivilianDestination");
    }

    public void SpawnHUD()
    {
        GameObject HUD = Instantiate(HUDPrefab);
        HUD.GetComponent<HUDScript>().manager = this;
    }

    public void GetTurrets()
    {
        turrets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Turret"));
    }

    public bool GetWin()
    {
        if (playersEscaped < 1)
            return false;

        if (civiliansEscaped < initialCivilians * (civiliansRequired / 100))
            return false;

        foreach (GameObject barricade in vitalBarricades)
        {
            if (barricade == null)
                return false;
        }

        return true;
    }

    public void GameOver()
    {
        if (GetWin())
            Instantiate(WinGUIPrefab);
        else
            Instantiate(LoseGUIPrefab);

        playing = false;
    }
}
