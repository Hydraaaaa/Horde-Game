using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rifle : MonoBehaviour
{
    public Bullet bulletPrefab;

    [Tooltip("Time in seconds between shots")]
    public float cooldown;
    float currentCooldown;

    public float accuracy;
    public int damage;

    public float laserLength;

    public float projectileSpeed;

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
        laser.SetWidth(0.02f, 0.02f);
    }

    void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
    }

    void LateUpdate()
    {
        Ray aimRay = new Ray(laserStartPoint.transform.position, laserStartPoint.transform.right);
        RaycastHit hit;
        Physics.queriesHitTriggers = false;

        int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

        if (Physics.Raycast(aimRay, out hit, laserLength, mask))
            laserEndPoint = hit.point;
        else
            laserEndPoint = laserStartPoint.transform.position + laserStartPoint.transform.right * laserLength;

        float distancePercent = (laserEndPoint - laserStartPoint.transform.position).magnitude / laserLength;
        Color endColor = new Color(laser.startColor.r, laser.startColor.g, laser.startColor.b, 1 - distancePercent);
        laser.endColor = endColor;

        laser.SetPosition(0, laserStartPoint.transform.position);
        laser.SetPosition(1, laserEndPoint);
    }

    public void Attack(ref float energy)
    {
        if (currentCooldown <= 0 && energy > energyCost)
        {
            currentCooldown = cooldown;
            energy -= energyCost;

            Vector3 aimDir = (laserStartPoint.transform.right + Random.insideUnitSphere * accuracy) * projectileSpeed;

            GameObject bullet = Instantiate(bulletPrefab.gameObject, laserStartPoint.transform.position, laserStartPoint.transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.velocity = aimDir;
            bulletScript.damage = damage;
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<SphereCollider>());
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<CapsuleCollider>());

            if (gunshot != null)
                gunshot.Play();
        }
    }
}
