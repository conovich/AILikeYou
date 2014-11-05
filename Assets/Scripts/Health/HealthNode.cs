using UnityEngine;
using System.Collections;

public class HealthNode : MonoBehaviour {
	public Color onColor;
	public Color offColor;


	// Use this for initialization
	void Start () {
		renderer.material.color = onColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TurnOn(bool isOn){
		if(isOn){
			gameObject.GetComponent<ColorLerper>().StartColorLerp(onColor);
		}
		else{
			gameObject.GetComponent<ColorLerper>().StartColorLerp(offColor);
		}
	}
}
