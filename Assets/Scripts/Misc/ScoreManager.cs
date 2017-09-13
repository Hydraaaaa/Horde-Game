using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int regularKillScore;
    public int spitterKillScore;
    public int chargerKillScore;
    [Tooltip("Specifically when a player kills a civilian")]
    public int civilianKillScore;
    [Tooltip("Specifically when a player kills a player")]
    public int playerKillScore;
    public int playerDeathScore;
    [Tooltip("Score to the player who is doing the reviving")]
    public int playerReviveScore;
    public int scrapPickupScore;
    public int turretUpgradeScore;
    public int turretRepairScore;
    public int barrierUpgradeScore;
    public int barrierRepairScore;
    public int civilianDeathScore;
    public int civilianEscapeScore;

    public void RegularKill   (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += regularKillScore; }
    public void SpitterKill   (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += spitterKillScore; }
    public void ChargerKill   (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += chargerKillScore; }
    public void CivilianKill  (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += civilianKillScore; }
    public void PlayerKill    (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += playerKillScore; }
    public void PlayerDeath   (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += playerDeathScore; }
    public void PlayerRevive  (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += playerReviveScore; }
    public void ScrapPickup   (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += scrapPickupScore; }
    public void TurretUpgrade (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += turretUpgradeScore; }
    public void TurretRepair  (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += turretRepairScore; }
    public void BarrierUpgrade(GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += barrierUpgradeScore; }
    public void BarrierRepair (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += barrierRepairScore; }

    void Start()
    {
        instance = this; 
    }

    public void CivilianDeath()
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            GameObjectManager.instance.players[i].score += civilianDeathScore;
    }

    public void CivilianEscape()
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            GameObjectManager.instance.players[i].score += civilianEscapeScore;
    }
}
