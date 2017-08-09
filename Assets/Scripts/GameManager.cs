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

    public string[] ButtonLocations = { "Josh'sScene" };
    public int buttonNo;

    public bool atMenu = true;
    public Image arrow;

    public GameObject MainHUD;
    public GameObject ControlsHUD;

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
        MainHUD.SetActive(true);
        ControlsHUD.SetActive(false);

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
                if (buttonNo == 0)
                {
                    LoadScene("Josh'sScene");
                    atMenu = false;
                }
                if (buttonNo == 1)
                {
                    ToggleControlsHUD();
                }
                if (buttonNo == 2)
                {
                    QuitScene();
                }

                hasShot = true;
            }

            if (arrow != null)
            {
                if (buttonNo == 0)
                {
                    arrow.rectTransform.transform.localPosition = (new Vector3(arrow.transform.localPosition.x, -39.5f, arrow.transform.position.z));
                }
                else if (buttonNo == 1)
                {
                    arrow.rectTransform.transform.localPosition = (new Vector3(arrow.transform.localPosition.x, -100, arrow.transform.position.z));
                }
                else if (buttonNo == 2)
                {
                    arrow.rectTransform.transform.localPosition = (new Vector3(arrow.transform.localPosition.x, -161.1f, arrow.transform.position.z));
                }
            }
        }
        
    }

    void CheckAxis()
    {
        if (MainHUD.active)
        {
            p1Axis = new Vector2(Input.GetAxis("Joy1Horizontal"), Input.GetAxis("Joy1Vertical"));
            p1Trigg = (Input.GetAxis("Joy1Shoot"));
            if (Input.GetAxis("Joy1Vertical") >= 0.90 ||
                Input.GetAxis("Joy1Vertical") <= -0.90)
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
                    if (buttonNo > 2)
                        buttonNo = 2;
                }
            }
        }
    }

    public void LoadScene(string sceneName = "TestScene")
    {
        atMenu = false;
        SceneManager.LoadScene(sceneName);
        StartCoroutine(SpawnObjectManager());
    }

    public void ToggleControlsHUD()
    {
        MainHUD.SetActive(!MainHUD.active);
        ControlsHUD.SetActive(!ControlsHUD.active);
    }

    public void QuitScene()
    {
        Application.Quit();
    }

    IEnumerator SpawnObjectManager()
    {
        while (SceneManager.GetActiveScene().name == "TestMenu")
            yield return new WaitForSeconds(Time.deltaTime);

        GetComponent<GameObjectManager>().enabled = true;
    }
}
