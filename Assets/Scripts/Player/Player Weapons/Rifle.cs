using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(LineRenderer))]
public class Rifle : NetworkBehaviour
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

    [SyncVar]
    Vector3 laserEndPoint;

    void Start()
    {
        currentAmmunition = ammunition;
        currentCooldown = 0;

        laser = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (isLocalPlayer)
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

            CmdSyncLaserEndPoint(laserEndPoint);
        }

        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, laserEndPoint);

        float distancePercent = (laserEndPoint - transform.position).magnitude / laserLength;
        Color endColor = new Color(laser.startColor.r, laser.startColor.g, laser.startColor.b, 1 - distancePercent);
        laser.endColor = endColor;
    }

    [Command]
    void CmdSyncLaserEndPoint(Vector3 endpoint)
    {
        laserEndPoint = endpoint;
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
                CmdShoot(hit.point);
                if (hit.transform.CompareTag("Enemy"))
                    hit.transform.GetComponent<Health>().Damage(damage);

                tracerLine.SetPosition(1, hit.point);
            }
            else
                tracerLine.SetPosition(1, transform.position + aimDir * 30);
        }
    }

    [Command]
    void CmdShoot(Vector3 destinationPos)
    {
        NetworkServer.Spawn(Instantiate(testObj, destinationPos, transform.rotation) as GameObject);
    }
}
