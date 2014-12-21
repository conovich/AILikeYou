using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AIAgent_New : MonoBehaviour {

	GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }

	public PlayerController_New MyPlayer;
	public AIController_New MyAIController;

	public float IdleTimeMax;
	public float IdleTimeMin;

	bool isRunningActionLoop = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(game.currentState == GameState_TurretTag.State.InGame && !isRunningActionLoop){
			Debug.Log("running ai action loop");
			StartCoroutine(ActionLoop());
			isRunningActionLoop = true;
		}
		else if(!(game.currentState == GameState_TurretTag.State.InGame) && isRunningActionLoop){
			Debug.Log("stopped ai action loop");
			StopCoroutine(ActionLoop());
			isRunningActionLoop = false;
		}
	}

	IEnumerator ActionLoop(){
		while(game.currentState == GameState_TurretTag.State.InGame){
			if(!MyPlayer.isDucking && !MyPlayer.isJumping && !MyPlayer.isMovingBackward && !MyPlayer.isMovingForward && !MyPlayer.shieldOn){
				ExecuteAction();
			}

			float idleTime = Random.Range(IdleTimeMin, IdleTimeMax);
			yield return new WaitForSeconds(idleTime);
		}
	}

	void ExecuteAction(){
		AIAction_New chosenAction = ChooseAction();

		string name = chosenAction.name;
		//Debug.Log("orig name: " + name);
		name = Regex.Replace( chosenAction.name, "(Clone)", "" );
		name = Regex.Replace( name, "[()]", "" );
		//Debug.Log("new name: " + name);

		switch( name ){
		case "jump" :
			MyPlayer.PlayerJump();
			break;
		case "duck" :
			MyPlayer.PlayerDuck();
			break;
		case "shield" :
			MyPlayer.PlayerShieldOn();
			break;
		case "moveForward" :
			MyPlayer.MoveRight();
			break;
		case "moveBackward" :
			MyPlayer.MoveLeft();
			break;
		default:
			Debug.Log("Name Doesn't Match!" + name);
			break;
		}
	}

	AIAction_New ChooseAction(){

		int[] stateArray = game.myAIStateController.stateArray;

		float sumQValues;
		float[] probabilityArray;

		SumQValues(stateArray, out sumQValues, out probabilityArray); //fills in the sum and the probability array

		float[] bPoints;

		CalculateBPoints(out bPoints, probabilityArray);

		float randomFloat = Random.Range (0.0f, 1.0f);
		//Debug.Log("random float: " + randomFloat);

		//find the bPoint that randomFloat is closest to
		float minDifference = randomFloat - bPoints[0];
		int bPointIndex = 0;
		for( int i = 1; i < bPoints.Length - 1; i++){ //don't need to compare to the last bPoint which equals 1
			float tempDifference = Mathf.Abs(randomFloat - bPoints[i]);
			if(tempDifference < minDifference){
				bPointIndex = i;
				minDifference = tempDifference;
			}
		}

		//Debug.Log("bpoint index: " + bPointIndex);
		AIAction_New chosenAction = MyAIController.aiActionList[bPointIndex];

		return chosenAction;

	}

	//sums q values, also populates probability array for optimization
	void SumQValues(int[] stateArray, out float sumQValues, out float[] probabilityArray){
		List<AIAction_New> actionList = MyAIController.aiActionList;

		int numActions = MyAIController.numActions;
		probabilityArray = new float[numActions];

		float sum = 0;

		for(int i = 0; i < probabilityArray.Length; i++){
			float qVal = actionList[i].qValArray[stateArray[0], stateArray[1], stateArray[2], stateArray[3], stateArray[4]];
			sum += qVal;
			probabilityArray[i] = qVal; //init probability array with the qvalues
			//Debug.Log("probability index: " + i + " value: " + probabilityArray[i]);
		}
	
		//average to get the actual probabilities
		for(int i = 0; i < probabilityArray.Length; i++){
			if(sum != 0){
				probabilityArray[i] = probabilityArray[i]/sum;
			}
			else{
				probabilityArray[i] = 1.0f/numActions;
			}
		}

		sumQValues = sum;

	}

	void CalculateBPoints(out float[] bPoints, float[] probabilityArray){

		int numActions = MyAIController.numActions;
		bPoints = new float[numActions + 1];

		bPoints[0] = 0;
		for(int i = 1; i < bPoints.Length; i++){
			bPoints[i] = bPoints[i-1] + probabilityArray[i-1];
			//Debug.Log("b point index: " + i + " value: " + bPoints[i]);
		}
	}


	/*
	//TODO: RE-IMPLEMENT
	//heap sort
	List<AIAction_New> SortList(List<AIAction_New> actionList){
		List<AIAction_New> sortedList = new List<AIAction_New>();
//		List<AIAction_New> actionListCopy = actionList;

		/*if(actionListCopy.Count > 0){

			AIAction_New highestProbabilityAction = actionListCopy[0];

			for(int i = 0; i < actionList.Count; i++){
				for(int j = 0; j < actionListCopy.Count; j++){
					if(j == 0){
						highestProbabilityAction = actionListCopy[0];
					}
					else{
						if(actionListCopy[j].q_probability > highestProbabilityAction.q_probability){
							highestProbabilityAction = actionListCopy[j];
						}
					}
				}
				
				//each time you find the max, remove it from the list
				//thus, each time you iterate, you find the next maximum
				sortedList.Add(highestProbabilityAction);
				actionListCopy.Remove(highestProbabilityAction);
				
		}*/
		/*
		return sortedList;
	}
*/
	/*


	//N should be 0 to # of actions
	AIAction_New RetrieveNthHighestProbabilityAction(int N){
		List<AIAction_New> actionList = MyAIController.GetAIActionList();
		actionList = SortList(actionList);
		/*Debug.Log("action list count: " + actionList.Count);

		Debug.Log("Nth" + N);
		Debug.Log("sorted list count:" + actionList.Count);
*/
/*		if(actionList.Count > N){
			return actionList[N];
		}
		else{
			return null;
		}
	}

*/
}
