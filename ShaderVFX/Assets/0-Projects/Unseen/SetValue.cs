using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetValue : MonoBehaviour
{
    public Renderer TheRenderer;
    public Image img;
    Material _theMaterial;
    public float dissolveValue;
    
    void Start() {
        _theMaterial = img.material;
    }
    private void Update() {
        SetShaderValue();
    }
    
    private void SetShaderValue() {
        _theMaterial.SetFloat("_DissolveAreaScale", dissolveValue);
        //_theMaterial.SetColor("_SomeColorParam", Color.red);
    }
}
