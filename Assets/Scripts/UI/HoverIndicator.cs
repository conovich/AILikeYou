using UnityEngine;
using System.Collections;

public class HoverIndicator : MonoBehaviour {

	public GameObject indicator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver(){
		indicator.SetActive(true);
	}

	void OnMouseExit(){
		indicator.SetActive(false);
	}
}
