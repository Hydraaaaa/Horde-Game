using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public float pickupCooldown;
    [HideInInspector]
    public float currentPickupCooldown;

    ItemInfo passiveItem;
    ItemInfo activeItem;

    private void Start()
    {
        currentPickupCooldown = 0;
        passiveItem = null;
        activeItem = null;
    }

    void Update()
    {
        currentPickupCooldown -= Time.deltaTime;
        if (passiveItem == null)
            Debug.Log("NULL");
        else
            Debug.Log("Not NULL");

    }

    public bool PickUp(ItemInfo item)
    {
        if (currentPickupCooldown <= 0)
        {
            currentPickupCooldown = 0.5f;
            if (item.active)
            {
                if (activeItem != null)
                    Instantiate(activeItem.pickup.gameObject, transform.position, transform.rotation);

            activeItem = item;
            }
            else
            {
                if (passiveItem != null)
                    Instantiate(passiveItem.pickup.gameObject, transform.position, transform.rotation);

                passiveItem = ItemManager.instance.GetItem(item.index);
            }
            return true;
        }
        return false;
    }
}
