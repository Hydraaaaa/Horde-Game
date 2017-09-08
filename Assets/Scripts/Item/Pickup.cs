using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Items item;

    void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();


        if (inventory != null)
            inventory.PickUp(gameObject, item);
    }
}
