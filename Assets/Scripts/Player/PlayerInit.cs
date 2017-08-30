using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    public GameObject aimRaycastBoxPrefab;
    public static GameObjectManager manager;

	void Start ()
    {
        GameObject aimRaycastBox = Instantiate(aimRaycastBoxPrefab);
        aimRaycastBox.GetComponent<SetPosition>().target = gameObject;
        GetComponent<Health>().OnDie = PlayerDie;
        
        Destroy(this);
	}

    public void PlayerDie()
    {
        for (int i = 0; i < manager.players.Count; i++)
        {
            if (manager.players[i].gameObject == gameObject)
            {
                manager.players[i].gameObject = null;
            }
        }
        Destroy(gameObject);
    }
}
