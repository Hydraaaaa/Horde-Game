using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoScript : MonoBehaviour
{
    public int PlayerNo = -1;

    public Text ScrapCount;
    public Text HealthCount;
    public Text ScoreCount;
    public Image HealthSlider;
    public Image EnergySlider;

    public Health playerHP;
    public BarrierPlayersideLogic playerScrap;
    public PlayerMovScript playerEnergy;

	void Start ()
    {
        playerHP = GetComponent<Health>();
        playerScrap = GetComponent<BarrierPlayersideLogic>();
        playerEnergy = GetComponent<PlayerMovScript>();
	}

    public void GetUIElements()
    {
        ScrapCount =   GameObjectManager.instance.players[PlayerNo - 1].UIMask.transform.GetChild(0).Find("ScrapCount" + PlayerNo.ToString()).GetComponent<Text>();
        HealthCount =  GameObjectManager.instance.players[PlayerNo - 1].UIMask.transform.GetChild(0).Find("HealthCount" + PlayerNo.ToString()).GetComponent<Text>();
        ScoreCount =   GameObjectManager.instance.players[PlayerNo - 1].UIMask.transform.GetChild(0).Find("ScoreCount" + PlayerNo.ToString()).GetComponent<Text>();
        HealthSlider = GameObjectManager.instance.players[PlayerNo - 1].UIMask.transform.GetChild(0).Find("HPBar" + PlayerNo.ToString()).GetComponent<Image>();
        EnergySlider = GameObjectManager.instance.players[PlayerNo - 1].UIMask.transform.GetChild(0).Find("EnergyBar" + PlayerNo.ToString()).GetComponent<Image>();
    }

    void Update ()
    {
        HealthCount.text = playerHP.health.ToString();
        ScoreCount.text = GameObjectManager.instance.GetPlayer(gameObject).score.ToString();
        ScrapCount.text = playerScrap.Resources.ToString();


        float fillAmount = ((float)playerHP.health / (float)playerHP.maxHealth);
        HealthSlider.fillAmount = fillAmount;

        fillAmount = ((float)playerEnergy.energy / (float)playerEnergy.maxEnergy);
        EnergySlider.fillAmount = fillAmount;
	}
}
