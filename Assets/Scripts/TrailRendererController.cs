using UnityEngine;
using System.Collections;

public class TrailRendererController : MonoBehaviour {
	public bool enableOnStart = false;

	// Use this for initialization
	void Start () {
		ToggleTrailRenderer(enableOnStart);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleTrailRenderer(bool onOrOff){
		gameObject.GetComponent<TrailRenderer>().enabled = onOrOff;
	}
}
