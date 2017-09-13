using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int regularKillScore;
    public int spitterKillScore;
    public int chargerKillScore;
    [Tooltip("Specifically when a player kills a civilian")]
    public int civilianKillScore;
    [Tooltip("Specifically when a player kills a player")]
    public int playerKillScore;
    public int playerDeathScore;
    public int playerReviveScore;
    public int civilianEscapeScore;
    public int scrapPickupScore;
    public int turretUpgradeScore;
    public int turretRepairScore;
    public int civilianDeathScore;
	
	public void RegularKill(int player) { GameObjectManager.instance.players[player].score += regularKillScore; }
    public void SpitterKill(int player) { GameObjectManager.instance.players[player].score += spitterKillScore; }
    public void ChargerKill(int player) { GameObjectManager.instance.players[player].score += chargerKillScore; }
    public void CivilianKill(int player) { GameObjectManager.instance.players[player].score += civilianKillScore; }
    public void PlayerKill(int player) { GameObjectManager.instance.players[player].score += playerKillScore; }
    public void PlayerDeath(int player) { GameObjectManager.instance.players[player].score += playerDeathScore; }
    public void PlayerRevive(int player) { GameObjectManager.instance.players[player].score += playerReviveScore; }
    public void ScrapPickup(int player) { GameObjectManager.instance.players[player].score += scrapPickupScore; }
    public void TurretUpgrade(int player) { GameObjectManager.instance.players[player].score += turretUpgradeScore; }
    public void TurretRepair(int player) { GameObjectManager.instance.players[player].score += turretRepairScore; }

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
