using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    // Barricade Game object holders
    public GameObject BarricadeOne = null;
    public GameObject BarricadeTwo = null;
    public GameObject BarricadeThree = null;

    // Barricade Textures (Hidden)
    public Material BarricadeOneTex;
    public Material BarricadeTwoTex;
    public Material BarricadeThreeTex;

    // Barricade textures (Shown)
    public Material HologramRenderer;
    public Color CanBuildColor = Color.cyan;
    public Color CantBuildColor = Color.red;


    // Keycode variables
    public KeyCode KeyOne = KeyCode.Keypad1;
    public KeyCode KeyTwo = KeyCode.Keypad2;
    public KeyCode KeyThree = KeyCode.Keypad3;

    public KeyCode CancelConstruction = KeyCode.Q;

    public GameObject ConstructionLine = null;

    // Public Variables
    public bool Building = false;
    public bool CanBuild = false;

    public float MaxPlaceDistance = 5.0f;

    // Private Variables
    enum ConstructionItem { NULL, ITEM_ONE, ITEM_TWO, ITEM_THREE };
    ConstructionItem ConsType;

    // Use this for initialization
    void Start ()
    {
        BarricadeOneTex = BarricadeOne.GetComponent<Renderer>().material;
        BarricadeTwoTex = BarricadeOne.GetComponent<Renderer>().material;
        BarricadeThreeTex = BarricadeOne.GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Building)
            BuildingPhase(ConstructionLine);
        else
            NotBuildingPhase(ConstructionLine);

        if (Input.GetKeyDown(CancelConstruction))
        {
            ConsType = ConstructionItem.NULL;   
            Building = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CanBuild = !CanBuild;
        }
    }

    void BuildingPhase(GameObject obj)
    {
        // Player Inputs
        if (Input.GetKeyDown(KeyOne))
        {
            ConsType = ConstructionItem.ITEM_ONE;
            obj = BarricadeOne;
        }
        if (Input.GetKeyDown(KeyTwo))
        {
            ConsType = ConstructionItem.ITEM_TWO;
            obj = BarricadeTwo;
        }
        if (Input.GetKeyDown(KeyThree))
        {
            ConsType = ConstructionItem.ITEM_THREE;
            obj = BarricadeThree;
        }
        if (Input.GetMouseButton(0))
        {

        }

        // Project the barrier
        ProjectBarrier(obj);
    }

    void ProjectBarrier(GameObject obj)
    {
        // Get Position (Distance) for the object to be placed
        Vector3 Pos = this.transform.position + this.transform.forward * MaxPlaceDistance;

        // Set the object to look at the player
        obj.transform.position = Pos;
        obj.transform.rotation.SetLookRotation(this.transform.position);

        obj.GetComponent<Renderer>().material = HologramRenderer;

        if (CanBuild)
            obj.GetComponent<Renderer>().material.color = CanBuildColor;
        else
            obj.GetComponent<Renderer>().material.color = CantBuildColor;
    }

    void NotBuildingPhase(GameObject obj)
    {
        switch (ConsType)
        {
            case ConstructionItem.NULL:
                break;
            case ConstructionItem.ITEM_ONE:
                obj.GetComponent<Renderer>().material = BarricadeOneTex;
                break;
            case ConstructionItem.ITEM_TWO:
                obj.GetComponent<Renderer>().material = BarricadeTwoTex;
                break;
            case ConstructionItem.ITEM_THREE:
                obj.GetComponent<Renderer>().material = BarricadeThreeTex;
                break;
            default:
                break;
        }
    }
}
