using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float lifeTime;

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
