using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CivilianAI : MonoBehaviour
{
    public enum QuestList { FIND_QITEM, GIVE_GUN, RESCUE, ESCORT }

    public Animator anim;

    // Enum for the type of quest
    public QuestList Quest;

    // The row of dialogue used for the npc
    public int questDialogueNo;

    public float talkDistance;

    // Dialogue time
    public float popupTime;
    float currentPopupTime;

    // bools
    public bool MissionAvailable;
    public bool ObjectiveCompleted;
    public bool MissionCompleted;
    public bool Talking;

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
        currentPopupTime = popupTime;
        if (MissionCompleted == true && MissionAvailable == false)
        {
            GetComponent<CivilianNavigation>().enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
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
                    GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.text = startDialogue[questDialogueNo];
                    GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.enabled = true;
                    Talking = true;
                    MissionAvailable = false;
                }

                if (ObjectiveCompleted && !MissionCompleted)
                {
                    currentPopupTime = popupTime;
                    MissionCompleted = true;
                    GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.text = endDialogue[questDialogueNo];
                    GameObjectManager.instance.HUD.GetComponent<HUDScript>().QuestText.enabled = true;
                    Talking = true;
                    GetComponent<CivilianNavigation>().enabled = true;
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
            }
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
