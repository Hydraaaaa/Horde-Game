using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    public GameObject testObj;

    [Tooltip("Time in seconds between shots")]
    public float cooldown;
    float currentCooldown;

    [Tooltip("Not implemented yet")]
    public int ammunition;
    int currentAmmunition;

    public int damage;

    void Start()
    {
        currentAmmunition = ammunition;
        currentCooldown = 0;
    }

    void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
    }

    public void Attack()
    {
        if (currentCooldown <= 0)
        {
            currentCooldown = cooldown;
            currentAmmunition--;

            Ray shootRay = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(shootRay, out hit))
            {
                Debug.Log("Hit");
                Instantiate(testObj, hit.point, transform.rotation);
                if (hit.transform.CompareTag("Enemy"))
                    hit.transform.GetComponent<Health>().Damage(damage);
            }
        }
    }
}
