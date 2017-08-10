using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CivilianNavigation : MonoBehaviour
{
    public GameObjectManager manager;
    NavMeshAgent agent;

    public float speed;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
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

        if (manager != null)
        {
            agent.SetDestination(manager.civilianDestination.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CivilianDestination"))
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Health>().enabled = false;
            manager.civiliansEscaped++;

            manager.civilians.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
