using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
	void Start ()
    {
        transform.GetChild(0).SetParent(null);
        transform.GetChild(0).SetParent(null);

        Destroy(this);
	}
}
