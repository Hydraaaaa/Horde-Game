using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    public GameObject aimRaycastBoxPrefab;

	void Start ()
    {
        GameObject aimRaycastBox = Instantiate(aimRaycastBoxPrefab);
        aimRaycastBox.GetComponent<SetPosition>().target = gameObject;

        Destroy(this);
	}
}
