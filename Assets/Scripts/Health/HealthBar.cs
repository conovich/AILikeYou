using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour {
	public HealthController myHealthController;
	public float separationDistance;
	public HealthNode healthBarNode;

	List<HealthNode> healthNodes;
	int currentHealthIndex = 0;

	// Use this for initialization
	void Start () {
		SpawnHealthBar();
		myHealthController.RemoveHealthDelegate += DecrementHealth;
		myHealthController.AddHealthDelegate += IncrementHealth;
	}

	void SpawnHealthBar(){
		healthNodes = new List<HealthNode>();

		for(int i = 0; i < myHealthController.maxHealth; i++){
			HealthNode newHealthNode = Instantiate(healthBarNode, transform.position + separationDistance*i*Vector3.right, transform.rotation) as HealthNode;
			healthNodes.Add(newHealthNode);
			newHealthNode.transform.parent = gameObject.transform;
		}

		currentHealthIndex = myHealthController.maxHealth - 1;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void DecrementHealth(){
		if(currentHealthIndex >= 0 && currentHealthIndex < myHealthController.maxHealth){
			healthNodes[currentHealthIndex].TurnOn(false);
			currentHealthIndex--;
			if(currentHealthIndex < 0){
				currentHealthIndex = 0;
			}
		}
	}
	
	public void IncrementHealth(){
		currentHealthIndex++;
		healthNodes[currentHealthIndex].TurnOn(true);
	}
}
