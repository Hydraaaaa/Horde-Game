using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoScript : MonoBehaviour
{
    public int PlayerNo = -1;

    public Text ScrapCount;
    public Text HealthCount;

    public Health playerHP;
    public BarrierPlayersideLogic playerScrap;

    // Use this for initialization
	void Start ()
    {
        ScrapCount = GameObject.Find("ScrapCount" + PlayerNo.ToString()).GetComponent<Text>();
        HealthCount = GameObject.Find("HealthCount" + PlayerNo.ToString()).GetComponent<Text>();

        playerHP = GetComponent<Health>();
        playerScrap = GetComponent<BarrierPlayersideLogic>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        HealthCount.text = playerHP.health.ToString();
        ScrapCount.text = playerScrap.Resources.ToString();
	}
}
