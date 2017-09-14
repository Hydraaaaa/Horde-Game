﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public enum Items
{
    none,
    energyBoost,
    damageBoost,
    runBoost,
    blowTorch,
    wrench,
    medkit,
    grenade,
    healField,
}

[RequireComponent(typeof(Actives))]
public class Inventory : MonoBehaviour
{
    Actives actives;
    public Items passive;
    public delegate void ItemActive();
    public ItemActive active;
    
    GameObject passivePickup;
    GameObject activePickup;

    public float pickupRadius;

    void Start()
    {
        actives = GetComponent<Actives>();
        passive = Items.none;
        active = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            AttemptPickup();

        if (Input.GetKeyDown(KeyCode.Q) && active != null && !GetComponent<PlayerMovScript>().useController)
        {
            active();
            active = null;
            activePickup = null;
        }
    }

    public void AttemptPickup()
    {
        int mask = 1 << LayerMask.NameToLayer("Pickup");

        Collider[] pickups = Physics.OverlapSphere(transform.position, pickupRadius, mask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < pickups.Length; i++)
        {
            if (pickups[i].GetComponent<Pickup>())
            {
                PickUp(pickups[i].gameObject, pickups[i].GetComponent<Pickup>().item);
                break;
            }
            if (pickups[i].GetComponent<WeaponPickup>())
            {
                GetComponent<WeaponInventory>().PickUp(pickups[i].gameObject, pickups[i].GetComponent<WeaponPickup>().weapon);
                break;
            }
        }
    }

    public void PickUp(GameObject pickup, Items item)
    {
        switch (item)
        {
            case Items.energyBoost:
                passive = Items.energyBoost;
                if (passivePickup != null)
                {
                    passivePickup.transform.position = transform.position;
                    passivePickup.SetActive(true);
                }
                passivePickup = pickup;
                break;
            case Items.damageBoost:
                passive = Items.damageBoost;
                if (passivePickup != null)
                {
                    passivePickup.transform.position = transform.position;
                    passivePickup.SetActive(true);
                }
                passivePickup = pickup;
                break;
            case Items.runBoost:
                passive = Items.runBoost;
                if (passivePickup != null)
                {
                    passivePickup.transform.position = transform.position;
                    passivePickup.SetActive(true);
                }
                passivePickup = pickup;
                break;
            case Items.medkit:
                active = actives.Medkit;
                if (activePickup != null)
                {
                    activePickup.transform.position = transform.position;
                    activePickup.SetActive(true);
                }
                activePickup = pickup;
                break;
            case Items.grenade:
                active = actives.Grenade;
                if (activePickup != null)
                {
                    activePickup.transform.position = transform.position;
                    activePickup.SetActive(true);
                }
                activePickup = pickup;
                break;
            case Items.healField:
                active = actives.PlaceHealField;
                if (activePickup != null)
                {
                    activePickup.transform.position = transform.position;
                    activePickup.SetActive(true);
                }
                activePickup = pickup;
                break;
        }

        pickup.SetActive(false);
    }
}