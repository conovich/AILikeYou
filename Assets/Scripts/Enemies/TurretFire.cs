﻿using UnityEngine;
using System.Collections;

public class TurretFire : MonoBehaviour {
	public GameObject bullet;
	public float upperTimeBound;
	public float lowerTimeBound;
	public float firingSpeed;
	public Transform bulletSpawnTransform;

	public float forwardOffset;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*IEnumerator SpawnBullets(){
		while(true){
			yield return new WaitForSeconds(Random.Range(lowerTimeBound, upperTimeBound));
			SpawnBullet ();
		}
	}*/

	void FireBullet(){
			GameObject newBullet = Instantiate(bullet, bulletSpawnTransform.position + bulletSpawnTransform.forward*forwardOffset, transform.rotation) as GameObject;
			
			Vector3 firingDirection = bulletSpawnTransform.forward;
			
			newBullet.GetComponent<Bullet>().Fire(firingDirection*firingSpeed);
	}
}
