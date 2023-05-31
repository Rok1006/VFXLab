using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A10ImageEffect : MonoBehaviour
{
    public const int WOBB = 0;
	public const int EDGE = 1;
    public const int PAPER = 2;
    public const string PP_WOBB_TEX = "_WobbTex";
	public const string PP_WOBB_TEX_SCALE = "_WobbScale";
	public const string PP_WOBB_POWER = "_WobbPower";
	public const string PP_EDGE_SIZE = "_EdgeSize";
	public const string PP_EDGE_POWER = "_EdgePower";
    public const string PP_PAPER_TEX = "_PaperTex";
    public const string PP_PAPER_SCALE = "_PaperScale";
public const string PP_PAPER_POWER = "_PaperPower";

    public Shader shader;
    Material _material;

    public Texture wobbTex;
	public float wobbScale = 1f;
	public float wobbPower = 0.01f;
	public float edgeSize = 1f;
	public float edgePower = 3f;
    public Texture paperTex;
    public float paperScale = 1f;
	public float paperPower = 1f;

    void OnEnable() {
		_material = new Material(shader);
	}
	void OnDisable() {
		DestroyImmediate(_material);
	}
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
			_material.SetTexture(PP_WOBB_TEX, wobbTex);
			_material.SetFloat(PP_WOBB_TEX_SCALE, wobbScale);
			_material.SetFloat(PP_WOBB_POWER, wobbPower);
			_material.SetFloat(PP_EDGE_SIZE, edgeSize);
			_material.SetFloat(PP_EDGE_POWER, edgePower);
            _material.SetTexture(PP_PAPER_TEX, paperTex);
            _material.SetFloat(PP_PAPER_SCALE, paperScale);
			_material.SetFloat(PP_PAPER_POWER, paperPower);
			
			var rt0 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
			var rt1 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
            
			Graphics.Blit(source, rt1, _material, WOBB);
			Swap(ref rt0, ref rt1);

            //Reflect paper
            Graphics.Blit(rt0, rt1, _material, PAPER);
			Swap(ref rt0, ref rt1);

			Graphics.Blit(rt0, destination, _material, EDGE);
			RenderTexture.ReleaseTemporary(rt0);
			RenderTexture.ReleaseTemporary(rt1);
		}

    void Swap(ref RenderTexture source, ref RenderTexture destination) {
		var tmp = source;
		source = destination;
		destination = tmp;
	}
}
