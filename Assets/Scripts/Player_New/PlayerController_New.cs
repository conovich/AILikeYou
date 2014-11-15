using UnityEngine;
using System.Collections;

public class PlayerController_New : MonoBehaviour {
	public bool OVERRIDE_KEYPRESS_CONTROLS;

	public Shield shield;
	public float jumpHeight;
	public float duckScale;
	public Transform duckingTransform;

	//INTERNAL STATES
	public bool isJumping;
	public bool isDucking;
	public bool shieldOn;
	public bool hasTaggedTurret;

	//STATE VARIABLES
	public int healthState;
	public int xPosition;
	public int turretHealth;
	public int distanceToTurret;
	public int distanceToBullet;
	public int bulletHeight;
	

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!OVERRIDE_KEYPRESS_CONTROLS){
			GetAttackDefenseInput();
			GetMovementInput();
		}
		//CheckMovesOver();
	}

	void GetMovementInput(){
		//myMovementControls.GetInput(); 
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			transform.position += Vector3.right;
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			transform.position += Vector3.left;
		}
	}

	void GetAttackDefenseInput(){
		if(Input.GetKey(KeyCode.Space)){
			PlayerShieldOn();
		}
		else{
			PlayerShieldOff();
		}

		if(Input.GetKeyDown(KeyCode.UpArrow)){
			PlayerJump();
		}
		
		else if(Input.GetKeyDown(KeyCode.DownArrow)){ //down arrow conditionally used in movement controls -- be wary of this!
			PlayerDuck();
		}
	}

	void OnCollisionEnter(Collision collision){
		if(collision.collider.tag == "Bullet" && !shieldOn){
			GetComponent<HealthController>().RemoveHealthDelegate();
			//IsHit = true;
		}
		else if(collision.collider.tag == "Turret"){
			hasTaggedTurret = true;
		}
		collision.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
	}


	public void PlayerShieldOn(){
		if(!shieldOn){
			shieldOn = true;
			shield.Activate();
		}
	}

	public void PlayerShieldOff(){
		if(shieldOn){
			shieldOn = false;
			shield.Deactivate();
		}
	}

	public void PlayerJump(){
		if(!isJumping){
			StartCoroutine(Jump ());
		}
	}

	IEnumerator Jump(){
		isJumping = true;
		
		transform.position += Vector3.up*jumpHeight;
		yield return new WaitForSeconds(.5f);
		transform.position -= Vector3.up*jumpHeight;
		
		isJumping = false;

	}

	public void PlayerDuck(){
		if(!isDucking){
			StartCoroutine(Duck());
		}
	}

	IEnumerator Duck(){
		isDucking = true;
		float currentYScale = duckingTransform.localScale.y;
		duckingTransform.localScale = new Vector3(duckingTransform.localScale.x, duckScale, duckingTransform.localScale.z);
		yield return new WaitForSeconds(.5f);

		duckingTransform.localScale = new Vector3(duckingTransform.localScale.x, currentYScale, duckingTransform.localScale.z);
		isDucking = false;

	}


	void HitEnemy(){
		//HasHitEnemy = true;
	}
}
