using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererTimedLife : MonoBehaviour
{
    public float startingLife;
    [HideInInspector]
    public float life;

    public bool fade;
    LineRenderer renderer;

    void Start()
    {
        life = startingLife;
        renderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        life -= Time.deltaTime;

        if (life <= 0)
            Destroy(gameObject);

        if (fade)
        {
            renderer.startColor = new Color(renderer.startColor.r, renderer.startColor.g, renderer.startColor.b, life / startingLife);
        }
    }
}
