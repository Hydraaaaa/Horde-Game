using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameObjectManager.instance.timer <= 0)
            {
                for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
                {
                    if (GameObjectManager.instance.players[i].gameObject == other.gameObject)
                    {
                        Destroy(GameObjectManager.instance.players[i].gameObject);
                    }
                }
                GameObjectManager.instance.playersEscaped++;
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Game Over: Enemy reached end");
            GameObjectManager.instance.Lose();
        }
    }
}
