using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Shotgun : MonoBehaviour
{
    public bool projectileToggle;

    public GameObject HitIndicatorPrefab;
    public LineRenderer tracerPrefab;
    public Bullet bulletPrefab;

    [Tooltip("Time in seconds between shots")]
    public float cooldown;
    float currentCooldown;

    public float accuracy;
    public int damage;

    public float laserLength;

    public float range;
    public float projectileSpeed;

    public float energyCost;

    public int pellets;

    LineRenderer laser;

    public GameObject laserStartPoint;
    Vector3 laserEndPoint;

    public AudioSource gunshot;

    void Start()
    {
        currentCooldown = 0;

        //        gunshot = Instantiate(gunshot);

        laser = GetComponent<LineRenderer>();
        laser.startWidth = 0.02f;
        laser.endWidth = 0.02f;
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

        int mask = 1 << LayerMask.NameToLayer("CursorRaycast");
        mask += 1 << LayerMask.NameToLayer("Projectile");
        mask = ~mask;

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

            if (projectileToggle)
            {
                for (int i = 0; i < pellets; i++)
                {
                    Vector3 aimDir = (laserStartPoint.transform.right + Random.insideUnitSphere * accuracy) * projectileSpeed;

                    GameObject bullet = Instantiate(bulletPrefab.gameObject, laserStartPoint.transform.position, laserStartPoint.transform.rotation);
                    Bullet bulletScript = bullet.GetComponent<Bullet>();
                    bulletScript.shooter = this.gameObject;
                    bulletScript.velocity = aimDir;
                    if (GetComponent<ReviveSystem>() == null || !GetComponent<ReviveSystem>().NeedRes)
                    {
                        if (GetComponent<Inventory>().passive == Items.damageBoost)
                            bulletScript.damage = Mathf.RoundToInt(damage * 1.3f);
                        else
                            bulletScript.damage = damage;
                    }

                    Collider[] playerColliders = GetComponents<Collider>();
                    for (int j = 0; j < playerColliders.Length; j++)
                        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), playerColliders[j]);
                }
            }
            else
            {
                for (int i = 0; i < pellets; i++)
                {
                    Vector3 aimDir = laserStartPoint.transform.right + Random.insideUnitSphere * accuracy;


                    Ray shootRay = new Ray(laserStartPoint.transform.position, aimDir);
                    RaycastHit hit;


                    int mask = ~(1 << LayerMask.NameToLayer("CursorRaycast"));

                    if (Physics.Raycast(shootRay, out hit, range, mask, QueryTriggerInteraction.Ignore))
                    {
                        GameObject newTracer = Instantiate(tracerPrefab.gameObject);
                        LineRenderer tracerRenderer = newTracer.GetComponent<LineRenderer>();
                        tracerRenderer.SetPosition(0, laserStartPoint.transform.position);
                        tracerRenderer.SetPosition(1, hit.point);

                        Instantiate(HitIndicatorPrefab, hit.point, transform.rotation);

                        if (hit.transform.GetComponent<Health>() != null)
                        {
                            if (GetComponent<ReviveSystem>() == null || !GetComponent<ReviveSystem>().NeedRes)
                            {
                                if (GetComponent<Inventory>().passive == Items.damageBoost)
                                    hit.transform.GetComponent<Health>().Damage(Mathf.RoundToInt(damage * 1.3f), gameObject);
                                else
                                    hit.transform.GetComponent<Health>().Damage(damage, gameObject);
                            }

                            // If the attacked target is an enemy
                            if (hit.transform.GetComponent<Health>().Enemy && this.CompareTag("Player"))
                                hit.transform.GetComponent<Health>().Attacker = this.gameObject;
                        }
                    }
                    else
                    {
                        GameObject newTracer = Instantiate(tracerPrefab.gameObject);
                        LineRenderer tracerRenderer = newTracer.GetComponent<LineRenderer>();
                        tracerRenderer.SetPosition(0, laserStartPoint.transform.position);
                        tracerRenderer.SetPosition(1, laserStartPoint.transform.position + aimDir * range);
                    }
                }
            }

            if (gunshot != null)
                gunshot.Play();
        }
    }
}
