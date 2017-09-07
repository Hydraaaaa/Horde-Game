using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Item
{
    public override void Activate()
    {
        GetComponent<Health>().Damage(-40);
    }
}
