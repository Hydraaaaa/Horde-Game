using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    [Header("References")]
    public Text timer;
    public Text QuestText;
    public Text civiliansSaved;
    public Text notEnoughCivilians;

    [Header("Prefabs")]
    public GameObject mask;
    public GameObject barrierHealthBar;
    public GameObject turretTimeBar;
    public GameObject struggleBar;
    public GameObject controllerTurretGUI;
    public GameObject PCTurretGUI;
    public GameObject P1Card, P2Card;

    public List<GameObject>[] barrierHealthBars;
    public List<GameObject>[] turretTimeBars;
    public List<GameObject>[] turretGUI;

    
    [HideInInspector] public GameObject[] struggleBars;
    [HideInInspector] public bool[] struggling;
    [HideInInspector] public float[] minSP, maxSP;
    
    void Start ()
    {
        // Instantiate a mask for each player
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
        {
            GameObject maskInstance = Instantiate(mask);
            maskInstance.transform.SetParent(transform);
            maskInstance.name = "P" + (i + 1) + "Mask";
            maskInstance.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            maskInstance.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

            RectTransform rectTransform = maskInstance.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1f / GameObjectManager.instance.players.Count * i, 0);
            rectTransform.anchorMax = new Vector2(1f / GameObjectManager.instance.players.Count * (i + 1), 1);

            if (i == 0)
            {
                GameObject playerCard = Instantiate(P1Card);
                playerCard.transform.SetParent(maskInstance.transform);
                playerCard.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                playerCard.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            }
            if (i == 1)
            {
                GameObject playerCard = Instantiate(P2Card);
                playerCard.transform.SetParent(maskInstance.transform);
                playerCard.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                playerCard.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            }

            GameObjectManager.instance.players[i].UIMask = maskInstance;

            GameObjectManager.instance.players[i].gameObject.GetComponent<PlayerInfoScript>().GetUIElements();
        }

        // Make the barrier UI
        InstantiateBarrierUI();

        // Make the Turret UI
        InstantiateTurretUI();

        // Make struggle UI
        InstantiateStruggleUI();
    }
	
    void InstantiateBarrierUI()
    {
        // Construct the array of lists
        barrierHealthBars = new List<GameObject>[GameObjectManager.instance.players.Count];
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            barrierHealthBars[i] = new List<GameObject>();

        // Iterate through each barrier in the managers list
        for (int i = 0; i < GameObjectManager.instance.barriers.Count; i++)
        {
            for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
            {
                // Instantiate a new healthbar for the barriers
                GameObject newHealthBar = Instantiate(barrierHealthBar) as GameObject;

                // Add to player[j]'s layer mask
                //newHealthBar.layer = LayerMask.NameToLayer("P1UI");
                //newHealthBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P1UI");
                //newHealthBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P1UI");

                // Set as a child of the player[j] mask 
                newHealthBar.transform.SetParent(GameObjectManager.instance.players[j].UIMask.transform);
                newHealthBar.transform.SetAsFirstSibling();

                // Add a reference to the barrier in the local list
                barrierHealthBars[j].Add(newHealthBar);
            }
        }
    }

    void InstantiateTurretUI()
    {
        // Turret Time Bars

        // Construct the array of lists
        turretTimeBars = new List<GameObject>[GameObjectManager.instance.players.Count];
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            turretTimeBars[i] = new List<GameObject>();

        // Iterate through the managers list of turrets
        for (int i = 0; i < GameObjectManager.instance.turrets.Count; i++)
        {
            for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
            {
                // Create a new timer
                GameObject newTimeBar = Instantiate(turretTimeBar) as GameObject;

                // Add it to player 1's layer mask
                //newTimeBar.layer = LayerMask.NameToLayer("P1UI");
                //newTimeBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P1UI");
                //newTimeBar.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P1UI");

                // Set as a child of the player1 mask
                newTimeBar.transform.SetParent(GameObjectManager.instance.players[j].UIMask.transform);
                newTimeBar.transform.SetAsFirstSibling();

                // Add a reference to the barrier in the local list
                turretTimeBars[j].Add(newTimeBar);
            }
        }
        
        // Turret Interaction GUI

        // Construct the array of lists
        turretGUI = new List<GameObject>[GameObjectManager.instance.players.Count];
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            turretGUI[i] = new List<GameObject>();
        
        // Iterate through the managers turret list
        for (int i = 0; i < GameObjectManager.instance.turrets.Count; i++)
        {
            for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
            {
                // Instantiate the correct GUI for the controller type used for player[j]
                if (GameObjectManager.instance.players[j].gameObject.GetComponent<PlayerMovScript>().useController)
                    turretGUI[j].Add(Instantiate(controllerTurretGUI));
                else
                    turretGUI[j].Add(Instantiate(PCTurretGUI));

                // Child to the player[j] mask
                turretGUI[j][i].transform.SetParent(GameObjectManager.instance.players[j].UIMask.transform);

                GameObjectManager.instance.turrets[i].GetComponent<TurretAI>().UIPiece[j] = turretGUI[j][i];
            }
        }
    }

    void InstantiateStruggleUI()
    {
        struggleBars = new GameObject[GameObjectManager.instance.players.Count];
        struggling = new bool[GameObjectManager.instance.players.Count];

        // Iterate through each barrier in the managers list
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
        {
            // Instantiate a new struggle GUI piece
            GameObject newStruggleBar = Instantiate(struggleBar) as GameObject;
            
            // Add to player[i]'s layer mask
            //newStruggleBar.layer = LayerMask.NameToLayer("P1UI");
            //  > Active
            //newStruggleBar.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P1UI");
            
            // > Active > Child: 1, 2, & 3
            //newStruggleBar.transform.GetChild(0).transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("P1UI");
            //newStruggleBar.transform.GetChild(0).transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P1UI");
            //newStruggleBar.transform.GetChild(0).transform.GetChild(2).gameObject.layer = LayerMask.NameToLayer("P1UI");
            
            // Set as a child of the player[i] mask 
            newStruggleBar.transform.SetParent(GameObjectManager.instance.players[i].UIMask.transform);
            newStruggleBar.transform.SetAsFirstSibling();
            
            // Add a reference to the barrier in the local list
            struggleBars[i] = newStruggleBar;
        }
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

        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
        {
            if (struggling[i])
            {
                struggleBars[i].SetActive(true);

                // Update the Barrier's position on player[i]'s screen
                struggleBars[i].transform.position = GameObjectManager.instance.players[0].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.players[0].gameObject.transform.position);
                struggleBars[i].transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().fillAmount = minSP[i] / maxSP[i];
            }
            else
                struggleBars[i].SetActive(false);
        }
        
        // For each barrier in the list
        for (int i = 0; i < GameObjectManager.instance.barriers.Count; i++)
        {
            // Grab the health script of the barrier
            Health healthScript = GameObjectManager.instance.barriers[i].GetComponent<Health>();
        
            // If the barrier's health is equal to or below 0
            if (healthScript.health <= 0)
            {
                // Deactivate the UI
                for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
                    barrierHealthBars[j][i].SetActive(false);
            }
            else
            {
                // Activate the UI
                for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
                    barrierHealthBars[j][i].SetActive(true);
            }

            // Update the Barrier's position on player[j]'s screen
            for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
            {
                barrierHealthBars[j][i].transform.position = GameObjectManager.instance.players[j].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.barriers[i].transform.position);
                barrierHealthBars[j][i].transform.GetChild(1).GetComponent<Image>().fillAmount = healthScript.health / (float)healthScript.maxHealth;
            }
        }
        
        // For each turret in the list
        for (int i = 0; i < GameObjectManager.instance.turrets.Count; i++)
        {
            // Get the turret's script
            TurretAI turretScript = GameObjectManager.instance.turrets[i].GetComponent<TurretAI>();
        
            // If the turrent's remaining time is below 0
            if (turretScript.curActiveTime <= 0)
            {
                // Disable the UI
                for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
                    turretTimeBars[j][i].SetActive(false);
            }
            else
            {
                // Enable the UI
                for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
                    turretTimeBars[j][i].SetActive(true);
            }

            // Update the Turret's position on player[j]'s screen
            for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
            {
                turretTimeBars[j][i].transform.position = GameObjectManager.instance.players[0].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.turrets[i].transform.position);
                turretTimeBars[j][i].transform.GetChild(1).GetComponent<Image>().fillAmount = turretScript.curActiveTime / (float)turretScript.CurrentLevelStats.TotalLifetime;

                turretGUI[j][i].transform.position = GameObjectManager.instance.players[0].camera.GetComponent<Camera>().WorldToScreenPoint(GameObjectManager.instance.turrets[i].transform.position);
            }
        }
    }

    public void GrabTurretsUI(GameObject turret)
    {
        for (int i = 0; i < GameObjectManager.instance.turrets.Count; i++)
        {
            if (GameObjectManager.instance.turrets[i] == turret)
            {
                for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
                    turret.GetComponent<TurretAI>().UIPiece[j] = turretGUI[j][i];
                return;
            }
        }
    }
}
