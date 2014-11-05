using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public float maxVelocity;
	public float maxForce;
	public float maxSpeed;
	public float mass;
	public GameObject target;
	Vector3 velocity;
	Vector3 desired_velocity;
	Vector3 steering;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if((target.transform.position - transform.position).magnitude > 2*velocity.magnitude){ //2* to make it stop before hitting player... TODO: put an OnCollision... 
			CalculateVelocity();
		}
	}

	void CalculateVelocity(){
		velocity = (target.transform.position - transform.position).normalized * maxVelocity;
		//desired_velocity = (target.transform.position - transform.position).normalized * maxVelocity;

		//steering = desired_velocity - velocity;

		//steering = steering.normalized*maxForce;
		//steering = steering / mass;
				
		//velocity = (velocity + steering).normalized*maxSpeed;
		transform.position = transform.position + velocity;
	}
}
