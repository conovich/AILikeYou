using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAgent : MonoBehaviour {

	public PlayerController MyPlayer;
	public AIController MyAIController;

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
			if(!MyPlayer.IsPunchingLeft && !MyPlayer.IsPunchingRight && !MyPlayer.IsKickingLeft && !MyPlayer.IsKickingRight){
				ChooseAction();
			}

			float idleTime = Random.Range(IdleTimeMin, IdleTimeMax);
			yield return new WaitForSeconds(idleTime);
			yield return 0;
		}
	}

	void ChooseAction(){
		// greedy action
		AIAction chosenAction = RetrieveNthHighestProbabilityAction(0);

		switch( chosenAction.name ){
		case "jump" :
			MyPlayer.PlayerJump();
		break;
		case "duck" :
			MyPlayer.PlayerDuck();
		break;
		case "punchLeft" :
			MyPlayer.PlayerPunchLeft();
		break;
		case "punchRight" :
			MyPlayer.PlayerPunchRight();
		break;
		case "kickLeft" :
			MyPlayer.PlayerKickLeft();
		break;
		case "kickRight" :
			MyPlayer.PlayerKickRight();
		break;
		case "kickLeftDuck" :
			MyPlayer.PlayerKickLeft();
			MyPlayer.PlayerDuck();
		break;
		case "kickRightDuck" :
			MyPlayer.PlayerKickRight();
			MyPlayer.PlayerDuck();
		break;
		default:
			Debug.Log("Name Doesn't Match!");
		break;
		}
	}


	//heap sort
	List<AIAction> SortList(List<AIAction> actionList){
		List<AIAction> sortedList = new List<AIAction>();
		List<AIAction> actionListCopy = actionList;

		if(actionListCopy.Count > 0){

			AIAction highestProbabilityAction = actionListCopy[0];

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
				
			}
		}
		
		return sortedList;
	}

	//N should be 0 to # of actions
	AIAction RetrieveNthHighestProbabilityAction(int N){
		List<AIAction> actionList = MyAIController.GetAIActionList();
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
