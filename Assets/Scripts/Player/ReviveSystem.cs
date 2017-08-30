using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveSystem : MonoBehaviour
{
    public float ReviveTime = 5f;
    public float curReviveTime = 0f;

    public GameObject InteractGUI;

    public bool NeedRes = false;

    // Use this for initialization
    void Start ()
    {
        InteractGUI = transform.GetChild(1).gameObject;
        GetComponent<Health>().OnDie = StartReviveSystem;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (NeedRes)
        {
            InteractGUI.SetActive(true);
        }
        else
        {
            InteractGUI.SetActive(false);
        }

        InteractGUI.transform.rotation = Quaternion.Euler(45, 45, 0);
    }

    public void StartReviveSystem()
    {
        NeedRes = true;
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && !NeedRes)
        {
            if (col.GetComponent<ReviveSystem>().NeedRes == true)
            {
                if (Input.GetButton("Joy" + GetComponent<PlayerMovScript>().playerNumber + "XButton") || (!GetComponent<PlayerMovScript>().useController && Input.GetKey(KeyCode.E)))
                {
                    curReviveTime += Time.deltaTime;

                    col.GetComponent<ReviveSystem>().InteractGUI.transform.GetChild(1).GetComponent<Image>().fillAmount = curReviveTime / ReviveTime;

                    if (curReviveTime >= ReviveTime)
                    {
                        col.GetComponent<Health>().health = col.GetComponent<Health>().maxHealth;
                        col.GetComponent<ReviveSystem>().NeedRes = false;
                        ScoreSystem.instance.PlayerRevived(gameObject);
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
        if (col.tag == "Player")
        {
            col.GetComponent<ReviveSystem>().curReviveTime = 0f;
        }
    }
}
