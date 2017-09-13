using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int regularKillScore;                                  // Not Done
    public int spitterKillScore;                                  // Not Done
    public int chargerKillScore;                                  // Not Done
    [Tooltip("Specifically when a player kills a civilian")]
    public int civilianKillScore;
    [Tooltip("Specifically when a player kills a player")]
    public int playerKillScore;
    public int playerDeathScore;
    [Tooltip("Score to the player who is doing the reviving")]
    public int playerReviveScore;
    public int scrapPickupScore;
    public int turretUpgradeScore;                                // Not Done
    public int turretRepairScore;                                 // Not Done
    public int civilianDeathScore;
    public int civilianEscapeScore;
	
	public void RegularKill  (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += regularKillScore;  Debug.Log("RegularKill"); }
    public void SpitterKill  (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += spitterKillScore;  Debug.Log("SpitterKill");}
    public void ChargerKill  (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += chargerKillScore;  Debug.Log("ChargerKill");}
    public void CivilianKill (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += civilianKillScore; Debug.Log("CivilianKill");}
    public void PlayerKill   (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += playerKillScore;   Debug.Log("PlayerKill");}
    public void PlayerDeath  (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += playerDeathScore;  Debug.Log("PlayerDeath");}
    public void PlayerRevive (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += playerReviveScore; Debug.Log("PlayerRevive");}
    public void ScrapPickup  (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += scrapPickupScore;  Debug.Log("ScrapPickup");}
    public void TurretUpgrade(GameObject player) { GameObjectManager.instance.GetPlayer(player).score += turretUpgradeScore;Debug.Log("TurretUpgrade"); }
    public void TurretRepair (GameObject player) { GameObjectManager.instance.GetPlayer(player).score += turretRepairScore; Debug.Log("TurretRepair");}

    void Start()
    {
        instance = this; 
    }

    public void CivilianDeath()
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            GameObjectManager.instance.players[i].score += civilianDeathScore;
        Debug.Log("CivilianDeath");
    }

    public void CivilianEscape()
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            GameObjectManager.instance.players[i].score += civilianEscapeScore;
        Debug.Log("CivilianEscape");
    }
}
