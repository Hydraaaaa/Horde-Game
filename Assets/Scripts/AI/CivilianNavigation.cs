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

        if (GameObjectManager.instance != null)
        {
            agent.SetDestination(GameObjectManager.instance.civilianDestination.transform.position);
        }
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CivilianDestination")) // Escape
        {
            GameObjectManager.instance.civiliansEscaped++;

            GameObjectManager.instance.civilians.Remove(gameObject);
            ScoreManager.instance.CivilianEscape();
            Destroy(gameObject);
        }
    }

    void OnDie(GameObject source = null)
    {
        GameObjectManager.instance.civilians.Remove(gameObject);
        GameObjectManager.instance.CheckCivilianCount();
        ScoreManager.instance.CivilianDeath();
        if (source != null && source.CompareTag("Player"))
            ScoreManager.instance.CivilianKill(source);
        Destroy(gameObject);
    }
}
