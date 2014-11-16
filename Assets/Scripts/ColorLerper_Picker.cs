using UnityEngine;
using System.Collections;

public class ColorLerper_Picker : MonoBehaviour {
	public float lerpSpeed;

	public Color Color1;
	public Color Color2;

	Color targetColor;
	
	// Use this for initialization
	void Start () {
		renderer.material.color = Color1;
	}
	
	// Update is called once per frame
	void Update () {
		LerpColor();
	}

	public void StartLerp1(){
		targetColor = Color1;
	}

	public void StartLerp2(){
		targetColor = Color2;
	}

	void LerpColor(){
		renderer.material.color = Color.Lerp(renderer.material.color, targetColor, Time.deltaTime*lerpSpeed);
	}
}
