using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
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
        RaycastHit hit;
        
        int mask = 1 << LayerMask.NameToLayer("CursorRaycast");

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, mask))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }

        if (Input.GetMouseButton(0) && playerAttack != null)
            playerAttack();
	}
}
