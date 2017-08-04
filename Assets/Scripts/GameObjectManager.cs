﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerPrefab;
    public GameObject HUDPrefab;
    public GameObject WinGUIPrefab;
    public GameObject LoseGUIPrefab;

    [HideInInspector] public GameObject camera;
    [HideInInspector] public GameObject endPos;
    public List<GameObject> playerStarts;
    public List<GameObject> players;
    public List<GameObject> enemySpawners;
    public List<GameObject> enemies;
    public List<GameObject> barricades;
    public List<GameObject> vitalBarricades;
    public List<GameObject> civilians;
    
    [Tooltip("This is a percentage")][Range(0, 100)]
    public float civiliansRequired;

    [HideInInspector] public float timer;
    public int initialCivilians;
    public int civiliansEscaped;
    public int playersEscaped;

    [HideInInspector] public bool playing;

    void Start()
    {
        SceneManager.sceneLoaded += Initialize;
        playing = false;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (players.Count == 0 && playing)
            GameOver();
    }

    public void Initialize(Scene scene, LoadSceneMode mode)
    {
        timer = 10;
        civiliansEscaped = 0;

        playerStarts = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player Start"));
        SpawnCamera();
        SpawnPlayers();
        GetEnemySpawners();
        GetCivilianSpawners();
        GetBarricades();
        GetCivilians();
        SpawnHUD();
        endPos = GameObject.FindGameObjectWithTag("End Position");
        GameObject.FindGameObjectWithTag("End Position").GetComponent<Endpoint>().manager = this;

        StartCoroutine(GetInitialCivilians());

        playing = true;
    }
    IEnumerator GetInitialCivilians()
    {
        yield return new WaitForSeconds(1);
        initialCivilians = civilians.Count;
    }

    public void SpawnCamera()
    {
        camera = Instantiate(cameraPrefab);
        camera.GetComponent<CameraLogic>().gameObjectManager = this;
    }

    public void SpawnPlayers()
    {
        int playerCount = GetComponent<GameManager>().playerCount;

        players = new List<GameObject>();

        CameraLogic cameraScript = camera.GetComponent<CameraLogic>();

        if (playerStarts.Count != 0)
        {
            for (int i = 0; i < playerCount; i++)
            {
                int playerStartIndex = i % playerStarts.Count;

                GameObject playerInstance = Instantiate(playerPrefab,
                                                        playerStarts[playerStartIndex].transform.position,
                                                        playerStarts[playerStartIndex].transform.rotation);

                playerInstance.GetComponent<PlayerMovScript>().playerNumber = i + 1;
                players.Add(playerInstance);
            }
        }
        else
            Debug.Log("No player starts found");

    }

    public void GetEnemySpawners()
    {
        enemySpawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy Spawner"));
        foreach (GameObject enemySpawner in enemySpawners)
        {
            enemySpawner.GetComponent<EnemySpawner>().gameObjectManager = this;
        }
    }

    public void GetCivilianSpawners()
    {
        List<GameObject> civilianSpawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("Civilian Spawner"));
        foreach (GameObject civilianSpawner in civilianSpawners)
        {
            civilianSpawner.GetComponent<CivilianSpawner>().gameObjectManager = this;
        }
    }

    void GetBarricades()
    {
        barricades = new List<GameObject>(GameObject.FindGameObjectsWithTag("Barricade"));
        vitalBarricades = new List<GameObject>();
        foreach (GameObject barricade in barricades)
        {
            if (barricade.GetComponent<BarrierLogic>().vital)
                vitalBarricades.Add(barricade);
        }
    }

    void GetCivilians()
    {
        civilians = new List<GameObject>(GameObject.FindGameObjectsWithTag("Civilian"));
    }

    public void SpawnHUD()
    {
        GameObject HUD = Instantiate(HUDPrefab);
        HUD.GetComponent<HUDScript>().manager = this;
    }

    public bool GetWin()
    {
        if (playersEscaped < 1)
            return false;

        if (civiliansEscaped < initialCivilians * (civiliansRequired / 100))
            return false;

        foreach (GameObject barricade in vitalBarricades)
        {
            if (barricade == null)
                return false;
        }

        return true;
    }

    public void GameOver()
    {
        if (GetWin())
            Instantiate(WinGUIPrefab);
        else
            Instantiate(LoseGUIPrefab);

        playing = false;
    }
}
