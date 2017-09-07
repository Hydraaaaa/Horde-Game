using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShaderHandler : MonoBehaviour
{
    public Color BaseColor;
    public Color OutlineColor;
    public float OutlineWidth;
    public Texture texture;
    public float Speed = 1, Offset;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        //// Get the current value of the material properties in the renderer.
        //_renderer.GetPropertyBlock(_propBlock);

        //// Assign our new value.
        //_propBlock.SetColor("_Color", BaseColor);
        //_propBlock.SetColor("_OutlineColor", OutlineColor);
        //_propBlock.SetFloat("_Outline", OutlineWidth);
        //_propBlock.SetTexture("_MainTex", texture);

        //// Apply the edited values to the renderer.
        //_renderer.SetPropertyBlock(_propBlock);
    }
}
