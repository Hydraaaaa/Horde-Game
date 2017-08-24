using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveSystem : MonoBehaviour
{
    public float ReviveTime = 5f;
    public float curReviveTime = 0f;

    public GameObject InteractGUI;

	// Use this for initialization
	void Start ()
    {
        InteractGUI = transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GetComponent<Health>().NeedRes)
        {
            InteractGUI.SetActive(true);
        }
        else
        {
            InteractGUI.SetActive(false);
        }

        InteractGUI.transform.rotation = Quaternion.Euler(45, 45, 0);
	}

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (col.GetComponent<Health>().NeedRes == true)
            {
                if (Input.GetButton("Joy" + GetComponent<PlayerMovScript>().playerNumber + "XButton") || (!GetComponent<PlayerMovScript>().useController && Input.GetKey(KeyCode.E)))
                {
                    curReviveTime += Time.deltaTime;

                    col.GetComponent<ReviveSystem>().InteractGUI.transform.GetChild(1).GetComponent<Image>().fillAmount = curReviveTime / ReviveTime;

                    if (curReviveTime >= ReviveTime)
                    {
                        col.GetComponent<Health>().health = col.GetComponent<Health>().maxHealth;
                        col.GetComponent<Health>().NeedRes = false;
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
