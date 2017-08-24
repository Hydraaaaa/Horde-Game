using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Shotgun : MonoBehaviour
{
    public GameObject testObj;
    public GameObject tracer;

    [Tooltip("Time in seconds between shots")]
    public float cooldown;
    float currentCooldown;

    public float accuracy;
    public int damage;
    public int pellets;

    public float laserLength;

    public float range;

    public float energyCost;

    LineRenderer laser;

    public GameObject laserStartPoint;
    Vector3 laserEndPoint;

    public AudioSource gunshot;

    void Start()
    {
        currentCooldown = 0;

        //        gunshot = Instantiate(gunshot);

        laser = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        Ray aimRay = new Ray(laserStartPoint.transform.position, laserStartPoint.transform.right);
        RaycastHit hit;
        Physics.queriesHitTriggers = false;

        int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

        if (Physics.Raycast(aimRay, out hit, laserLength, mask))
            laserEndPoint = hit.point;
        else
            laserEndPoint = laserStartPoint.transform.position + laserStartPoint.transform.right * laserLength;

        laser.SetPosition(0, laserStartPoint.transform.position);
        laser.SetPosition(1, laserEndPoint);

        float distancePercent = (laserEndPoint - laserStartPoint.transform.position).magnitude / laserLength;
        Color endColor = new Color(laser.startColor.r, laser.startColor.g, laser.startColor.b, 1 - distancePercent);
        laser.endColor = endColor;
        laser.SetWidth(0.02f, 0.02f);
    }

    public void Attack(ref float energy)
    {
        if (currentCooldown <= 0 && energy > energyCost)
        {
            currentCooldown = cooldown;
            energy -= energyCost;

            for (int i = 0; i < pellets; i++)
            {
                Vector3 aimDir = laserStartPoint.transform.right + Random.insideUnitSphere * accuracy;

                Ray shootRay = new Ray(laserStartPoint.transform.position, aimDir);
                RaycastHit hit;


                int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

                if (Physics.Raycast(shootRay, out hit, range, mask))
                {
                    GameObject newTracer = Instantiate(tracer);
                    LineRenderer tracerRenderer = newTracer.GetComponent<LineRenderer>();
                    tracerRenderer.SetPosition(0, laserStartPoint.transform.position);
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
                    tracerRenderer.SetPosition(0, laserStartPoint.transform.position);
                    tracerRenderer.SetPosition(1, laserStartPoint.transform.position + aimDir * range);
                }
            }

            if (gunshot != null)
                gunshot.Play();
        }
    }
}
