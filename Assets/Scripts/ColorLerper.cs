using UnityEngine;
using System.Collections;

public class ColorLerper : MonoBehaviour {
	public float lerpSpeed;

	Color targetColor;
	
	// Use this for initialization
	void Start () {
		targetColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		LerpColor();
	}

	public void StartColorLerp(Color newTargetColor){
		targetColor = newTargetColor;
	}

	void LerpColor(){
		renderer.material.color = Color.Lerp(renderer.material.color, targetColor, Time.deltaTime*lerpSpeed);
	}
}
