using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : Bullet
{
    public GameObject bullet;

    public int pellets;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Explode();

        transform.position += velocity * Time.deltaTime;
    }

    void Explode()
    {
        float angle = 360f / pellets;

        for (int i = 0; i < pellets; i++)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            Vector2 direction = DegreeToVector2(angle * i);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
            bulletScript.velocity = new Vector3(direction.x, 0, direction.y).normalized * velocity.magnitude;
            bulletScript.damage = damage;
            bulletScript.shooter = shooter;
        }
        Destroy(gameObject);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Health>() != null)
        {
            other.gameObject.GetComponent<Health>().Damage(damage, shooter);

            if (other.transform.GetComponent<Health>().Enemy && shooter.tag == "Player")
                other.transform.GetComponent<Health>().Attacker = shooter;
        }
        Destroy(gameObject);
    }
}
