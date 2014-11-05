using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public bool OVERRIDE_KEYPRESS_CONTROLS;


	public AttackDefenseControls myAttackDefenseControls;
	public MovementControls myMovementControls;
	
	public bool IsPunchingLeft = false;
	public bool IsPunchingRight = false;
	public bool IsKickingLeft = false;
	public bool IsKickingRight = false;

	public bool IsJumping;
	public bool IsDucking;

	public bool HasHitEnemy = false;
	public bool IsHit = false;

	// Use this for initialization
	void Start () {
		//myAttackDefenseControls = GetComponent<AttackDefenseControls>();
		//Debug.Log("my AD controlssss" + myAttackDefenseControls);
		//myMovementControls = GetComponent<MovementControls>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!OVERRIDE_KEYPRESS_CONTROLS){
			GetAttackDefenseInput();
			GetMovementInput();
		}
		CheckMovesOver();
	}

	void GetAttackDefenseInput(){
		if(Input.GetKeyDown(KeyCode.S)){
			PlayerPunchRight();
		}
		if(Input.GetKeyDown(KeyCode.A)){
			PlayerPunchLeft();
		}
		if(Input.GetKeyDown(KeyCode.Z)){
			PlayerKickRight();
		}
		if(Input.GetKeyDown(KeyCode.X)){
			PlayerKickLeft();
		}
		if(Input.GetKey(KeyCode.Space)){
			PlayerShieldOn();
		}
		else{
			PlayerShieldOff();
		}

		if(Input.GetKey(KeyCode.UpArrow)){
			PlayerJump();
		}
		
		else if(Input.GetKeyDown(KeyCode.DownArrow)){ //down arrow conditionally used in movement controls -- be wary of this!
			PlayerDuck();
		}
	}

	public void PlayerPunchRight(){
		myAttackDefenseControls.PunchRight();
		IsPunchingRight = true;
	}

	public void PlayerPunchLeft(){
		myAttackDefenseControls.PunchLeft();
		IsPunchingLeft = true;
	}

	public void PlayerKickRight(){
		myAttackDefenseControls.KickRight();
		IsKickingRight = true;
	}

	public void PlayerKickLeft(){
		myAttackDefenseControls.KickLeft();
		IsKickingLeft = true;
	}

	public void PlayerShieldOn(){
		if(!myAttackDefenseControls.shieldOn){
			myAttackDefenseControls.ActivateShield();
		}
	}

	public void PlayerShieldOff(){
		if(myAttackDefenseControls.shieldOn){
			myAttackDefenseControls.DeactivateShield();
		}
	}

	public void PlayerJump(){
		if(!IsJumping){
			IsJumping = true;
			myMovementControls.StartCoroutine("Jump");
		}
	}

	public void PlayerDuck(){
		if(!IsDucking){
			myMovementControls.StartCoroutine("Duck");
		}
	}

	void CheckMovesOver(){
		if(!myAttackDefenseControls.CheckPunchingKicking(myAttackDefenseControls.rightHand)){
			IsPunchingRight = false;
		}
		if(!myAttackDefenseControls.CheckPunchingKicking(myAttackDefenseControls.leftHand)){
			IsPunchingLeft = false;
		}
		if(!myAttackDefenseControls.CheckPunchingKicking(myAttackDefenseControls.rightLeg)){
			IsKickingRight = false;
		}
		if(!myAttackDefenseControls.CheckPunchingKicking(myAttackDefenseControls.leftLeg)){
			IsKickingLeft = false;
		}
		if(!myMovementControls.isJumping){
			IsJumping = false;
		}
		if(!myMovementControls.isDucking){
			IsDucking = false;
		}

	}

	void GetMovementInput(){
		myMovementControls.GetInput(); 
	}

	void OnCollisionEnter(Collision collision){
		if(collision.collider.tag == "Bullet" && !myAttackDefenseControls.shieldOn){
			GetComponent<HealthController>().RemoveHealthDelegate();
			IsHit = true;
		}
		collision.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
	}
	

	void HitEnemy(){
		HasHitEnemy = true;
	}
}
