using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    [HideInInspector] public GameObjectManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (manager.timer <= 0)
            {
                for (int i = 0; i < manager.players.Count; i++)
                {
                    if (manager.players[i].gameObject == other.gameObject)
                    {
                        Destroy(manager.players[i].gameObject);
                    }
                }
                manager.playersEscaped++;
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Game Over: Enemy reached end");
            manager.Lose();
        }
    }
}
