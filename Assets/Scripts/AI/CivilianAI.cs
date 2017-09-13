﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CivilianAI : MonoBehaviour
{
    public enum QuestList { DECIDE_ON_STARTUP, FIND_QITEM, GIVE_GUN, RESCUE, ESCORT }

    public Animator anim;

    // Enum for the type of quest
    public QuestList Quest;

    public float talkDistance;

    // Dialogue time
    public float popupTime;
    float currentPopupTime;



    [Header("Collection Dialogue")]
    // All dialogue that can be used by the npc
    public string[] startDialogueCollect;
    public string[] endDialogueCollect;
    public string[] todoListInfoCollect;

    [Header("Give Dialogue")]
    public string[] startDialogueGive;
    public string[] endDialogueGive;
    public string[] todoListInfoGive;

    [Header("Rescue Dialogue")]
    public string[] startDialogueRescue;
    public string[] endDialogueRescue;
    public string[] todoListInfoRescue;

    [Header("Escort Dialogue")]
    public string[] startDialogueEscort;
    public string[] endDialogueEscort;
    public string[] todoListInfoEscort;


    [Header("Debug Information")]
    // Public list of all players in range of the npc
    public List<GameObject> playersInRange;

    // bools
    public bool MissionAvailable;
    public bool ObjectiveCompleted;
    public bool MissionCompleted;
    public bool Talking;

    // The row of dialogue used for the npc
    public int questDialogueNo;

    // Use this for initialization
    void Start ()
    {
        // If the mission type is undefined
        if (Quest == QuestList.DECIDE_ON_STARTUP)
        {
            Quest = (QuestList)Random.Range(1, 4);
        }

        // anim = transform.GetChild(0).GetComponent<Animator>();

        // Generate the Quest Text
        switch (Quest)
        {
            case QuestList.ESCORT:
                questDialogueNo = Random.Range(0, startDialogueEscort.Length);
                currentPopupTime = popupTime;
                if (MissionCompleted == true && MissionAvailable == false)
                {
                    GetComponent<CivilianNavigation>().enabled = true;
                }
                break;
            case QuestList.FIND_QITEM:
                questDialogueNo = Random.Range(0, startDialogueCollect.Length);
                currentPopupTime = popupTime;
                if (MissionCompleted == true && MissionAvailable == false)
                {
                    GetComponent<CivilianNavigation>().enabled = true;
                }
                break;
            case QuestList.GIVE_GUN:
                questDialogueNo = Random.Range(0, startDialogueGive.Length);
                currentPopupTime = popupTime;
                if (MissionCompleted == true && MissionAvailable == false)
                {
                    GetComponent<CivilianNavigation>().enabled = true;
                }
                break;
            case QuestList.RESCUE:
                questDialogueNo = Random.Range(0, startDialogueRescue.Length);
                currentPopupTime = popupTime;
                if (MissionCompleted == true && MissionAvailable == false)
                {
                    GetComponent<CivilianNavigation>().enabled = true;
                }
                break;
        }


	}
	
    void EscortType()
    {
    }

    void FindType()
    {
        Debug.Log("UPD");
        anim.SetBool("MissionAvailable", MissionAvailable);
        anim.SetBool("MissionCompleted", MissionCompleted);
        anim.SetBool("Talking", Talking);

        if (MissionCompleted != true)
        {
            // Make sure the referances to the player are still useable
            for (int i = 0; i < playersInRange.Count; i++)
            {
                if (playersInRange[i] == null)
                {
                    playersInRange.RemoveAt(i);
                    break;
                }
                if (Vector3.Distance(playersInRange[i].transform.position, transform.position) > talkDistance)
                {
                    playersInRange.RemoveAt(i);
                    break;
                }

                if (i == 0)
                {
                    Vector3 dir = playersInRange[i].transform.position - transform.position;
                    Quaternion rot = transform.rotation;
                    rot.SetLookRotation(new Vector3(dir.x, dir.y, dir.z));

                    transform.rotation = rot;
                }
                if (MissionAvailable)
                {
                    currentPopupTime = popupTime;
                    GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.text = startDialogueCollect[questDialogueNo];
                    GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.enabled = true;
                    Talking = true;
                    MissionAvailable = false;
                }

                if (ObjectiveCompleted && !MissionCompleted)
                {
                    currentPopupTime = popupTime;
                    MissionCompleted = true;
                    GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.text = endDialogueCollect[questDialogueNo];
                    GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.enabled = true;
                    Talking = true;
                }
            }
        }

        if (currentPopupTime > 0)
        {
            currentPopupTime -= Time.deltaTime;

            if (currentPopupTime <= 0)
            {
                GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.enabled = false;
                Talking = false;

                if (MissionCompleted)
                {
                    GetComponent<CivilianNavigation>().enabled = true;
                }
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        switch (Quest)
        {
            case QuestList.ESCORT:
                EscortType();
                break;
            case QuestList.FIND_QITEM:
                FindType();
                break;
            case QuestList.GIVE_GUN:
                break;
            case QuestList.RESCUE:
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        // If the thing that collides is the player
        if (col.tag == "Player")
        {
            // If they're in range for talking
            if (Vector3.Distance(col.transform.position, transform.position) < talkDistance)
            {
                playersInRange.Add(col.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
    }
}