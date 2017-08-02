using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public List<GameObject> playerStarts;
    public List<GameObject> players;

    void Start()
    {
        SceneManager.sceneLoaded += Initialize;
    }

    public void Initialize(Scene scene, LoadSceneMode mode)
    {
        playerStarts = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player Start"));
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        int playerCount = GetComponent<GameManager>().playerCount;

        players = new List<GameObject>();

        if (playerStarts.Count != 0)
        {
            for (int i = 0; i < playerCount; i++)
            {
                int playerStartIndex = i % playerStarts.Count;

                GameObject playerInstance = Instantiate(playerPrefab,
                                                        playerStarts[playerStartIndex].transform.position,
                                                        playerStarts[playerStartIndex].transform.rotation);

                players.Add(playerInstance);
            }
        }
        else
            Debug.Log("No player starts found");

    }
}
