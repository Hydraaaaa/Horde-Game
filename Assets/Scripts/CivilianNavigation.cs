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
    bool escaped;

    Renderer renderer;
    float alpha;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        escaped = false;

        renderer = GetComponent<Renderer>();
        alpha = 1;
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

        if (gameObjectManager != null && !escaped)
        {
            agent.SetDestination(gameObjectManager.endPos.transform.position);
        }

        if (escaped)
        {
            alpha -= Time.deltaTime;
            Color newColor = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
            GetComponent<Renderer>().material.color = newColor;

            if (alpha <= 0)
            {
                gameObjectManager.civilians.Remove(gameObject);
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End Position"))
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Health>().enabled = false;
            escaped = true;
            gameObjectManager.civiliansEscaped++;
        }
    }
}
