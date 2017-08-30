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
        GetComponent<Health>().OnDie = PlayerDie;
        
        Destroy(this);
	}

    public void PlayerDie()
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
        {
            if (GameObjectManager.instance.players[i].gameObject == gameObject)
            {
                GameObjectManager.instance.players[i].gameObject = null;
            }
        }
        Destroy(gameObject);
    }
}
