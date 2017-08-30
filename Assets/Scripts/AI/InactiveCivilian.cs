using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveCivilian : MonoBehaviour
{
    public Material activeMaterial;

    CivilianNavigation navigationScript;

    public float range;
    bool rescued;

    float materialChange;

	void Start ()
    {
        navigationScript = GetComponent<CivilianNavigation>();
        navigationScript.enabled = false;

        materialChange = 0;
	}
	
	void Update ()
    {
        if (!rescued)
            for (int i = 0; i < navigationScript.manager.players.Count; i++)
            {
                int mask = ~(1 << 9);
                if (Vector3.Distance(transform.position, navigationScript.manager.players[i].gameObject.transform.position) <= range)
                {
                    if (!Physics.Linecast(transform.position, navigationScript.manager.players[i].gameObject.transform.position, mask))
                    {
                        navigationScript.enabled = true;
                        rescued = true;
                    }
                }
            }

        if (rescued)
            SetMaterial(transform);
    }

    void SetMaterial(Transform obj)
    {
        foreach (Transform child in obj.transform)
        {
            SetMaterial(child);
        }

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            materialChange += Time.deltaTime;
            renderer.material.Lerp(renderer.material, activeMaterial, materialChange);
        }
    }
}
