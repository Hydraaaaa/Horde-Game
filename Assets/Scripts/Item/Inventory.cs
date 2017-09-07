using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Inventory : MonoBehaviour
{
    public float pickupCooldown;
    [HideInInspector]
    public float currentPickupCooldown;

    ItemInfo passiveItemInfo;
    ItemInfo activeItemInfo;
    Item passiveItem;
    Item activeItem;

    List<GameObject> pickupsInRange; // Not Implemented

    private void Start()
    {
        currentPickupCooldown = 0;
        passiveItemInfo = null;
        activeItemInfo = null;
    }

    void Update()
    {
        currentPickupCooldown -= Time.deltaTime;
        if (passiveItemInfo == null)
            Debug.Log("NULL");
        else
            Debug.Log("Not NULL");

        if (Input.GetKeyDown(KeyCode.Q) && activeItem != null)
            activeItem.Activate();
    }

    public bool PickUp(ItemInfo item)
    {
        if (currentPickupCooldown <= 0)
        {
            currentPickupCooldown = 0.5f;
            if (item.active)
            {
                if (activeItemInfo != null)
                {
                    Instantiate(activeItemInfo.pickup.gameObject, transform.position, transform.rotation);
                    Destroy(GetComponent(activeItemInfo.script.GetClass()));
                }

                activeItemInfo = ItemManager.instance.GetItem(item.index);
                gameObject.AddComponent(activeItemInfo.script.GetClass());
                activeItem = GetComponent(activeItemInfo.script.GetClass()) as Item;
            }
            else
            {
                if (passiveItemInfo != null)
                {
                    Instantiate(passiveItemInfo.pickup.gameObject, transform.position, transform.rotation);
                    Destroy(GetComponent(passiveItemInfo.script.GetClass()));
                }

                passiveItemInfo = ItemManager.instance.GetItem(item.index);
                gameObject.AddComponent(passiveItemInfo.script.GetClass());
                passiveItem = GetComponent(passiveItemInfo.script.GetClass()) as Item;
            }
            return true;
        }
        return false;
    }
}
