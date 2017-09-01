using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    PassiveItem passiveItem;
    ActiveItem activeItem;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void PickUp(ItemType item)
    {
        switch (item)
        {
            case ItemType.Test:
                if (passiveItem != null)
                    Instantiate(passiveItem.pickupPrefab, transform.position, transform.rotation);
                passiveItem = new Items.Test();
                break;
        }
    }
}
