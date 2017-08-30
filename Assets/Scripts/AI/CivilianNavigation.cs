using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CivilianNavigation : MonoBehaviour
{
    NavMeshAgent agent;

    public float speed;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        GetComponent<Health>().OnDie = OnDie;
	}
	
	void Update ()
    {
        Vector3 dir = GetComponent<NavMeshAgent>().velocity;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime
            );
        }

        if (GameObjectManager.instance != null)
        {
            agent.SetDestination(GameObjectManager.instance.civilianDestination.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CivilianDestination"))
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Health>().enabled = false;
            GameObjectManager.instance.civiliansEscaped++;

            GameObjectManager.instance.civilians.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    void OnDie()
    {
        GameObjectManager.instance.civilians.Remove(gameObject);
        GameObjectManager.instance.CheckCivilianCount();
        Destroy(gameObject);
    }
}
