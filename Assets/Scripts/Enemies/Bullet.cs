using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }

	public float lifeTime;
	
	public int reboundShadowCost = 0;

	Transform midHeightTransform { get { return game.TurretOne.MidTransform; } }

	public enum Height{
		none, //0
		low, //1
		high //2
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("bullet pos" + transform.position.y + "mid height" + midHeightTransform.position.y);
	}


	public void Fire(Vector3 firingVelocity){
		rigidbody.velocity = firingVelocity;
		StartCoroutine(Life ());
	}

	public int GetHeight(){ //MUST BE FROM 0-2
		if(transform.position.y > midHeightTransform.position.y){
			return (int)(Height.high);
		}
		else{
			return (int)(Height.low);
		}
	}

	void OnCollisionEnter(Collision collision){
		//TODO: Die() when we hit anything other than the turret this came from
		if(collision.gameObject.tag == "Player"){
			PlayerController_New playerController = collision.gameObject.GetComponent<PlayerController_New>();
			if( playerController.shieldOn ){
				Rebound ();
			}
			else{
				Die ();
			}
		}
	}

	void Rebound(){
		rigidbody.velocity *= -1;
		tag = "BulletRebound";
		reboundShadowCost = game.PlayerOne.hitCost;
		game.PlayerOne.ReboundBulletDelegate();
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
