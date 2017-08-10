using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierPlayersideLogic : MonoBehaviour
{
    public int Resources = 0;
    public int ResourceCap = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Resources += 100;
        }
    }
}
