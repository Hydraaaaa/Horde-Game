using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class OPEnergy : Item
    {
        void Start()
        {
        }

        void Update()
        {
            GetComponent<PlayerMovScript>().energy += 100 * Time.deltaTime;
        }
    }
}
