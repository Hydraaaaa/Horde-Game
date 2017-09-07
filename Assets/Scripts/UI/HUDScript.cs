using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public Text timer;
    public Text QuestText;
    public Text civiliansSaved;
    public Text notEnoughCivilians;

    public GameObject p1Mask;
    public GameObject p2Mask;

    public GameObject barricadeHealthBarPrefab;

    public List<GameObject> barricadeHealthBarsP1;
    public List<GameObject> barricadeHealthBarsP2;

    public GameObject turretTimeBarPrefab;

    public List<GameObject> turretTimeBarsP1;
    public List<GameObject> turretTimeBarsP2;

    public GameObject controllerTurretGUI;
    public GameObject PCTurretGUI;

    public List<GameObject> turretGUIP1;
    public List<GameObject> turretGUIP2;


    void Start ()
    {
        // Make the barricade UI
        InstantiateBarricadeUI();

        // Make the Turret UI
        InstantiateTurretUI();

        GameObjectManager.instance.players[0].UIMask = p1Mask;
        GameObjectManager.instance.players[1].UIMask = p2Mask;
    }
	
    void InstantiateBarricadeUI()
    {
        // Create two new lists
        barricadeHealthBarsP1 = new List<GameObject>();
        barricadeHealthBarsP2 = new List<GameObject>();

        // Iterate through each barricade in the managers list
        for (int i = 0; i < GameObjectManager.instance.barricades.Count; i++)
        {
            // Instantiate a new healthbar for the barricades
            GameObject newHealthBar = Instantiate(barricadeHealthBarPrefab, transform.position, Quaternion.identity) as GameObject;

            // Add to player 1's layer mask
            newHealthBar.layer = LayerMask.NameToLayer("P1UI");
            newHealthBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P1UI");
            newHealthBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P1UI");

            // Set as a child of the player1 mask 
            newHealthBar.transform.SetParent(p1Mask.transform);
            newHealthBar.transform.SetAsFirstSibling();

            // Add a referance to the barricade in the local list
            barricadeHealthBarsP1.Add(newHealthBar);

            // Instantiate a new healthbar for the barricades
            newHealthBar = Instantiate(barricadeHealthBarPrefab, transform.position, Quaternion.identity) as GameObject;

            // Add to player 2's layer mask
            newHealthBar.layer = LayerMask.NameToLayer("P2UI");
            newHealthBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P2UI");
            newHealthBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P2UI");

            // Set as a child of the player1 mask 
            newHealthBar.transform.SetParent(p2Mask.transform);
            newHealthBar.transform.SetAsFirstSibling();

            // Add a referance to the barricade in the local list
            barricadeHealthBarsP2.Add(newHealthBar);

        }
        return;
    }

    void InstantiateTurretUI()
    {
        // Instantiate new turret lists
        turretTimeBarsP1 = new List<GameObject>();
        turretTimeBarsP2 = new List<GameObject>();

        // Iterate through the managers list of turrets
        for (int i = 0; i < GameObjectManager.instance.turrets.Count; i++)
        {
            // Create a new timer
            GameObject newTimeBar = Instantiate(turretTimeBarPrefab, transform.position, Quaternion.identity) as GameObject;

            // Add it to player 1's layer mask
            newTimeBar.layer = LayerMask.NameToLayer("P1UI");
            newTimeBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P1UI");
            newTimeBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P1UI");

            // Set as a child of the player1 mask
            newTimeBar.transform.SetParent(p1Mask.transform);
            newTimeBar.transform.SetAsFirstSibling();

            // Add a referance to the barricade in the local list
            turretTimeBarsP1.Add(newTimeBar);

            // Create a new timer
            newTimeBar = Instantiate(turretTimeBarPrefab, transform.position, Quaternion.identity) as GameObject;

            // Add it to player 1's layer mask
            newTimeBar.layer = LayerMask.NameToLayer("P2UI");
            newTimeBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P2UI");
            newTimeBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P2UI");

            // Set as a child of the player1 mask
            newTimeBar.transform.SetParent(p2Mask.transform);
            newTimeBar.transform.SetAsFirstSibling();

            // Add a referance to the barricade in the local list
            turretTimeBarsP2.Add(newTimeBar);
        }

        // Instantiate new turret lists
        turretGUIP1 = new List<GameObject>();
        turretGUIP2 = new List<GameObject>();

        // Iterate through the managers turret list
        for (int i = 0; i < GameObjectManager.instance.turrets.Count; i++)
        {
            // Instantiate the correct GUI for the controller type used for player 1
            if (GameObjectManager.instance.players[0].gameObject.GetComponent<PlayerMovScript>().useController)
                turretGUIP1.Add(Instantiate(controllerTurretGUI));
            else
                turretGUIP1.Add(Instantiate(PCTurretGUI));

            // Child to the player 1 mask
            turretGUIP1[i].transform.SetParent(p1Mask.transform);

            // If there is more than 1 player alive, instantiate the correct GUI for player 2
            if (GameObjectManager.instance.players.Count > 1)
            {
                if (GameObjectManager.instance.players[1].gameObject.GetComponent<PlayerMovScript>().useController)
                    turretGUIP2.Add(Instantiate(controllerTurretGUI));
                else
                    turretGUIP2.Add(Instantiate(PCTurretGUI));

                turretGUIP2[i].transform.SetParent(p2Mask.transform);
            }

            GameObjectManager.instance.turrets[i].GetComponent<TurretAI>().P1UIPiece = turretGUIP1[i];
            GameObjectManager.instance.turrets[i].GetComponent<TurretAI>().P2UIPiece = turretGUIP2[i];
        }
        return;
    }

	void Update ()
    {
        // If the timer for the exit bulkhead is still above 60 seconds
        if (GameObjectManager.instance.timer > 60)
        {
            // Put the minute on the left of the semicolon, otherwise leave it empty
            if (GameObjectManager.instance.timer % 60 < 10)
                timer.text = " " + (Mathf.FloorToInt(GameObjectManager.instance.timer) / 60).ToString() + ":0" + Mathf.FloorToInt(GameObjectManager.instance.timer % 60).ToString();
            else
                timer.text = " " + (Mathf.FloorToInt(GameObjectManager.instance.timer) / 60).ToString() + ":" + Mathf.FloorToInt(GameObjectManager.instance.timer % 60).ToString();
        }
        // If the time for the exit bulkhead is less then 60
        else if (GameObjectManager.instance.timer > 0)
        {
            // Just put the time in exactly
            timer.text = Mathf.FloorToInt(GameObjectManager.instance.timer).ToString();
        }
        // Otherwise, if the timer is less than or equal to 0
        else
            timer.text = "Door Open";

        // If the amount of civilians needed to escape has been reached
        if (GameObjectManager.instance.civiliansEscaped < (GameObjectManager.instance.civiliansRequired / 100.0f) * GameObjectManager.instance.initialCivilians)
            notEnoughCivilians.enabled = true;
        // Otherwise just ignore it
        else
            notEnoughCivilians.enabled = false;

        // Display the civilian saved count to the screen
        civiliansSaved.text = GameObjectManager.instance.civiliansEscaped.ToString() + " ";

        // For each barricade in the list
        for (int i = 0; i < GameObjectManager.instance.barricades.Count; i++)
        {
            // Grab the health script of the barricade
            Health healthScript = GameObjectManager.instance.barricades[i].GetComponent<Health>();

            // If the barricade's health is equal to or below 0
            if (healthScript.health <= 0)
            {
                // Disable the UI
                barricadeHealthBarsP1[i].SetActive(false);
                barricadeHealthBarsP2[i].SetActive(false);
            }
            // Otherwise
            else
            {
                // Turn on the UI
                barricadeHealthBarsP1[i].SetActive(true);
                barricadeHealthBarsP2[i].SetActive(true);
            }

            // Update the Barricade's position on player 1's screen
            barricadeHealthBarsP1[i].transform.position = GameObjectManager.instance.players[0].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.barricades[i].transform.position);
            barricadeHealthBarsP1[i].transform.GetChild(1).GetComponent<Image>().fillAmount = healthScript.health / (float)healthScript.maxHealth;
            
            // Update the Barricade's position on player 2's screen
            barricadeHealthBarsP2[i].transform.position = GameObjectManager.instance.players[1].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.barricades[i].transform.position);
            barricadeHealthBarsP2[i].transform.GetChild(1).GetComponent<Image>().fillAmount = healthScript.health / (float)healthScript.maxHealth;
        }

        // For each turret in the list
        for (int i = 0; i < GameObjectManager.instance.turrets.Count; i++)
        {
            // Get the turret's script
            TurretAI turretScript = GameObjectManager.instance.turrets[i].GetComponent<TurretAI>();

            // If the turrent's Lifetime is less than or equal to 0
            if (turretScript.curActiveTime <= 0)
            {
                // Disable the UI
                turretTimeBarsP1[i].SetActive(false);
                turretTimeBarsP2[i].SetActive(false);
            }
            // Otherwise
            else
            {
                // Enable the UI
                turretTimeBarsP1[i].SetActive(true);
                turretTimeBarsP2[i].SetActive(true);
            }

            // Update the Turret's position on player 1's screen
            turretTimeBarsP1[i].transform.position = GameObjectManager.instance.players[0].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.turrets[i].transform.position);
            turretTimeBarsP1[i].transform.GetChild(1).GetComponent<Image>().fillAmount = turretScript.curActiveTime / (float)turretScript.CurrentLevelStats.TotalLifetime;

            turretGUIP1[i].transform.position = GameObjectManager.instance.players[0].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.turrets[i].transform.position);
            turretGUIP2[i].transform.position = GameObjectManager.instance.players[1].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.turrets[i].transform.position);


            // Update the Turret's position on player 2's screen
            turretTimeBarsP2[i].transform.position = GameObjectManager.instance.players[1].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.turrets[i].transform.position);
            turretTimeBarsP2[i].transform.GetChild(1).GetComponent<Image>().fillAmount = turretScript.curActiveTime / (float)turretScript.CurrentLevelStats.TotalLifetime;
        }
    }

    public void GrabTurretsUI(GameObject turret)
    {
        for(int i = 0; i < GameObjectManager.instance.turrets.Count; i++)
        {
            if (GameObjectManager.instance.turrets[i] == turret)
            {
                turret.GetComponent<TurretAI>().P1UIPiece = turretGUIP1[i];
                turret.GetComponent<TurretAI>().P2UIPiece = turretGUIP2[i];
                return;
            }
        }
    }
}
