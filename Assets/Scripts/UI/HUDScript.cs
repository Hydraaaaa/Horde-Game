using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    [HideInInspector] public GameObjectManager manager;
    public Text timer;
    public Text civiliansSaved;

    public GameObject barricadeHealthBarPrefab;
    List<GameObject> barricades;
    List<GameObject> barricadeHealthBars;

	void Start ()
    {
        barricades = manager.barricades;
        barricadeHealthBars = new List<GameObject>();
        foreach (GameObject barricade in barricades)
        {
            GameObject newHealthBar = Instantiate(barricadeHealthBarPrefab) as GameObject;
            newHealthBar.transform.parent = transform;
            barricadeHealthBars.Add(newHealthBar);
        }
    }
	
	void Update ()
    {
        if (manager.timer > 0)
            timer.text = " " + (Mathf.FloorToInt(manager.timer) / 60).ToString() + ":" + Mathf.FloorToInt(manager.timer % 60).ToString();
        else
            timer.text = "Escape";
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
            barricadeHealthBars[i].transform.position = Camera.main.WorldToScreenPoint(manager.barricades[i].transform.position);
            barricadeHealthBars[i].GetComponent<Image>().fillAmount = manager.barricades[i].GetComponent<Health>().health / (float)manager.barricades[i].GetComponent<Health>().maxHealth;
        }
	}
}
