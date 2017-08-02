using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveCivilian : MonoBehaviour
{
    public Material inactiveMaterial;
    public Material activeMaterial;

    CivilianNavigation navigationScript;
    Renderer renderer;

    public float range;

	void Start ()
    {
        navigationScript = GetComponent<CivilianNavigation>();
        navigationScript.enabled = false;

        renderer = GetComponent<Renderer>();
        renderer.material = inactiveMaterial;
	}
	
	void Update ()
    {
        for (int i = 0; i < navigationScript.gameObjectManager.players.Count; i++)
        {
            int mask = ~(1 << 9);
            if (Vector3.Distance(transform.position, navigationScript.gameObjectManager.players[i].transform.position) <= range)
            {
                if (!Physics.Linecast(transform.position, navigationScript.gameObjectManager.players[i].transform.position, mask))
                {
                    Debug.Log("Freed");
                    renderer.material = activeMaterial;
                    navigationScript.enabled = true;
                }
            }
        }
	}
}
