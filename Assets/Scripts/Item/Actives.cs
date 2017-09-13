using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actives : MonoBehaviour
{
    public int medkitHealth;
    public GameObject grenade;
    public GameObject healField;

    public void Medkit()
    {
        GetComponent<Health>().Damage(-medkitHealth);
    }

    public void Grenade()
    {
        GameObject spawnedGrenade = Instantiate(grenade, transform.position, transform.rotation);
        spawnedGrenade.GetComponent<Rigidbody>().AddForce(transform.forward * 400);

        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            Physics.IgnoreCollision(spawnedGrenade.GetComponent<Collider>(), colliders[i]);
    }

    public void PlaceHealField()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        
        int mask = 1 << LayerMask.NameToLayer("CursorRaycast");
        mask += 1 << LayerMask.NameToLayer("Projectile");
        mask += 1 << LayerMask.NameToLayer("Seethrough");
        mask = ~mask;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10, mask))
        {
            Instantiate(healField, hit.point, Quaternion.Euler(90, 0, 0));
        }
    }
}
