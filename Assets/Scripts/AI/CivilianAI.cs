using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAI : MonoBehaviour
{
    public enum QuestList { FIND_QITEM, GIVE_GUN, RESCUE, ESCORT }

    // Enum for the type of quest
    public QuestList Quest;

    // The row of dialogue used for the npc
    public int questDialogueNo;

    public float talkDistance;

    // Dialogue time
    public int popupTime;
    int currentPopupTime;

    // bools
    public bool MissionAvailable;
    public bool MissionCompleted;

    // All dialogue that can be used by the npc
    public string[] startDialogue;
    public string[] endDialogue;
    public string[] todoListInfo;


    // Public list of all players in range of the npc
    public List<GameObject> playersInRange;

    // Use this for initialization
    void Start ()
    {
        questDialogueNo = Random.Range(0, startDialogue.Length);
	}
	
	// Update is called once per frame
	void Update ()
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

            if (MissionAvailable)
            {
                currentPopupTime = popupTime;
                Debug.Log(startDialogue);
                MissionAvailable = false;
            }
        }

        if (MissionAvailable == false)
        {
            currentPopupTime -= popupTime;
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
