using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapons weapon;

    void OnTriggerEnter(Collider other)
    {
        WeaponInventory inventory = other.GetComponent<WeaponInventory>();


        if (inventory != null)
            inventory.PickUp(gameObject, weapon);
    }
}
