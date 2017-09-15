using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // Public list of all players in range of the turret
    public List<GameObject> playersInRange;

    public bool Locked = false;
    public bool isOpen = false;

    public float OpenSpeed = 3;
    public float End1, End2;
    float begin1, begin2;

    // Use this for initialization
    void Start ()
    {
        begin1 = transform.GetChild(0).transform.localPosition.z;
        begin2 = transform.GetChild(1).transform.localPosition.z;

    }

    // Update is called once per frame
    void Update ()
    {
        // Make sure the references are still useable
        for (int i = 0; i < playersInRange.Count; i++)
        {
            if (playersInRange[i] == null || (Vector3.Distance(transform.position, playersInRange[i].transform.position) > 4))
            {
                playersInRange.RemoveAt(i);
            }
        }

        if (playersInRange.Count > 0)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }

        if (!isOpen)
        {
            Vector3 pos = transform.GetChild(0).transform.localPosition;
            pos.z = Mathf.Lerp(pos.z, End1, Time.deltaTime * OpenSpeed);
            transform.GetChild(0).transform.localPosition = pos;

            pos = transform.GetChild(1).transform.localPosition;
            pos.z = Mathf.Lerp(pos.z, End2, Time.deltaTime * OpenSpeed);
            transform.GetChild(1).transform.localPosition = pos;
        }
        else
        {
            Vector3 pos = transform.GetChild(0).transform.localPosition;
            pos.z = Mathf.Lerp(pos.z, begin1, Time.deltaTime * OpenSpeed);
            transform.GetChild(0).transform.localPosition = pos;

            pos = transform.GetChild(1).transform.localPosition;
            pos.z = Mathf.Lerp(pos.z, begin2, Time.deltaTime * OpenSpeed);
            transform.GetChild(1).transform.localPosition = pos;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (Vector3.Distance(transform.position, col.transform.position) < 4)
        {
            for (int i = 0; i < playersInRange.Count; i++)
            {
                if (playersInRange[i] == col.gameObject)
                {
                    return;
                }
            }
            playersInRange.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        playersInRange.Remove(col.gameObject);
    }
}
