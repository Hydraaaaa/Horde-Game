using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityBlocker : MonoBehaviour
{
    public int currentPlayers;

    List<GameObject> objects;

    bool initialCheck;

    [Range(0, 1)]
    public float transparency;

    void Start()
    {
        currentPlayers = 0;
        objects = new List<GameObject>();
        initialCheck = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
            currentPlayers++;
        else
            objects.Add(other.gameObject);

        CheckList();

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

        CheckList();

        if (currentPlayers == 0)
            SetInvisible();
    }

    void CheckList()
    {
        foreach (GameObject obj in objects)
        {
            if (obj == null)
                objects.Remove(obj);
        }
    }

    void SetVisible()
    {
        foreach (GameObject obj in objects)
        {
            SetVisible(obj.transform);
        }
    }

    void SetVisible(Transform obj)
    {
        foreach (Transform child in obj.transform)
        {
            SetVisible(child);
        }

        if (obj.GetComponent<Renderer>() != null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
        }
    }

    void SetInvisible()
    {
        foreach (GameObject obj in objects)
        {
            SetInvisible(obj.transform);
        }
    }

    void SetInvisible(Transform obj)
    {
        foreach (Transform child in obj.transform)
        {
            SetVisible(child);
        }

        if (obj.GetComponent<Renderer>() != null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            if (CompareTag("Enemy"))
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
            else
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, transparency);
        }
    }
}
