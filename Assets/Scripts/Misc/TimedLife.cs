using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLife : MonoBehaviour
{
    public float startingLife;
    [HideInInspector] public float life;

    public bool fade;
    Renderer renderer;
    
	void Start ()
    {
        life = startingLife;
        renderer = GetComponent<Renderer>();
	}
	
	void Update ()
    {
        life -= Time.deltaTime;

        if (life <= 0)
            Destroy(gameObject);

        if (fade)
        {
            Color color = renderer.material.color;
            renderer.material.color = new Color(color.r, color.g, color.b, life / startingLife);
        }
	}
}
