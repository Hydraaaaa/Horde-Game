using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

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

    [Header("Kills & Killstreaks")]
    public int regularKillScore;
    public int spitterKillScore;
    public int chargerKillScore;
    [Space]
    [Tooltip("Increased Multiplier per kill")]
    public float killstreakAddedMultiplier;
    [Tooltip("Maximum Multiplier")]
    public float killstreakMaxMultiplier;
    [Tooltip("Time until killstreak resets")]
    public float killstreakResetTime;

    public int[] currentKillstreak; // Number of kills in streak
    public float[] currentMultiplier; // Resets to 0, a value of 0.5 for example will add 50% of gained kill score to multiplierScore
    public float[] currentResetTime; // Current progress towards killstreak being reset
    public float[] multiplierScore; // Score gained from the multiplier is pooled in this variable and awarded when the streak ends

    public void CivilianKill  (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += civilianKillScore; }
    public void PlayerKill    (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += playerKillScore; }
    public void PlayerDeath   (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += playerDeathScore; }
    public void PlayerRevive  (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += playerReviveScore; }
    public void ScrapPickup   (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += scrapPickupScore; }
    public void TurretUpgrade (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += turretUpgradeScore; }
    public void TurretRepair  (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += turretRepairScore; }
    public void BarrierUpgrade(GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += barrierUpgradeScore; }
    public void BarrierRepair (GameObject player) { if (player != null) GameObjectManager.instance.GetPlayer(player).score += barrierRepairScore; }
    
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

    public void RegularKill(GameObject player)
    {
        if (player != null)
        {
            Player currentPlayer = GameObjectManager.instance.GetPlayer(player);
            currentPlayer.score += regularKillScore;
            currentPlayer.regularKills++;

            int num = player.GetComponent<PlayerMovScript>().playerNumber - 1;
            currentMultiplier[num] += killstreakAddedMultiplier;
            if (currentMultiplier[num] > killstreakMaxMultiplier)
                currentMultiplier[num] = killstreakMaxMultiplier;
            currentResetTime[num] = 0;
            currentKillstreak[num]++;
            multiplierScore[num] += regularKillScore * currentMultiplier[num];
        }
    }
    public void SpitterKill(GameObject player)
    {
        if (player != null)
        {
            Player currentPlayer = GameObjectManager.instance.GetPlayer(player);
            currentPlayer.score += spitterKillScore;
            currentPlayer.spitterKills++;

            int num = player.GetComponent<PlayerMovScript>().playerNumber - 1;
            currentMultiplier[num] += killstreakAddedMultiplier;
            if (currentMultiplier[num] > killstreakMaxMultiplier)
                currentMultiplier[num] = killstreakMaxMultiplier;
            currentResetTime[num] = 0;
            currentKillstreak[num]++;
            multiplierScore[num] += spitterKillScore * currentMultiplier[num];
        }
    }
    public void ChargerKill(GameObject player)
    {
        if (player != null)
        {
            Player currentPlayer = GameObjectManager.instance.GetPlayer(player);
            currentPlayer.score += chargerKillScore;
            currentPlayer.chargerKills++;

            int num = player.GetComponent<PlayerMovScript>().playerNumber - 1;
            currentMultiplier[num] += killstreakAddedMultiplier;
            if (currentMultiplier[num] > killstreakMaxMultiplier)
                currentMultiplier[num] = killstreakMaxMultiplier;
            currentResetTime[num] = 0;
            currentKillstreak[num]++;
            multiplierScore[num] += chargerKillScore * currentMultiplier[num];
        }
    }

    void OnEnable()
    {
        instance = this;
        currentKillstreak = new int[GameObjectManager.instance.players.Count];
        currentMultiplier = new float[GameObjectManager.instance.players.Count];
        currentResetTime = new float[GameObjectManager.instance.players.Count];
        multiplierScore = new float[GameObjectManager.instance.players.Count];
    }

    void Update()
    {
        for (int i = 0; i < currentResetTime.Length; i++)
            currentResetTime[i] += Time.deltaTime;

        for (int i = 0; i < currentResetTime.Length; i++)
            if (currentResetTime[i] > killstreakResetTime)
            {
                currentKillstreak[i] = 0;
                currentMultiplier[i] = 0;
                GameObjectManager.instance.players[i].score += Mathf.FloorToInt(multiplierScore[i]);
                multiplierScore[i] = 0;
            }
    }
}
