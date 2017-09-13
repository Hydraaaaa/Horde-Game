using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public int health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            if (currentHealth <= 0)
            {
                OnDie();
            }
        }
    }
    public delegate void NoHealth(GameObject source = null);
    public NoHealth OnDie;

    public bool Enemy = false;
    public bool Player = false;
    public GameObject Attacker = null;

    void Awake()
    {
        OnDie = Die;
    }

    public void Damage(int damage, GameObject source = null)
    {
        health -= damage;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (currentHealth <= 0)
        {
            OnDie(source);
        }
    }

    public void Die(GameObject source = null)
    {
        Destroy(gameObject);
    }

    //public void PlayerDie()
    //{
    //    GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameObjectManager>().players.Remove(gameObject);
    //    Destroy(gameObject);
    //}
}
