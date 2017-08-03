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
    List<GameObject> barricadeHealthBars;

	void Start ()
    {
        foreach (GameObject barricade in manager.barricades)
        {
            barricadeHealthBars.Add(Instantiate(barricadeHealthBarPrefab));
        }
    }
	
	void Update ()
    {
        timer.text = " " + (Mathf.FloorToInt(manager.timer) / 60).ToString() + ":" + Mathf.FloorToInt(manager.timer % 60).ToString();
        civiliansSaved.text = manager.civiliansEscaped.ToString() + " ";

        foreach (GameObject barricade in manager.barricades)
        {
            Camera.main.WorldToScreenPoint(barricade.transform.position);
        }
	}
}
