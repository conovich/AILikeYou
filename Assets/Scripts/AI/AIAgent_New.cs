using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAgent_New : MonoBehaviour {

	public PlayerController_New MyPlayer;
	public AIController_New MyAIController;

	public float IdleTimeMax;
	public float IdleTimeMin;

	// Use this for initialization
	void Start () {
		StartCoroutine(ActionLoop());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator ActionLoop(){
		while(true){
			if(!MyPlayer.isDucking && !MyPlayer.isJumping && !MyPlayer.isMovingBackward && !MyPlayer.isMovingForward && !MyPlayer.shieldOn){
				ChooseAction();
			}

			float idleTime = Random.Range(IdleTimeMin, IdleTimeMax);
			yield return new WaitForSeconds(idleTime);
			yield return 0;
		}
	}

	void ChooseAction(){
		// greedy action
		AIAction_New chosenAction = RetrieveNthHighestProbabilityAction(0);
		Debug.Log("nameeeee" + chosenAction.name);
		switch( chosenAction.name ){
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
			Debug.Log("Name Doesn't Match!");
		break;
		}
	}

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
		
		return sortedList;
	}

	//N should be 0 to # of actions
	AIAction_New RetrieveNthHighestProbabilityAction(int N){
		List<AIAction_New> actionList = MyAIController.GetAIActionList();
		actionList = SortList(actionList);
		/*Debug.Log("action list count: " + actionList.Count);

		Debug.Log("Nth" + N);
		Debug.Log("sorted list count:" + actionList.Count);
*/
		if(actionList.Count > N){
			return actionList[N];
		}
		else{
			return null;
		}
	}


}
