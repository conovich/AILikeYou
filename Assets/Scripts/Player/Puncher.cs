using UnityEngine;
using System.Collections;

public class Puncher : MonoBehaviour {

	public float punchDistance;
	public GameObject parent;
	public int speed = 4;
	
	public bool shouldPunch = false;
	public bool shouldReturn = false;
	Vector3 startingPosDifference; //difference from parent object to this object -- punch should return to position relative to parent
	public Transform inTransform;
	public Transform outTransform;
	Vector3 targetPos;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(shouldPunch){
			gameObject.GetComponent<TrailRendererController>().ToggleTrailRenderer(true);
			PunchOut();

		}
		else if(shouldReturn){
			gameObject.GetComponent<TrailRendererController>().ToggleTrailRenderer(false);

			Return ();
		}
	}

	public void Punch(){
		if(!shouldPunch){
			shouldPunch = true;
		}
	}

	void PunchOut(){;

		targetPos = outTransform.position;

		transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime*speed*speed);
		 

		if((transform.position - targetPos).magnitude < 0.03f){
			shouldPunch = false;
			shouldReturn = true;
		}
	}

	void Return(){
	
		targetPos = inTransform.position;
		transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime*speed);
		//transform.RotateAround(parent.transform.position, Vector3.up, parent.transform.rotation.y);
		
		if((transform.position - targetPos).magnitude < 0.03f){
			shouldPunch = false;
			shouldReturn = false;
			transform.position = targetPos;
		}

	}

}