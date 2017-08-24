using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    [HideInInspector] public GameObjectManager manager;
    public Text timer;
    public Text civiliansSaved;
    public Text notEnoughCivilians;

    public GameObject p1Mask;
    public GameObject p2Mask;

    public GameObject barricadeHealthBarPrefab;    // Not Implemented
    List<GameObject> barricadeHealthBarsP1;        // Not Implemented
    List<GameObject> barricadeHealthBarsP2;        // Not Implemented

    public GameObject turretTimeBarPrefab;
    List<GameObject> turretTimeBarsP1;
    List<GameObject> turretTimeBarsP2;

    void Start ()
    {
        barricadeHealthBarsP1 = new List<GameObject>();
        barricadeHealthBarsP2 = new List<GameObject>();
        for (int i = 0; i < manager.barricades.Count; i++)
        {
            GameObject newHealthBar = Instantiate(barricadeHealthBarPrefab, transform.position, Quaternion.identity) as GameObject;
            newHealthBar.layer = LayerMask.NameToLayer("P1UI");
            newHealthBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P1UI");
            newHealthBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P1UI");
            newHealthBar.transform.SetParent(p1Mask.transform);
            newHealthBar.transform.SetAsFirstSibling();
            barricadeHealthBarsP1.Add(newHealthBar);

            newHealthBar = Instantiate(barricadeHealthBarPrefab, transform.position, Quaternion.identity) as GameObject;
            newHealthBar.layer = LayerMask.NameToLayer("P2UI");
            newHealthBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P2UI");
            newHealthBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P2UI");
            newHealthBar.transform.SetParent(p2Mask.transform);
            newHealthBar.transform.SetAsFirstSibling();
            barricadeHealthBarsP2.Add(newHealthBar);
        }

        turretTimeBarsP1 = new List<GameObject>();
        turretTimeBarsP2 = new List<GameObject>();
        for (int i = 0; i < manager.turrets.Count; i++)
        {
            GameObject newTimeBar = Instantiate(turretTimeBarPrefab, transform.position, Quaternion.identity) as GameObject;
            newTimeBar.layer = LayerMask.NameToLayer("P1UI");
            newTimeBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P1UI");
            newTimeBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P1UI");
            newTimeBar.transform.SetParent(p1Mask.transform);
            newTimeBar.transform.SetAsFirstSibling();
            turretTimeBarsP1.Add(newTimeBar);

            newTimeBar = Instantiate(turretTimeBarPrefab, transform.position, Quaternion.identity) as GameObject;
            newTimeBar.layer = LayerMask.NameToLayer("P2UI");
            newTimeBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P2UI");
            newTimeBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P2UI");
            newTimeBar.transform.SetParent(p2Mask.transform);
            newTimeBar.transform.SetAsFirstSibling();
            turretTimeBarsP2.Add(newTimeBar);
        }
    }
	
	void Update ()
    {

        if (manager.timer > 60)
        {
            if (manager.timer % 60 < 10)
                timer.text = " " + (Mathf.FloorToInt(manager.timer) / 60).ToString() + ":0" + Mathf.FloorToInt(manager.timer % 60).ToString();
            else
                timer.text = " " + (Mathf.FloorToInt(manager.timer) / 60).ToString() + ":" + Mathf.FloorToInt(manager.timer % 60).ToString();
        }
        else if (manager.timer > 0)
        {
            timer.text = Mathf.FloorToInt(manager.timer).ToString();
        }
        else
            timer.text = "Door Open";

        if (manager.civiliansEscaped < (manager.civiliansRequired / 100.0f) * manager.initialCivilians)
            notEnoughCivilians.enabled = true;
        else
            notEnoughCivilians.enabled = false;

        civiliansSaved.text = manager.civiliansEscaped.ToString() + " ";

        for (int i = 0; i < manager.barricades.Count; i++)
        {
            Health healthScript = manager.barricades[i].GetComponent<Health>();
            if (healthScript.health <= 0)
            {
                barricadeHealthBarsP1[i].SetActive(false);
                barricadeHealthBarsP2[i].SetActive(false);
            }
            else
            {
                barricadeHealthBarsP1[i].SetActive(true);
                barricadeHealthBarsP2[i].SetActive(true);
            }
            barricadeHealthBarsP1[i].transform.position = manager.camera1.GetComponent<Camera>().WorldToScreenPoint(manager.barricades[i].transform.position);
            barricadeHealthBarsP1[i].transform.GetChild(1).GetComponent<Image>().fillAmount = healthScript.health / (float)healthScript.maxHealth;
            
            barricadeHealthBarsP2[i].transform.position = manager.camera2.GetComponent<Camera>().WorldToScreenPoint(manager.barricades[i].transform.position);
            barricadeHealthBarsP2[i].transform.GetChild(1).GetComponent<Image>().fillAmount = healthScript.health / (float)healthScript.maxHealth;
        }

        for (int i = 0; i < manager.turrets.Count; i++)
        {
            TurretAIScript turretScript = manager.turrets[i].GetComponent<TurretAIScript>();
            if (turretScript.timeLeft <= 0)
            {
                turretTimeBarsP1[i].SetActive(false);
                turretTimeBarsP2[i].SetActive(false);
            }
            else
            {
                turretTimeBarsP1[i].SetActive(true);
                turretTimeBarsP2[i].SetActive(true);
            }
            turretTimeBarsP1[i].transform.position = manager.camera1.GetComponent<Camera>().WorldToScreenPoint(manager.turrets[i].transform.position);
            turretTimeBarsP1[i].transform.GetChild(1).GetComponent<Image>().fillAmount = turretScript.timeLeft / (float)turretScript.turretRef.TurInformation[turretScript.TurretNo - 1].activeTime;
            
            turretTimeBarsP2[i].transform.position = manager.camera2.GetComponent<Camera>().WorldToScreenPoint(manager.turrets[i].transform.position);
            turretTimeBarsP2[i].transform.GetChild(1).GetComponent<Image>().fillAmount = turretScript.timeLeft / (float)turretScript.turretRef.TurInformation[turretScript.TurretNo - 1].activeTime;
        }
    }
}
