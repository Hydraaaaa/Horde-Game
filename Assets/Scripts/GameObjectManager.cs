using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerPrefab;

    public GameObject camera;
    public GameObject endPos;
    public List<GameObject> playerStarts;
    public List<GameObject> players;
    public List<GameObject> enemySpawners;
    public List<GameObject> enemies;

    void Start()
    {
        SceneManager.sceneLoaded += Initialize;
    }

    public void Initialize(Scene scene, LoadSceneMode mode)
    {
        playerStarts = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player Start"));
        SpawnCamera();
        SpawnPlayers();
        GetEnemySpawners();
        GetCivilianSpawners();
        endPos = GameObject.FindGameObjectWithTag("End Position");
        Debug.Log(endPos);
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
}
