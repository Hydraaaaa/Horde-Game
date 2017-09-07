using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Test,
    Medpack,
    DamageBoost
}

public class Pickup : MonoBehaviour
{
    public ItemInfo item;

    void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null)
            if (inventory.PickUp(item))
                Destroy(gameObject);
    }
}
