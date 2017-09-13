using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public delegate void NoHealth(GameObject source = null);
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
        if (health > maxHealth)
            health = maxHealth;

        if (health <= 0)
            OnDie();
    }

    public void Damage(int damage, GameObject source = null)
    {
        health -= damage;

        if (health > maxHealth)
            health = maxHealth;

        if (health <= 0)
            OnDie(source);
    }

    public void Die(GameObject source = null)
    {
        Destroy(gameObject);
    }
}
