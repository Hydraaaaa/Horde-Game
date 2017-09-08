using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Item
{
    Health health;

    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        currentCooldown -= Time.deltaTime;
    }

    public override void Activate()
    {
        if (currentCooldown <= 0 && health.health < health.maxHealth)
        {
            health.Damage(-40);
            currentCooldown = 10;
        }
        else
            Debug.Log(currentCooldown);
    }
}
