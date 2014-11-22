using UnityEngine;
using System.Collections;

public class EnemyController_New: MonoBehaviour {
	GameState game { get { return GameState.Instance; } }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
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
