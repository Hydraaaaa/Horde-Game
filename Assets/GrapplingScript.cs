using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingScript : MonoBehaviour
{
    public bool Grabbed = false;
    public GameObject Player;
    public ChargerNavigation nav;
    public Vector3 pos;

    // Use this for initialization
    void Start()
    {
        nav = transform.parent.GetComponent<ChargerNavigation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            Player.transform.position = nav.HoldingPos.position;
            Player.transform.rotation = nav.HoldingPos.rotation;
        }
    }

    void OnTriggerStay(Collider col)
    {
        // if the zombie charges into a player
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<Health>().Damage(nav.chargeDamage);

            if (Player == null)
            {
                if (Vector3.Distance(transform.position, col.transform.position) < 2f)
                {
                    col.gameObject.transform.position = nav.HoldingPos.position;
                    col.gameObject.transform.rotation = nav.HoldingPos.rotation;
                    Debug.Log("Grabbed");

                    Player = col.gameObject;
                    Grabbed = true;
                }
            }
        }
    }
}
