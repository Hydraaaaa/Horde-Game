using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityBlocker : MonoBehaviour
{
    int currentPlayers;

    List<GameObject> objects;

    bool initialCheck;

	void Start ()
    {
        currentPlayers = 0;
        objects = new List<GameObject>();
        initialCheck = false;
	}
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            currentPlayers++;
        else
            objects.Add(other.gameObject);

        if (currentPlayers > 0)
            SetVisible();
    }

    void OnTriggerStay(Collider other)
    {
        if (!initialCheck)
        {
            initialCheck = true;

            if (other.CompareTag("Player"))
                currentPlayers++;
            else
                objects.Add(other.gameObject);

            if (currentPlayers > 0)
                SetVisible();

            if (currentPlayers == 0)
                SetInvisible();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            currentPlayers--;
        else
            objects.Remove(other.gameObject);

        if (currentPlayers == 0)
            SetInvisible();
    }

    void SetVisible()
    {
        foreach (GameObject obj in objects)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
        }
    }

    void SetInvisible()
    {
        foreach (GameObject obj in objects)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            if (CompareTag("Enemy"))
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
            else
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.25f);
        }
    }
}
