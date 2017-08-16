﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAIScript : MonoBehaviour
{
    public delegate void Attack();

    public Attack turretAttack;

    public float maxTargetRange = 5f;
    public GameObjectManager manager;
    public GameObject Target;
    public float targetsDistance;

    public GameObject HorizontalRotator;
    public GameObject VerticalRotator;

    public int damage = 3;
    public float attackInterval = 0.75f;
    public float curAttackInterval;

    public TurretScript turretRef;
    public int TurretNo;
    public float timeLeft;

    // Use this for initialization
    void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameObjectManager>();
        turretAttack = VerticalRotator.GetComponent<TurretRifle>().Attack;
        damage = turretRef.TurInformation[TurretNo - 1].DPS;
    }

    // Update is called once per frame
    void Update ()
    {
        timeLeft = turretRef.TurInformation[TurretNo - 1].curActiveTime;
        VerticalRotator.GetComponent<TurretRifle>().damage = turretRef.TurInformation[TurretNo - 1].DPS;

        if (turretRef.TurInformation[TurretNo - 1].active)
        {
            VerticalRotator.GetComponent<LineRenderer>().startColor = new Color(0, 1, 0, 1);

            if (Target != null)
            {
                if (curAttackInterval < attackInterval)
                    curAttackInterval += Time.deltaTime;
                else
                {
                    curAttackInterval = 0;
                    turretAttack();
                }

                FollowTarget();

                // Calculate the enem's distance from the turret
                targetsDistance = Vector3.Distance(Target.transform.position, transform.position);

                Debug.DrawLine(transform.position, Target.transform.position, Color.red);

                // If the target dies
                if (Target.GetComponent<Health>().health <= 0)
                {
                    // Remove the referance
                    Target = null;
                }

                // If the enemy goes out of range
                if (targetsDistance > maxTargetRange)
                {
                    // Remove the referance
                    Target = null;
                }

            }
            else
            {
                VerticalRotator.transform.rotation = Quaternion.Euler(0, VerticalRotator.transform.rotation.y, VerticalRotator.transform.rotation.z);
            }
        }
        else
        {
            VerticalRotator.GetComponent<LineRenderer>().startColor = new Color(0.5f, 0, 0, 1);
            VerticalRotator.transform.rotation = Quaternion.Euler(22, VerticalRotator.transform.rotation.y, VerticalRotator.transform.rotation.z);
        }
    }


    private void FollowTarget()
    {
        Vector3 dir = Target.transform.position - transform.position;
        HorizontalRotator.transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 500);
        HorizontalRotator.transform.localRotation = Quaternion.Euler(0, HorizontalRotator.transform.rotation.eulerAngles.y, 0);

        VerticalRotator.transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 500);
        VerticalRotator.transform.localRotation = Quaternion.Euler(VerticalRotator.transform.rotation.eulerAngles.x, 0, 0);
    }

    private void OnTriggerStay(Collider col)
    {
        // If the object that entered the collider is an enemy
        if (col.tag == "Enemy")
        {
            // If there is no target currently
            if (Target == null)
            {
                targetsDistance = 0;

                // If the turret has line of sight
                int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
                layermask = 1 << LayerMask.NameToLayer("Enemy");
                layermask = ~layermask;

                if (!Physics.Linecast(VerticalRotator.transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
                {
                    Debug.Log("New Target(null before)");
                    Target = col.gameObject;
                }
            }
            // If there is a target (Options 2 3, and 4) (Closest, Weakest, and Closest & Weakest)
            else
            {
                // If the turret has line of sight
                int layermask = 1 << LayerMask.NameToLayer("SeeThrough");
                layermask = 1 << LayerMask.NameToLayer("Enemy");
                layermask = ~layermask;

                if (!Physics.Linecast(VerticalRotator.transform.position, col.transform.position, layermask, QueryTriggerInteraction.Ignore))
                {
                    // If the new target is closer
                    if (Vector3.Distance(transform.position, col.transform.position) < Vector3.Distance(transform.position, Target.transform.position))
                    {
                        Debug.Log("New Target(closer)");

                        Target = col.gameObject;
                    }

                    //// If new target has less health
                    //if (col.GetComponent<Health>().health < Target.GetComponent<Health>().health)
                    //{

                    //}

                    //// If the new target is closer than the old target and has less health
                    //if (Vector3.Distance(transform.position, col.transform.position) < Vector3.Distance(transform.position, Target.transform.position) &&
                    //    col.GetComponent<Health>().health < Target.GetComponent<Health>().health)
                    //{
                    //    Target = col.gameObject;
                    //}
                }
            }
        }
    }
}
