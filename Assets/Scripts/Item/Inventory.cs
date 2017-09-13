using System.Collections;
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
    public float pickupCooldown;
    [HideInInspector]
    public float currentPickupCooldown;

    Actives actives;
    public Items passive;
    public delegate void ItemActive();
    public ItemActive active;

    GameObject passivePickup;
    GameObject activePickup;

    void Start()
    {
        currentPickupCooldown = 0;
        actives = GetComponent<Actives>();
        passive = Items.none;
        active = null;
    }

    void Update()
    {
        currentPickupCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q) && active != null && !GetComponent<PlayerMovScript>().useController)
        {
            active();
            active = null;
            activePickup = null;
        }
    }

    public void PickUp(GameObject pickup, Items item)
    {
        if (currentPickupCooldown <= 0)
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
            currentPickupCooldown = pickupCooldown;
        }
    }
}