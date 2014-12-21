using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public HealthController myHealthController;
	public int healthState { get { return myHealthController.currentHealth; } }
	public Transform MidTransform;
	public Transform MovingTransform;

	GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }

	//semi-copy/pasted for use in bullet -- should refactor to a height calculator class or something!
	public enum Height{
		//none, //0
		low, //0
		high //1
	}


	// Use this for initialization
	void Start () {
		
	}

	public void Reset(){
		myHealthController.Reset();
	}
	
	// Update is called once per frame
	void Update () {

	}


	public int GetHeight(){ //MUST BE FROM 0-2
		if(MovingTransform.position.y > MidTransform.position.y){
			return (int)(Height.high);
		}
		else{
			return (int)(Height.low);
		}
	}

	public float GetTimeSinceLastFire(){
		return GetComponentInChildren<TurretFire>().timeSinceFired;
	}

	/*void OnTriggerEnter(Collider collider){
		bool isHit = false;
		//if(game.PlayerOne.IsPunchingLeft && collider.tag == "PuncherLeft"){
		//	isHit = true;
		//}
		
		
		if(isHit){
			GetComponent<HealthController>().RemoveHealthDelegate();
			//game.PlayerOne.SendMessage("HitEnemy");
		}
	}*/

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "BulletRebound"){
			Bullet bullet = collision.gameObject.GetComponent<Bullet>();
			for(int i = 0; i < bullet.reboundShadowCost; i++){
				GetComponent<HealthController>().RemoveHealthDelegate();
			}
			bullet.SendMessage("Die");
		}
		
		/*bool isHit = false;
		if(game.PlayerOne.IsPunchingLeft && collider.tag == "PuncherLeft"){
			isHit = true;
		}
		if(game.PlayerOne.IsPunchingRight && collider.tag == "PuncherRight"){
			isHit = true;
		}
		if(game.PlayerOne.IsKickingLeft && collider.tag == "KickerLeft"){
			isHit = true;
		}
		if(game.PlayerOne.IsKickingRight && collider.tag == "KickerRight"){
			isHit = true;
		}


		if(isHit){
			GetComponent<HealthController>().RemoveHealthDelegate();
			game.PlayerOne.SendMessage("HitEnemy");
		}*/
	}
}
