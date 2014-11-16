using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public GameObject desiredObject;
	public string function;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		desiredObject.SendMessage(function);
	}
}
