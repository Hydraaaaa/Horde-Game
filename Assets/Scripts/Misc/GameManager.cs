using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public int playerCount;

    public float ControllerMenuAxisSensitivity = 0.75f;
    public float ControllerNextInputCheckTimeGap = 0.2f;
    public float timeBetweenMovement;

    public string[] Joysticks;

    public Vector2 p1Axis;
    public float p1Trigg;
    bool hasShot = false;


    public string[] ButtonLocations = { "Josh'sScene" };
    public int buttonNo;

    public bool atMenu = true;
    public Image arrow;

    public GameObject MainHUD;
    public GameObject ControlsHUD;

    [HideInInspector]
    public bool UsingKbrd;

    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    void Start()
    {
        MainHUD.SetActive(true);
        ControlsHUD.SetActive(false);

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SpawnObjectManager;
    }

    void Update()
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

            if (MainHUD.activeInHierarchy)
            {
                // Right Trigger
                if (!Input.GetButton("Joy1AButton"))
                {
                    hasShot = false;
                }

                if (Input.GetButton("Joy1AButton") && !hasShot)
                {
                    if (buttonNo == 0)
                    {
                        LoadScene("FullScene");
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

                if (Input.GetButton("Joy1YButton") && !hasShot)
                {
                    if (buttonNo == 0)
                    {
                        LoadScene("Josh'sScene");
                        atMenu = false;
                    }
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

    }

    void CheckAxis()
    {
        if (MainHUD.activeInHierarchy)
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
        else
        {
            if (Input.GetButton("Joy1BButton"))
            {
                ToggleControlsHUD();
            }
        }
    }

    public void SinglePlayer(string sceneName = "TestScene")
    {
        playerCount = 1;
        UsingKbrd = true;
        atMenu = false;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(string sceneName = "TestScene")
    {
        playerCount = 2;
        UsingKbrd = false;
        atMenu = false;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneKeyboard(string sceneName = "TestScene")
    {
        playerCount = 2;
        UsingKbrd = true;
        atMenu = false;
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleControlsHUD()
    {
        MainHUD.SetActive(!MainHUD.activeInHierarchy);
        ControlsHUD.SetActive(!ControlsHUD.activeInHierarchy);
    }

    public void QuitScene()
    {
        Application.Quit();
    }

    void SpawnObjectManager(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("SpawnObjectManager");
        if (SceneManager.GetActiveScene().name != "TestMenu")
        {
            GetComponent<GameObjectManager>().enabled = false;
            GetComponent<GameObjectManager>().enabled = true;
            GetComponent<ScoreManager>().enabled = false;
            GetComponent<ScoreManager>().enabled = true;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= SpawnObjectManager;
    }
}
