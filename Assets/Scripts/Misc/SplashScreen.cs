using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public float fadeInTime;
    public float waitTime;
    public float fadeOutTime;
    public bool skippable;

    float currentTime;

    Image splashImage;

	void Start ()
    {
        splashImage = GetComponent<Image>();
	}
    
	void Update ()
    {
        currentTime += Time.deltaTime;
        if (currentTime < fadeInTime)
        {
            float alpha = currentTime / fadeInTime;
            splashImage.color = new Color(alpha, alpha, alpha);
        }
        else if (currentTime < fadeInTime + waitTime)
        {
            splashImage.color = new Color(1, 1, 1);
        }
        else if (currentTime < fadeInTime + waitTime + fadeOutTime)
        {
            float alpha = 1 - (currentTime - fadeInTime - waitTime) / fadeOutTime;
            splashImage.color = new Color(alpha, alpha, alpha);
        }
        else
        {
            SceneManager.LoadScene("TestMenu");
        }

        if (Input.anyKeyDown && skippable)
            SceneManager.LoadScene("TestMenu");
    }
}
