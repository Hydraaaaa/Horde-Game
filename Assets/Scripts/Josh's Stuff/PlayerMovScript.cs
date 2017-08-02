using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovScript : MonoBehaviour
{
    public delegate void Attack();

    public Attack playerAttack;

    public string playerBeginning = "Joy";
    public int playerNumber = 1;

    public string[] buttonEndings = { "Start" };
    public string[] axisEndings = { "Horizontal", "Vertical", "Shoot", "Horizontal2", "Vertical2" };

    public float moveSpeed = 3;

    public bool useController = true;

    public CharacterController controller;
    public Vector3 direction;
    public Vector3 lookDir;

    public Vector2 screenCenter;

    // Use this for initialization
    void Start ()
    {
        playerBeginning = playerBeginning + playerNumber.ToString();

        // Getting player controller
        controller = GetComponent<CharacterController>();
        playerAttack = GetComponent<Rifle>().Attack;

        // Turning off legacy controls
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
    }
    void Awake()
    {
        playerAttack = null;
    }

    // Update is called once per frame
    void Update ()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (useController)
        {
            CheckKeys();
        }
        else
        {
            // Mouse
            RaycastHit hit;

            Debug.Log(LayerMask.NameToLayer("CursorRaycast"));
            int mask = 1 << LayerMask.NameToLayer("CursorRaycast");

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, mask))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }

            if (Input.GetMouseButton(0) && playerAttack != null)
                playerAttack();


            // Keyboard
            direction = Vector3.zero;
            Vector3 forward = Vector3.Cross(Camera.main.transform.right, Vector3.up);

            if (Input.GetKey(KeyCode.W))
                direction += forward * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                direction -= forward * moveSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.A))
                direction -= Camera.main.transform.right * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                direction += Camera.main.transform.right * moveSpeed * Time.deltaTime;

            controller.Move(direction);
        }
    }

    void CheckKeys()
    {
        direction = Vector3.zero;
        lookDir = Vector3.zero;

        Vector2 lookScreenPos = screenCenter;
        Vector3 forward = Vector3.Cross(Camera.main.transform.right, Vector3.up);

        foreach (string axis in axisEndings)
        {
            string stringCombo = playerBeginning + axis;
            if (Input.GetAxis(stringCombo) > 0.2f ||
                Input.GetAxis(stringCombo) < -0.2f)
            {
                if (axis == "Horizontal")
                    direction += (Camera.main.transform.right) * Input.GetAxis(stringCombo) * moveSpeed * Time.deltaTime;
                else if (axis == "Vertical")
                    direction -= forward * Input.GetAxis(stringCombo) * moveSpeed * Time.deltaTime;
            }

            if (Input.GetAxis(stringCombo) > 0.005 ||
                Input.GetAxis(stringCombo) < 0.005)
            {
                if (axis == "Horizontal2")
                {
                    lookScreenPos = new Vector2(Input.GetAxis(stringCombo), lookScreenPos.y);
                }
                else if (axis == "Vertical2")
                {
                    lookScreenPos = new Vector2(lookScreenPos.x, -Input.GetAxis(stringCombo));
                }
            }
            else
            {
                lookScreenPos = GetComponent<CharacterController>().velocity;
            }

            if (Input.GetAxis(stringCombo) >= 0.5)
            {
                if (axis == "Shoot")
                {
                    playerAttack();
                }
            }
        }

        foreach (string button in buttonEndings)
        {
            string stringCombo = playerBeginning + button;
            if (Input.GetButton(stringCombo))
            {

            }
        }

        lookDir = transform.forward = new Vector3(lookScreenPos.x, 0, lookScreenPos.y);
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, 45, 0);

        controller.Move(direction);
    }
}
