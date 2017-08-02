using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerCount;

	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
    {
	}

    public void LoadScene(string sceneName = "TestScene")
    {
        SceneManager.LoadScene(sceneName);
    }
}
