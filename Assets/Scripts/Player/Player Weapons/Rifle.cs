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

    public float accuracy;
    public int damage;

    public float laserLength;

    public float range;

    LineRenderer laser;

    [SyncVar]
    Vector3 laserEndPoint;

    void Start()
    {
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

            Vector3 aimDir = transform.forward + Random.insideUnitSphere * accuracy;

            Ray shootRay = new Ray(transform.position, aimDir);
            RaycastHit hit;


            int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

            if (Physics.Raycast(shootRay, out hit, range, mask))
            {
                CmdShootHit(hit.point);
                if (hit.transform.CompareTag("Enemy"))
                    hit.transform.GetComponent<Health>().Damage(damage);
            }
            else
                CmdShootMiss(aimDir);
        }
    }

    [Command]
    void CmdShootHit(Vector3 destinationPos)
    {
        GameObject newTracer = Instantiate(tracer);
        Tracer tracerScript = newTracer.GetComponent<Tracer>();
        tracerScript.startPos = transform.position;
        tracerScript.endPos = destinationPos;
        Debug.Log(destinationPos);

        NetworkServer.Spawn(newTracer);

        NetworkServer.Spawn(Instantiate(testObj, destinationPos, transform.rotation) as GameObject);
    }

    [Command]
    void CmdShootMiss(Vector3 direction)
    {
        GameObject newTracer = Instantiate(tracer);
        Tracer tracerScript = newTracer.GetComponent<Tracer>();
        tracerScript.startPos = transform.position;
        tracerScript.endPos = transform.position + direction * range;
        Debug.Log(direction * range);

        NetworkServer.Spawn(newTracer);
    }
}
