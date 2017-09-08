using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actives : MonoBehaviour
{
    public int medkitHealth;
    public GameObject grenade;

    public void Medkit()
    {
        GetComponent<Health>().Damage(-medkitHealth);
    }

    public void Grenade()
    {
        Instantiate(grenade, transform.position, transform.rotation);
    }
}
