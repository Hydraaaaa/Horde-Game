using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretRef : MonoBehaviour
{
    public bool Repairing = false;
    
    public GameObject Turret1;
    public GameObject Turret2;
    public GameObject Turret3;
    public GameObject Turret4;

    public Text Cost1;  
    public Text Cost2;  
    public Text Cost3; 

    public Text Active1;
    public Text Active2;
    public Text Active3;

    public Text State1;
    public Text State2;
    public Text State3;

    // Use this for initialization
    void Start ()
    {
        State1 = Turret1.transform.GetChild(2).GetComponent<Text>();
        State2 = Turret2.transform.GetChild(2).GetComponent<Text>();
        State3 = Turret3.transform.GetChild(2).GetComponent<Text>();
        
        Cost1 = Turret1.transform.GetChild(3).GetComponent<Text>();
        Cost2 = Turret2.transform.GetChild(3).GetComponent<Text>();
        Cost3 = Turret3.transform.GetChild(3).GetComponent<Text>();

        Active1 = Turret1.transform.GetChild(4).GetComponent<Text>();
        Active2 = Turret2.transform.GetChild(4).GetComponent<Text>();
        Active3 = Turret3.transform.GetChild(4).GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Repairing)
        {
            State1.text = "Repairing";
            State2.text = "Repairing";
            State3.text = "Repairing";
        }
        else
        {
            State1.text = "Upgrading";
            State2.text = "Upgrading";
            State3.text = "Upgrading";
        }
    }
}
