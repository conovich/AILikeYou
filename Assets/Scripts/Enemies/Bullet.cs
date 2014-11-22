﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float lifeTime;
	
	public int reboundShadowCost = 0;

	GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

	}


	public void Fire(Vector3 firingVelocity){
		rigidbody.velocity = firingVelocity;
		StartCoroutine(Life ());
	}

	void OnCollisionEnter(Collision collision){
		//TODO: Die() when we hit anything other than the turret this came from
		if(collision.gameObject.tag == "Player"){
			PlayerController_New playerController = collision.gameObject.GetComponent<PlayerController_New>();
			if( playerController.shieldOn ){
				rigidbody.velocity *= -1;
				tag = "BulletRebound";
				reboundShadowCost = game.PlayerOne.hitCost;
			}
			else{
				Die ();
			}
		}
	}

	IEnumerator Life(){
		yield return new WaitForSeconds(lifeTime);

		Die();
	}

	void Die(){
		//TODO: should explode!
		Destroy(gameObject);
	}
}
