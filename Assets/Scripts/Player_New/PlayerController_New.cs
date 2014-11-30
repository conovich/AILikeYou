using UnityEngine;
using System.Collections;

public class PlayerController_New : MonoBehaviour {
	public bool OVERRIDE_KEYPRESS_CONTROLS;

	public GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }

	public HealthController myHealthController;


	public delegate void ReboundBullet();
	public ReboundBullet ReboundBulletDelegate;
	public delegate void MoveForward();
	public ReboundBullet MoveForwardDelegate;
	public delegate void MoveBackward();
	public ReboundBullet MoveBackwardDelegate;


	public Shield shield;
	public float jumpHeight;
	public float duckScale;
	public Transform duckingTransform;

	//INTERNAL STATES
	public bool isJumping = false;
	public bool isDucking = false;
	public bool shieldOn = false;
	public bool hasTaggedTurret = false;
	public bool isMovingForward = false;
	public bool isMovingBackward = false;

	//STATE VARIABLES
	public int healthState{ get{ return myHealthController.currentHealth; } }
	public int xPosition = 0;
	public int turretHealth { get { return game.TurretOne.healthState; } }
	public int distanceToTurret { get { return CalculateDistanceToTurret(); } }
	public int distanceToBullet { get { return CalculateDistanceToBullet(); } }
	public int bulletHeight { get { return CalculateBulletHeight(); } }


	//COST VARIABLES
	int jumpCost = 1;
	int duckCost = 1;
	int moveCost = 1;
	int shieldCost = 1;
	public int hitCost = 1;

	int costIncrement = 1;

	Vector3 initPosition;

	float moveIncrement = 1;
	int maxDistance;

	// Use this for initialization
	void Start () {
		initPosition = transform.position;
		maxDistance = distanceToTurret;
	}

	public void Reset(){
		transform.position = initPosition;
		myHealthController.Reset();
		hasTaggedTurret = false;
		shieldCost = 1;
		hitCost = 1;
	}

	// Update is called once per frame
	void Update () {
		if(!OVERRIDE_KEYPRESS_CONTROLS && game.currentState == GameState_TurretTag.State.InGame){
			GetAttackDefenseInput();
			GetMovementInput();
		}
	}

	void DoShadowCost(int cost){
		for(int i = 0; i < cost; i++){
			myHealthController.RemoveHealthDelegate();
		}
	}

	void AddToShadowCosts_Distance(){
		//Debug.Log("dist to turret" + distanceToTurret);
		if(distanceToTurret > 1 && distanceToTurret < maxDistance){
			hitCost += costIncrement;
			shieldCost += costIncrement;
			//Debug.Log("adding cost");
			//Debug.Log("hitCost" + hitCost);
			//Debug.Log("shieldCost" + shieldCost);
		}
	}

	void SubFromShadowCosts_Distance(){
		//Debug.Log("dist to turret" + distanceToTurret);
		if(distanceToTurret > 1 && distanceToTurret <= maxDistance){
			hitCost -= costIncrement;
			shieldCost -= costIncrement;
			//Debug.Log("removing cost");
			//Debug.Log("hitCost" + hitCost);
			//Debug.Log("shieldCost" + shieldCost);
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

	int CalculateBulletHeight(){
		Bullet[] bullets = GameObject.FindObjectsOfType<Bullet>();

		//find closest bullet
		if(bullets.Length  > 0){
			Bullet closestBullet = bullets[0];
			float closeDistance = (transform.position - bullets[0].transform.position).magnitude;
			for(int i = 1; i < bullets.Length; i++){
				float tempDistance = (transform.position - bullets[i].transform.position).magnitude;
				
				if(tempDistance < closeDistance){
					closeDistance = tempDistance;
					closestBullet = bullets[i];
				}
			}
			return closestBullet.GetHeight();
		}
		else{
			return 0;
		}
	}

	void GetMovementInput(){
		//myMovementControls.GetInput(); 
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			MoveRight();
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			MoveLeft();
		}
	}

	public void MoveRight(){
		StartCoroutine(MoveForwardCoroutine());
	}

	IEnumerator MoveForwardCoroutine(){

		isMovingForward = true;
		MoveForwardDelegate(); //isMovingForward MUST BE TRUE BEFORE CALLING THIS
		transform.position += Vector3.right*moveIncrement;
		DoShadowCost(moveCost);
		AddToShadowCosts_Distance();
		yield return 0;
		isMovingForward = false;
	}

	public void MoveLeft(){
		StartCoroutine(MoveBackwardCoroutine());
	}
	
	IEnumerator MoveBackwardCoroutine(){
		isMovingBackward = true;
		MoveBackwardDelegate(); //isMovingBackward MUST BE TRUE BEFORE CALLING THIS
		transform.position += Vector3.left*moveIncrement;
		DoShadowCost(moveCost);
		SubFromShadowCosts_Distance();
		yield return 0;
		isMovingBackward = false;
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
		if((collision.collider.tag == "Bullet" || collision.collider.tag == "BulletRebound") && !shieldOn){
			DoShadowCost(hitCost);
			//GetComponent<HealthController>().RemoveHealthDelegate();
		}
		if(collision.collider.tag == "Turret"){
			hasTaggedTurret = true;
		}
	}


	public void PlayerShieldOn(){
		if(!shieldOn){
			DoShadowCost(shieldCost);
			shieldOn = true;
			if(!OVERRIDE_KEYPRESS_CONTROLS){
				shield.Activate();
			}
			else{
				shield.ActivateForTime(this);
			}
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
			DoShadowCost(jumpCost);
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
			DoShadowCost(duckCost);
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
