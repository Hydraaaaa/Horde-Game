using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float ControllerMenuAxisSensitivity = 0.75f;
    public float ControllerNextInputCheckTimeGap = 0.2f;
    public float timeBetweenMovement;

    public string[] Joysticks;

    public Vector2 p1Axis;
    public float p1Trigg;
    bool hasShot = false;

    public int playerCount;

    public string[] ButtonLocations = { "TestScene", "Josh'sScene" };
    public int buttonNo;

    public bool atMenu = true;
    public Text arrow;

    void Awake()
    {
        List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Game Manager"));
        if (managers.Count > 1)
        {
            foreach (GameObject manager in managers)
            {
                if (manager != gameObject)
                    Destroy(manager);
            }
        }
    }

	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
    {
        if (atMenu)
        {
            // Left Thumbstick
            if (timeBetweenMovement < ControllerNextInputCheckTimeGap)
            {
                timeBetweenMovement += Time.deltaTime;
            }
            else if (timeBetweenMovement >= ControllerNextInputCheckTimeGap)
            {
                CheckAxis();
            }

            // Right Trigger
            if (!Input.GetButton("Joy1AButton"))
            {
                hasShot = false;
            }

            if (Input.GetButton("Joy1AButton") && !hasShot)
            {
                Debug.Log("Bang!");
                LoadScene(ButtonLocations[buttonNo]);
                hasShot = true;
                atMenu = false;
            }

            if (arrow != null)
            {
                if (buttonNo == 0)
                {
                    arrow.rectTransform.transform.localPosition = (new Vector3(arrow.transform.localPosition.x, 30, arrow.transform.position.z));
                }
                if (buttonNo == 1)
                {
                    arrow.rectTransform.transform.localPosition = (new Vector3(arrow.transform.localPosition.x, 0, arrow.transform.position.z));
                }
            }
        }
        
    }

    void CheckAxis()
    {
        p1Axis = new Vector2(Input.GetAxis("Joy1Horizontal"), Input.GetAxis("Joy1Vertical"));
        p1Trigg = (Input.GetAxis("Joy1Shoot"));
        if (Input.GetAxis("Joy1Horizontal") != 0)
        {
            if (Input.GetAxis("Joy1Horizontal") >= ControllerMenuAxisSensitivity)
            {
                timeBetweenMovement = 0;
            }
            else if (Input.GetAxis("Joy1Horizontal") <= -ControllerMenuAxisSensitivity)
            {
                timeBetweenMovement = 0;
            }
        }
        if (Input.GetAxis("Joy1Vertical") >= 0.65 ||
            Input.GetAxis("Joy1Vertical") <= -0.65)
        {
            if (Input.GetAxis("Joy1Vertical") <= ControllerMenuAxisSensitivity)
            {
                timeBetweenMovement = 0;

                buttonNo--;
                if (buttonNo < 0)
                    buttonNo = 0;
            }
            else if (Input.GetAxis("Joy1Vertical") >= -ControllerMenuAxisSensitivity)
            {
                timeBetweenMovement = 0;

                buttonNo++;
                if (buttonNo > 1)
                    buttonNo = 1;
            }
        }
    }

    public void LoadScene(string sceneName = "TestScene")
    {
        SceneManager.LoadScene(sceneName);
        StartCoroutine(SpawnObjectManager());
    }

    IEnumerator SpawnObjectManager()
    {
        while (SceneManager.GetActiveScene().name == "TestMenu")
            yield return new WaitForSeconds(Time.deltaTime);

        GetComponent<GameObjectManager>().enabled = true;
    }
}
