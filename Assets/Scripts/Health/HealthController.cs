using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
	public int maxHealth;

	public delegate void RemoveHealth();
	public RemoveHealth RemoveHealthDelegate;
	public delegate void AddHealth();
	public AddHealth AddHealthDelegate;

	int health;

	// Use this for initialization
	void Start () {
		RemoveHealthDelegate += DecrementHealth;
		AddHealthDelegate += IncrementHealth;
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DecrementHealth(){

		health --;

		if(health < 0){
			health = 0;
		}
	}

	void IncrementHealth(){

		health ++;

		if(health > maxHealth){
			health = maxHealth;
		}
	}
	
}
