using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealField : MonoBehaviour
{
    public float radius;
    public float duration;

    public float healthPerSec;
    float healing;

    void Start()
    {
        healing = 0;
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            Destroy(gameObject);

        int mask = 1 << LayerMask.NameToLayer("Seethrough");

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask, QueryTriggerInteraction.Ignore);

        healing += healthPerSec * Time.deltaTime;

        for (int i = 0; i < colliders.Length; i++)
        {
            Health health = colliders[i].gameObject.GetComponent<Health>();
            if (health != null && !health.Enemy)
            {
                health.Damage(-Mathf.FloorToInt(healing));
                healing = healing % 1;
            }
        }
    }
}
