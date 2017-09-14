using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveSystem : MonoBehaviour
{
    public float ReviveTime = 5f;
    public float curReviveTime = 0f;

    public GameObject InteractGUIController;
    public GameObject InteractGUIKeyboard;

    public bool NeedRes = false;

    // Use this for initialization
    void Start ()
    {
        GetComponent<Health>().OnDie = StartReviveSystem;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (NeedRes)
        {
            if (GetComponent<PlayerMovScript>().useController)
                InteractGUIController.SetActive(true);
            else
                InteractGUIKeyboard.SetActive(true);
        }
        else
        {
            InteractGUIController.SetActive(false);
            InteractGUIKeyboard.SetActive(false);
        }

        InteractGUIController.transform.rotation = Quaternion.Euler(45, 45, 0);
        InteractGUIKeyboard.transform.rotation = Quaternion.Euler(45, 45, 0);
    }

    public void StartReviveSystem(GameObject source = null)
    {
        if (!NeedRes)
        {
            ScoreManager.instance.PlayerDeath(gameObject);

            if (source != null && source.CompareTag("Player"))
                ScoreManager.instance.PlayerKill(source);
            NeedRes = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && !NeedRes && col.GetComponent<ReviveSystem>() != null)
        {
            if (col.GetComponent<ReviveSystem>().NeedRes == true)
            {
                if (Input.GetButton("Joy" + GetComponent<PlayerMovScript>().playerNumber + "XButton") || (!GetComponent<PlayerMovScript>().useController && Input.GetKey(KeyCode.E)))
                {
                    curReviveTime += Time.deltaTime;

                    col.GetComponent<ReviveSystem>().InteractGUIController.transform.GetChild(1).GetComponent<Image>().fillAmount = curReviveTime / ReviveTime;
                    col.GetComponent<ReviveSystem>().InteractGUIKeyboard.transform.GetChild(1).GetComponent<Image>().fillAmount = curReviveTime / ReviveTime;

                    if (curReviveTime >= ReviveTime)
                    {
                        col.GetComponent<Health>().health = col.GetComponent<Health>().maxHealth;
                        col.GetComponent<ReviveSystem>().NeedRes = false;
                        ScoreManager.instance.PlayerRevive(gameObject);
                    }
                }
                else
                {
                    curReviveTime = 0f;
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && col.GetComponent<ReviveSystem>() != null)
        {
            col.GetComponent<ReviveSystem>().curReviveTime = 0f;
        }
    }
}
