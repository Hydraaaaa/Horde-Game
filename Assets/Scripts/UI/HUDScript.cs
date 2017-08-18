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

    public GameObject barricadeHealthBarPrefab;
    List<GameObject> barricades;
    List<GameObject> barricadeHealthBars;

    public GameObject turretTimeBarPrefab;
    List<GameObject> turrets;
    List<GameObject> turretTimeBars;

    void Start ()
    {
        barricades = manager.barricades;
        barricadeHealthBars = new List<GameObject>();
        foreach (GameObject barricade in barricades)
        {
            GameObject newHealthBar = Instantiate(barricadeHealthBarPrefab) as GameObject;
            newHealthBar.transform.parent = barricade.transform;
            newHealthBar.transform.localPosition = Vector3.zero;
            //newHealthBar.transform.SetAsFirstSibling();
            barricadeHealthBars.Add(newHealthBar);
        }

        turrets = manager.turrets;
        turretTimeBars = new List<GameObject>();
        foreach (GameObject turret in turrets)
        {
            GameObject newTimeBar = Instantiate(turretTimeBarPrefab) as GameObject;
            newTimeBar.transform.parent = transform;
            newTimeBar.transform.SetAsFirstSibling();
            turretTimeBars.Add(newTimeBar);
        }
    }
	
	void Update ()
    {
        foreach (GameObject barricade in barricades)
        {
            barricade.transform.GetChild(5).transform.localPosition = Vector3.zero;
        }

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

        for (int i = barricades.Count - 1; i >= 0; i--)
        {
            if (barricades[i] == null)
            {
                barricades.RemoveAt(i);
                Destroy(barricadeHealthBars[i]);
                barricadeHealthBars.RemoveAt(i);
            }
        }

        for (int i = 0; i < manager.barricades.Count; i++)
        {
            if (manager.barricades[i].GetComponent<Health>().health <= 0)
                barricadeHealthBars[i].SetActive(false);
            else
                barricadeHealthBars[i].SetActive(true);
            barricadeHealthBars[i].transform.position = Camera.main.WorldToScreenPoint(manager.barricades[i].transform.position);
            barricadeHealthBars[i].transform.GetChild(1).GetComponent<Image>().fillAmount = manager.barricades[i].GetComponent<Health>().health / (float)manager.barricades[i].GetComponent<Health>().maxHealth;
        }

        for (int i = 0; i < manager.turrets.Count; i++)
        {
            TurretAIScript turretScript = manager.turrets[i].GetComponent<TurretAIScript>();
            if (turretScript.timeLeft <= 0)
                turretTimeBars[i].SetActive(false);
            else
                turretTimeBars[i].SetActive(true);
            turretTimeBars[i].transform.position = Camera.main.WorldToScreenPoint(manager.turrets[i].transform.position);
            turretTimeBars[i].transform.GetChild(1).GetComponent<Image>().fillAmount = turretScript.timeLeft / (float)turretScript.turretRef.TurInformation[turretScript.TurretNo - 1].activeTime;
        }
    }
}
