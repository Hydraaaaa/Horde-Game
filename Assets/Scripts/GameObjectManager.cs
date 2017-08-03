using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerPrefab;
    public GameObject HUDPrefab;

    public GameObject camera;
    public GameObject endPos;
    public List<GameObject> playerStarts;
    public List<GameObject> players;
    public List<GameObject> enemySpawners;
    public List<GameObject> enemies;

    public float timer;

    public int civiliansEscaped;

    void Start()
    {
        SceneManager.sceneLoaded += Initialize;
    }

    void Update()
    {
        timer -= Time.deltaTime;
    }

    public void Initialize(Scene scene, LoadSceneMode mode)
    {
        timer = 300;

        playerStarts = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player Start"));
        SpawnCamera();
        SpawnPlayers();
        GetEnemySpawners();
        GetCivilianSpawners();
        SpawnHUD();
        endPos = GameObject.FindGameObjectWithTag("End Position");
    }

    public void SpawnCamera()
    {
        camera = Instantiate(cameraPrefab);
        camera.GetComponent<CameraLogic>().gameObjectManager = this;
    }

    public void SpawnPlayers()
    {
        int playerCount = GetComponent<GameManager>().playerCount;

        players = new List<GameObject>();

        CameraLogic cameraScript = camera.GetComponent<CameraLogic>();

        if (playerStarts.Count != 0)
        {
            for (int i = 0; i < playerCount; i++)
            {
                int playerStartIndex = i % playerStarts.Count;

                GameObject playerInstance = Instantiate(playerPrefab,
                                                        playerStarts[playerStartIndex].transform.position,
                                                        playerStarts[playerStartIndex].transform.rotation);

                playerInstance.GetComponent<PlayerMovScript>().playerNumber = i + 1;
                players.Add(playerInstance);
            }
        }
        else
            Debug.Log("No player starts found");

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
        List<GameObject> civilianSpawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("Civilian Spawner"));
        foreach (GameObject civilianSpawner in civilianSpawners)
        {
            civilianSpawner.GetComponent<CivilianSpawner>().gameObjectManager = this;
        }
    }

    public void SpawnHUD()
    {
        GameObject HUD = Instantiate(HUDPrefab);
        HUD.GetComponent<HUDScript>().manager = this;
    }
}
