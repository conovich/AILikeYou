using UnityEngine;
using System.Collections;

public class AttackDefenseControls : MonoBehaviour {

	public GameObject rightHand;
	public GameObject leftHand;
	public GameObject rightLeg;
	public GameObject leftLeg;

	public Shield shield;
	public bool shieldOn;


	bool isTurning = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public bool CheckPunchingKicking(GameObject limb){
		if(!limb.GetComponent<Puncher>().shouldPunch && !limb.GetComponent<Puncher>().shouldReturn){
			return false;
		}
		else{
			return true;
		}
	}

	public void PunchRight(){
		rightHand.GetComponent<Puncher>().Punch();
	}

	public void PunchLeft(){
		leftHand.GetComponent<Puncher>().Punch();
	}

	public void KickRight(){
		if(!isTurning){
			StartCoroutine(Turn(360));
			rightLeg.GetComponent<Puncher>().Punch();
		}
	}

	public void KickLeft(){
		if(!isTurning){
			StartCoroutine(Turn(-360));
			leftLeg.GetComponent<Puncher>().Punch();
		}
	}

	IEnumerator Turn(int degrees){
		int direction = 1;
		if(degrees < 0){
			direction = -1;
		}

		degrees = Mathf.Abs(degrees);
		int degreesPerFrame = 1;

		int count = 0;
		while(count < degrees){
			transform.RotateAround(transform.position, Vector3.up, direction*degreesPerFrame);

			count+= degreesPerFrame;

			if(count < 180){
				degreesPerFrame += 1;
			}
			else{
				degreesPerFrame -= 1;
			}

			yield return 0;
		}
		isTurning = false;
	}

	public bool CheckShieldOn(){
		return shieldOn;
	}

	public void ActivateShield(){
		shieldOn = true;
		shield.Activate();
	}

	public void DeactivateShield(){
		shieldOn = false;
		shield.Deactivate();
	}



}
