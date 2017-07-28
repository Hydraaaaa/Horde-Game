using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int health;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health > maxHealth)
            health = maxHealth;

        if (health <= 0)
            Destroy(gameObject);
    }
}
