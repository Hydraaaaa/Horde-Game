using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingScript : MonoBehaviour
{
    public bool Grabbed = false;
    public GameObject Player;
    public ChargerNavigation nav;
    public Vector3 pos;
    public bool firstHit = false;

    // Use this for initialization
    void Start()
    {
        nav = transform.parent.GetComponent<ChargerNavigation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf)
        {
            Player = null;
        }
        if (Player != null)
        {
            //Player.transform.position = nav.HoldingPos.position;
            //Player.transform.rotation = nav.HoldingPos.rotation;

            Player.transform.position = nav.HoldingPos.position;
            Player.transform.rotation = nav.HoldingPos.rotation;

            // Stop player from moving during tackle
            Player.GetComponent<PlayerMovScript>().incapacitated = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (nav.charging && firstHit == false)
        {
            if (col.CompareTag("Player"))
            {
                if (col.GetComponent<ReviveSystem>().NeedRes != true)
                {

                    // Ignore see through things
                    int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
                    layermask = ~layermask;

                    // Dont ignore terrain
                    layermask = 1 << LayerMask.NameToLayer("Terrain");

                    if (!Physics.Linecast(transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
                    {
                        Debug.Log("Hit In Charge");
                        col.GetComponent<Health>().Damage(nav.chargeDamage);
                        firstHit = true;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            if (col == Player)
            {
                Player = null;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        // if the zombie charges into a player
        if (col.gameObject.CompareTag("Player"))
        {
            if (Player == null)
            {
                if (col.GetComponent<ReviveSystem>().NeedRes != true ||
                    col.GetComponent<Health>().health > 0)
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
}
