using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAttack : NetworkBehaviour
{
    public delegate void Attack();

    public Attack playerAttack;
    
    void Awake()
    {
        playerAttack = null;
    }

    void Start ()
    {
        playerAttack = GetComponent<Rifle>().Attack;
	}
	
	void Update ()
    {
        if (isLocalPlayer)
        {
            RaycastHit hit;

            Debug.Log(LayerMask.NameToLayer("CursorRaycast"));
            int mask = 1 << LayerMask.NameToLayer("CursorRaycast");

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, mask))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }

            if (Input.GetMouseButton(0) && playerAttack != null)
                playerAttack();
        }
	}
}
