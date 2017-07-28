using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public GameObject player;
    public GameObject camera;
}

public class GameObjectManager : MonoBehaviour
{
    List<PlayerData> players;
    List<GameObject> enemies;

    void Start ()
    {
        players = new List<PlayerData>();
        List<PlayerMovement> foundPlayers = new List<PlayerMovement>(FindObjectsOfType(typeof(PlayerMovement)) as PlayerMovement[]);
        
        foreach (PlayerMovement foundPlayer in foundPlayers)
        {
            PlayerData foundPlayerData = new PlayerData();

            foundPlayerData.player = foundPlayer.gameObject;

            players.Add(foundPlayerData);
        }

	}
}
