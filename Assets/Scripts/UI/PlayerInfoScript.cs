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
        ScrapCount = GameObject.Find("ScrapCount" + PlayerNo.ToString()).GetComponent<Text>();
        HealthCount = GameObject.Find("HealthCount" + PlayerNo.ToString()).GetComponent<Text>();
        ScoreCount = GameObject.Find("ScoreCount" + PlayerNo.ToString()).GetComponent<Text>();
        HealthSlider = GameObject.Find("HPBar" + PlayerNo.ToString()).GetComponent<Image>();
        EnergySlider = GameObject.Find("EnergyBar" + PlayerNo.ToString()).GetComponent<Image>();

        playerHP = GetComponent<Health>();
        playerScrap = GetComponent<BarrierPlayersideLogic>();
        playerEnergy = GetComponent<PlayerMovScript>();
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
