using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Test,
    Medpack,
    DamageBoost
}

public class ItemPickup : MonoBehaviour
{
    public ItemType item;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Inventory>() != null)
        {
            other.GetComponent<Inventory>().PickUp(item);
            Destroy(gameObject);
        }
    }
}
