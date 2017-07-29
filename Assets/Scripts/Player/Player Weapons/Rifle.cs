using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rifle : MonoBehaviour
{
    public GameObject testObj;
    public GameObject tracer;

    [Tooltip("Time in seconds between shots")]
    public float cooldown;
    float currentCooldown;

    [Tooltip("Not implemented yet")]
    public int ammunition;
    int currentAmmunition;

    public float accuracy;
    public int damage;

    public float laserLength;

    LineRenderer laser;

    void Start()
    {
        currentAmmunition = ammunition;
        currentCooldown = 0;

        laser = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        Ray aimRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        laser.SetPosition(0, transform.position);
        Physics.queriesHitTriggers = false;

        int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

        if (Physics.Raycast(aimRay, out hit, laserLength, mask))
        {
            laser.SetPosition(1, hit.point);
            float distancePercent = (hit.point - transform.position).magnitude / laserLength;
            Color endColor = new Color(laser.startColor.r, laser.startColor.g, laser.startColor.b, 1 - distancePercent);
            laser.endColor = endColor;
        }
        else
        {
            laser.SetPosition(1, transform.position + transform.forward * laserLength);

            Color endColor = new Color(laser.startColor.r, laser.startColor.g, laser.startColor.b, 0);
            laser.endColor = endColor;
        }
    }

    public void Attack()
    {
        if (currentCooldown <= 0)
        {
            currentCooldown = cooldown;
            currentAmmunition--;

            Vector3 aimDir = transform.forward + Random.insideUnitSphere * accuracy;

            Ray shootRay = new Ray(transform.position, aimDir);
            RaycastHit hit;

            GameObject newTracer = Instantiate(tracer);
            LineRenderer tracerLine = newTracer.GetComponent<LineRenderer>();
            tracerLine.SetPosition(0, transform.position);

            int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

            if (Physics.Raycast(shootRay, out hit, 100, mask))
            {
                Instantiate(testObj, hit.point, transform.rotation);
                if (hit.transform.CompareTag("Enemy"))
                    hit.transform.GetComponent<Health>().Damage(damage);

                tracerLine.SetPosition(1, hit.point);
            }
            else
                tracerLine.SetPosition(1, transform.position + aimDir * 30);
        }
    }
}
