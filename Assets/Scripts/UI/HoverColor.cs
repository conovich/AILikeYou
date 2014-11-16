using UnityEngine;
using System.Collections;

public class HoverColor : MonoBehaviour {
	
	public Color hoverColor;
	public float lerpSpeed;

	Color originalColor;
	Color targetColor;


	// Use this for initialization
	void Start () {
		originalColor = renderer.material.color;
		targetColor = originalColor;
	}
	
	// Update is called once per frame
	void Update () {
		targetColor.a = 1.0f;
		renderer.material.color = Color.Lerp(renderer.material.color, targetColor, Time.deltaTime*lerpSpeed);
	}

	void OnMouseEnter(){
		//GetComponent<AudioSource>().Play();
		targetColor = hoverColor;
	}

	void OnMouseExit(){
		targetColor = originalColor;
	}
}
