using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerPrefab;

    public GameObject camera;
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
}
