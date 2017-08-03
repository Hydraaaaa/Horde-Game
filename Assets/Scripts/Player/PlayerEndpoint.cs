using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndpoint : MonoBehaviour
{
    public GameObjectManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (CompareTag("End Position"))
        {
            if (manager.timer <= 0)
            {
                manager.players.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
