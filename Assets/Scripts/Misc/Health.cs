using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public delegate void NoHealth();
    public NoHealth OnDie;

    public bool Enemy = false;
    public bool Player = false;
    public GameObject Attacker = null;

    void Awake()
    {
        OnDie = Die;
    }

    void Update()
    {
        if (health <= 0)
        {
            OnDie();
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health > maxHealth)
            health = maxHealth;

        if (health <= 0)
        {
            OnDie();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    //public void PlayerDie()
    //{
    //    GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameObjectManager>().players.Remove(gameObject);
    //    Destroy(gameObject);
    //}
}
