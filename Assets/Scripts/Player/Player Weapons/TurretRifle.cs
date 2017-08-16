using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TurretRifle : MonoBehaviour
{
    public GameObject testObj;
    public GameObject tracer;

    [Tooltip("Time in seconds between shots")]
    public float cooldown;
    float currentCooldown;

    public float accuracy;
    public int damage;

    public float laserLength;

    public float range;

    LineRenderer laser;

    Vector3 laserEndPoint;

    public AudioSource gunshot;

    void Start()
    {
        currentCooldown = 0;

        laser = GetComponent<LineRenderer>();

        if (gunshot != null)
            gunshot = Instantiate(gunshot);
    }

    void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        Ray aimRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Physics.queriesHitTriggers = false;

        int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

        if (Physics.Raycast(aimRay, out hit, laserLength, mask))
            laserEndPoint = hit.point;
        else
            laserEndPoint = transform.position + transform.forward * laserLength;

        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, laserEndPoint);

        float distancePercent = (laserEndPoint - transform.position).magnitude / laserLength;
        Color endColor = new Color(laser.startColor.r, laser.startColor.g, laser.startColor.b, 1 - distancePercent);
        laser.endColor = endColor;
    }

    public void Attack()
    {
        if (currentCooldown <= 0)
        {
            currentCooldown = cooldown;

            Vector3 aimDir = transform.forward + Random.insideUnitSphere * accuracy;

            Ray shootRay = new Ray(transform.position, aimDir);
            RaycastHit hit;


            int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

            if (Physics.Raycast(shootRay, out hit, range, mask))
            {
                GameObject newTracer = Instantiate(tracer);
                LineRenderer tracerRenderer = newTracer.GetComponent<LineRenderer>();
                tracerRenderer.SetPosition(0, transform.position);
                tracerRenderer.SetPosition(1, hit.point);

                Instantiate(testObj, hit.point, transform.rotation);

                if (hit.transform.GetComponent<Health>() != null)
                {
                    hit.transform.GetComponent<Health>().Damage(damage);

                    // If the attacked target is an enemy
                    if (hit.transform.GetComponent<Health>().Enemy && this.tag == "Player")
                        hit.transform.GetComponent<Health>().Attacker = this.gameObject;
                }
            }
            else
            {
                GameObject newTracer = Instantiate(tracer);
                LineRenderer tracerRenderer = newTracer.GetComponent<LineRenderer>();
                tracerRenderer.SetPosition(0, transform.position);
                tracerRenderer.SetPosition(1, transform.position + aimDir * range);
            }
            if (gunshot != null)
                gunshot.Play();
        }
    }
}
