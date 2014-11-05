using UnityEngine;
using System.Collections;

public class MovementControls : MonoBehaviour {
	
	public Transform rightBounds;
	public Transform leftBounds;
	public Transform topBounds;
	public Transform bottomBounds;
	
	public float timeToStop;
	public float maxVelocity;
	public float incrementSpeed;
	public float turnAngleIncrement;

	public bool onlyLeftRightMovement;

	public Transform duckingTransform;

	public bool isJumping = false;
	public bool isDucking = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//GetInput();
	}

	//move to playercontroller?
	public void GetInput(){
		bool isInput = false;

		if(onlyLeftRightMovement){
			if(Input.GetKey(KeyCode.RightArrow)){
				IncrementVelocity(new Vector3(transform.forward.x, 0.0f, transform.forward.z)*incrementSpeed);
				isInput = true;
			}
			else if(Input.GetKey(KeyCode.LeftArrow)){
				IncrementVelocity(new Vector3(-transform.forward.x, 0.0f, -transform.forward.z)*incrementSpeed);
				isInput = true;
			}
			/*

			if(Input.GetKey(KeyCode.UpArrow)){
				if(!isJumping){
					isJumping = true;
					StartCoroutine(Jump ());
				}
			}
			
			else if(Input.GetKeyDown(KeyCode.DownArrow)){ //down arrow conditionally used in movement controls -- be wary of this!
				if(!isDucking){
					StartCoroutine(Duck ());
				}
			}*/
		}
		else{
			if(Input.GetKey(KeyCode.RightArrow)){
				RotateSelf(turnAngleIncrement);
				isInput = true;
			}
			else if(Input.GetKey(KeyCode.LeftArrow)){
				RotateSelf(-turnAngleIncrement);
				isInput = true;
			}
			if(Input.GetKey(KeyCode.UpArrow)){
				IncrementVelocity(new Vector3(transform.forward.x, 0.0f, transform.forward.z)*incrementSpeed);
				isInput = true;
			}
			else if(Input.GetKey(KeyCode.DownArrow)){
				IncrementVelocity(new Vector3(-transform.forward.x, 0.0f, -transform.forward.z)*incrementSpeed);
				isInput = true;
			}
		}
		if(!isInput){
			SlowToStop();
		}

		CheckForOutOfBounds();

	}

	IEnumerator Jump(){
		isJumping = true;
		float initYPos = transform.position.y;
		float jumpIncrement = 0.015f;
		float jumpAmount = 0.3f;

		while(jumpAmount > 0.05){
			transform.position += Vector3.up*jumpAmount;
			jumpAmount -= jumpIncrement;
			yield return 0;
		}
		while(jumpAmount <= 0.3f){
			transform.position -= Vector3.up*jumpAmount;
			jumpAmount += jumpIncrement;

			if(transform.position.y < initYPos){
				transform.position = new Vector3(transform.position.x, initYPos, transform.position.z);
			}

			yield return 0;
		}

		isJumping = false;
		yield return 0;
	}

	IEnumerator Duck(){
		isDucking = true;
		float squashMax = 0.1f;
		float amountSquashed = 0;
		float squashIncrement = 0.01f;
		
		Vector3 originalScale = duckingTransform .localScale;
		
		while(amountSquashed < squashMax){
			amountSquashed += squashIncrement;
			duckingTransform.localScale -= Vector3.up*amountSquashed;
			yield return 0;
		}

		while(Input.GetKey(KeyCode.DownArrow)){
			yield return 0;
		}

		while(amountSquashed > 0){
			amountSquashed -= squashIncrement;
			duckingTransform.localScale += Vector3.up*amountSquashed;
			yield return 0;
		}
		
		duckingTransform.localScale = originalScale;
	}
	
	void IncrementVelocity(Vector3 additionalVel){
		rigidbody.velocity += additionalVel;
		CapVelocity();
	}

	void RotateSelf(float degrees){
		gameObject.transform.RotateAround(transform.position, Vector3.up, degrees);
	}
	
	void SlowToStop(){
		//rigidbody.velocity = Vector3.zero;


		float epsilon = 0.1f;
		if(rigidbody.velocity.x > epsilon || rigidbody.velocity.x < -epsilon){
			float slowingSpeed = rigidbody.velocity.x/timeToStop;
			if(rigidbody.velocity.x > 0){
				IncrementVelocity(-Vector3.right*slowingSpeed);
			}
			else{
				IncrementVelocity(-Vector3.right*slowingSpeed);
			}
		}
		else{
			rigidbody.velocity = new Vector3(0.0f, 0.0f, rigidbody.velocity.z);
		}
		
		if(rigidbody.velocity.z > epsilon || rigidbody.velocity.z < -epsilon){
			float slowingSpeed = rigidbody.velocity.z/timeToStop;
			if(rigidbody.velocity.z > 0){
				IncrementVelocity(-Vector3.forward*slowingSpeed);
			}
			else{
				IncrementVelocity(-Vector3.forward*slowingSpeed);
			}
		}
		else{
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0.0f, 0.0f);
		}

	}
	
	void CapVelocity(){
		if(rigidbody.velocity.x > maxVelocity){
			rigidbody.velocity = new Vector3(maxVelocity, rigidbody.velocity.y, rigidbody.velocity.z);
		}
		else if(rigidbody.velocity.x < -maxVelocity){
			rigidbody.velocity = new Vector3(-maxVelocity, rigidbody.velocity.y, rigidbody.velocity.z);
		}
		
		if(rigidbody.velocity.z > maxVelocity){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, maxVelocity);
		}
		else if(rigidbody.velocity.z < -maxVelocity){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, -maxVelocity);
		}
	}
	
	void CheckForOutOfBounds(){
		//x bounds
		if(transform.position.x > rightBounds.position.x){
			if(rigidbody.velocity.x > 0){
				rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, rigidbody.velocity.z);
			}
		}
		else if(transform.position.x < leftBounds.position.x){
			if(rigidbody.velocity.x < 0){
				rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, rigidbody.velocity.z);
			}
		}
		
		//y bounds
		if(transform.position.z > topBounds.position.z){
			if(rigidbody.velocity.z > 0){
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
			}
		}
		else if(transform.position.z < bottomBounds.position.z){
			if(rigidbody.velocity.z < 0){
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
			}
		}
	}
	
}
