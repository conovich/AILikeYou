using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAction_New : MonoBehaviour {
	GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }
	PlayerController_New player { get { return GameState_TurretTag.Instance.PlayerOne; } }

	public AIController_New myAIController;
	

	//EACH MOVE HAS ONE OF THESE ARRAYS -- IN THAT INDEX IS WHERE THE PROBABILITY IS STORED FOR THAT SPECIFIC STATE.
	//then, we can access the probability of each action at a particular state, and pick the best action.

	/* ORDER IN PROBABILITY ARRAY
	int playerHealthIndex = 0; //0-10
	int turretHealthIndex = 1; //0-10
	int turretDistanceIndex = 2; //0-10 
	int bulletDistanceIndex = 3; //0-10
	int bulletHeightIndex = 4; //0-2
	*/
	public float[ , , , , ] qValArray;

	//OLD
//	float alphaWeight = 0.5f;


	// Use this for initialization
	void Start () {

	}


	public void Init(){ //call this from the ai action controller
		qValArray = new float[11,11,11,11,3];
		if(myAIController){
			if(myAIController.ShouldCreateNewActions){
				InitQValArray();
			}
		}
	}

	void InitQValArray(){
		for(int j = 0; j < 11; j++){
			for(int k = 0; k < 11; k++){
				for(int l = 0; l < 11; l++){
					for(int m = 0; m < 11; m++){
						for(int n = 0; n < 3; n++){
							qValArray[j,k,l,m,n] = myAIController.initActionValue;
						}
					}
				}
			}
		}
	}


	// Update is called once per frame
	void Update () {

	}

	public float GetCurrentProbability(){
		int[] state = game.myAIStateController.stateArray;

		float q_probability = qValArray[state[0], state[1], state[2], state[3], state[4]];

		return q_probability;
	}



	/* OLD
	//Qk=Qvk-1+a[rk-Qk-1]    ---> Qk=Qvk-1+a[rk-(maxOfFutureState)Qk-1]
	public void UpdateWeightedProbability(float reward){
		int[] state = game.myAIStateController.stateArray;

		float q_probability = GetCurrentProbability();

//		Debug.Log ("before: " + q_probability);

		//k_iteration++;
		q_probability = q_probability + alphaWeight*(reward - q_probability);

		qValArray[state[0], state[1], state[2], state[3], state[4]] = q_probability;

//		Debug.Log ("after: " + q_probability);
	}
	*/
}
