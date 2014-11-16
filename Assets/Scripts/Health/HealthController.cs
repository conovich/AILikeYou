using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
	public int maxHealth;

	public delegate void RemoveHealth();
	public RemoveHealth RemoveHealthDelegate;
	public delegate void AddHealth();
	public AddHealth AddHealthDelegate;

	public int currentHealth;

	// Use this for initialization
	void Start () {
		RemoveHealthDelegate += DecrementHealth;
		AddHealthDelegate += IncrementHealth;
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DecrementHealth(){
		currentHealth --;

		if(currentHealth < 0){
			currentHealth = 0;
		}
	}

	void IncrementHealth(){

		currentHealth ++;

		if(currentHealth > maxHealth){
			currentHealth = maxHealth;
		}
	}

	public void Reset(){
		while (currentHealth < maxHealth){
			AddHealthDelegate();
		}
	}
	
}
