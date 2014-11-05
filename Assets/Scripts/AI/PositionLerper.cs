using UnityEngine;
using System.Collections;

public class PositionLerper : MonoBehaviour {
	public float lerpSpeed;
	public float epsilon;
	Vector3 target;
	bool shouldLerp = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(shouldLerp){
			LerpPosition();
		}
	}

	void LerpPosition(){
		transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime*lerpSpeed);
		if((transform.position - target).magnitude < epsilon){
			transform.position = target;
			shouldLerp = false;
		}
	}

	public void SetTarget(Vector3 newTarget){
		target = newTarget;
		shouldLerp = true;
	}
}
