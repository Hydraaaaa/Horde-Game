using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CivilianNavigation : MonoBehaviour
{
    public GameObjectManager gameObjectManager;
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

        if (gameObjectManager != null)
        {
            agent.SetDestination(gameObjectManager.civilianDestination.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CivilianDestination"))
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Health>().enabled = false;
            gameObjectManager.civiliansEscaped++;

            gameObjectManager.civilians.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
