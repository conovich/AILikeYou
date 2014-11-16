using UnityEngine;
using System.Collections;

public class PlayerController_New : MonoBehaviour {
	public bool OVERRIDE_KEYPRESS_CONTROLS;

	public GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }

	public HealthController myHealthController;

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
	public int healthState{ get{ return myHealthController.currentHealth; } }
	public int xPosition = 0;
	public int turretHealth { get { return game.TurretOne.healthState; } }
	public int distanceToTurret { get { return CalculateDistanceToTurret(); } }
	public int distanceToBullet { get { return CalculateDistanceToBullet(); } }
	public int bulletHeight;
	

	Vector3 initPosition;

	float moveIncrement = 1;

	// Use this for initialization
	void Start () {
		initPosition = transform.position;
	}

	public void Reset(){
		transform.position = initPosition;
		myHealthController.Reset();
		hasTaggedTurret = false;
	}

	// Update is called once per frame
	void Update () {
		if(!OVERRIDE_KEYPRESS_CONTROLS && game.currentState == GameState_TurretTag.State.InGame){
			GetAttackDefenseInput();
			GetMovementInput();
		}
	}

	int CalculateDistanceToTurret(){
		int distance = -1;

		float worldDistance = (transform.position - game.TurretOne.transform.position).magnitude;
		distance = (int)(worldDistance / moveIncrement);

		distance--; //for collider range

		return distance;

	}

	int CalculateDistanceToBullet(){
		int distance = -1;

		Bullet[] bullets = GameObject.FindObjectsOfType<Bullet>();
		float worldDistance = -1;

		for(int i = 0; i < bullets.Length; i++){
			float tempDistance = (transform.position - bullets[i].transform.position).magnitude;

			if(tempDistance < worldDistance || worldDistance == -1){
				worldDistance = tempDistance;
			}
		}

		if(worldDistance != -1){
			distance = (int)(worldDistance / moveIncrement);
			distance--; //for collider range
		}

		return distance;
		
	}

	void GetMovementInput(){
		//myMovementControls.GetInput(); 
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			transform.position += Vector3.right*moveIncrement;
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			transform.position += Vector3.left*moveIncrement;
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
		if(collision.collider.tag == "Turret"){
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
		if(!isJumping && !isDucking){ //shouldn't duck and jump at the same time
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
		if(!isDucking && !isJumping){ //shouldn't duck and jump at the same time
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
