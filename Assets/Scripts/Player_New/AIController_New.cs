using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AIController_New: MonoBehaviour {
	/*
	class State{
		public int healthIndex;
		public int turretHealthIndex;
		public int turretDistanceIndex;
		public int bulletDistanceIndex;
		public int bulletHeightIndex;
	}

	State lastState;
*/
	GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }

	public PlayerController_New MyPlayer;
	public AIReaderRecorder_New MyReaderRecorder;

	public float Positive_reward;
	public float Negative_reward;

	public bool ShouldCreateNewActions;
	public GameObject AIAction_BasicPrefab;
	public float initActionValue;

	public List<AIAction_New> aiActionList { get { return GetAIActionList(); } }

	int currentNumRewards = 0;
	string currentAction = "";


	// Use this for initialization
	void Start () {
		/*
		MyPlayer.myHealthController.RemoveHealthDelegate += CheckPlayerStatus_Negative;
		MyPlayer.ReboundBulletDelegate += CheckPlayerStatus_Positive;
		MyPlayer.MoveForwardDelegate += CheckPlayerStatus_Positive;
		MyPlayer.MoveBackwardDelegate += CheckPlayerStatus_Negative;
*/
		MyPlayer.myHealthController.RemoveHealthDelegate += DecrementNumPlayerRewards;
		MyPlayer.ReboundBulletDelegate += IncrementNumPlayerRewards;
		MyPlayer.MoveForwardDelegate += IncrementNumPlayerRewards;
		MyPlayer.MoveBackwardDelegate += DecrementNumPlayerRewards;

		if(ShouldCreateNewActions){
			InstantiateNewActions();
			LinkActions();
			InitActions();
		}
		else{
			LinkActions();
			InitActions();
			LoadProbabilities();
		}
	}

	void InitActions(){
		List<AIAction_New> actionList = GetAIActionList();
		for(int i = 0; i < actionList.Count; i++){
			actionList[i].Init();
		}
	}


	//ACTION CREATION AND LINKING

	public string[] actionNames;
	public string newFolderName;
	public int numActions { get { return GetNumActions(); } }

	public List<GameObject> actionObjList;
	//public List<AIAction> actionList;
	
	void InstantiateNewActions(){
		for(int i = 0; i < actionNames.Length; i++){
			PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/AIActions/" + newFolderName + "/" + actionNames[i] + ".prefab", AIAction_BasicPrefab);
		}
	}

	void LinkActions(){

		actionObjList = new List<GameObject>();

		Object[] AssetsInActionFolder = Resources.LoadAll("Prefabs/AIActions/" + newFolderName);

		//if the object is a prefab, add it to the action list!
		for(int i = 0; i < AssetsInActionFolder.Length; i++){
			if(PrefabUtility.GetPrefabType(AssetsInActionFolder[i]) != PrefabType.None){
				GameObject newActionObject = Instantiate((GameObject)AssetsInActionFolder[i], transform.position, transform.rotation) as GameObject;
				newActionObject.transform.parent = transform;
				actionObjList.Add(newActionObject);
				//actionObjList.Add((GameObject)AssetsInActionFolder[i]);
				if(ShouldCreateNewActions){
					AIAction_New newAction = newActionObject.GetComponent<AIAction_New>();
					newAction.myAIController = GetComponent<AIController_New>();
				}
			}
		}
	}

	int GetNumActions(){
		return actionNames.Length;
	}

	void CleanUp(){
		for(int i = 0; i < transform.childCount; i++){
			if(transform.GetChild(0).tag == "AIAction"){
				Destroy(transform.GetChild(0));
			}
		}
	}


	//UPDATE LOOP STUFFS

	// Update is called once per frame
	void Update () {

		//update state arrays
		game.myAIStateController.UpdateStateArrays();

		//update qvalues if oustanding rewards exist -- rewards != 0
		UpdateQValues();

		//check for player input -- depending on the input, create rewards, store action executed
		CheckPlayerStatus();


		//else, if no player input, rewards = 0



	}

	void UpdateQValues(){
		GameObject actionToUpdateObj = FindInActionList(currentAction);

		for(int i = 0; i < Mathf.Abs(currentNumRewards); i++){

			Debug.Log("Updating QValues & there are rewards!");

			/*if(i < 0){
				CheckPlayerStatus_Negative();
			}
			else{
				CheckPlayerStatus_Positive();
			}*/
			if(actionToUpdateObj != null){
				//actionToUpdate.GetComponent<AIAction_New>().UpdateWeightedProbability(Positive_reward);
				AIAction_New actionToUpdate = actionToUpdateObj.GetComponent<AIAction_New>();

				float alphaWeight = 0.1f; //how fast does learning take place? //TODO: take this out of AIAction_New or something...
				float gammaWeight = 0.5f; //how important is future learning?

				int[] oldState = game.myAIStateController.lastStateArray;

				float newQVal = ( 1.0f - alphaWeight ) * ( actionToUpdate.qValArray[oldState[0], oldState[1], oldState[2], oldState[3], oldState[4]] );

				float reward = Positive_reward;
				if(currentNumRewards < 0){
					reward = Negative_reward;
				}
				newQVal += alphaWeight * ( reward + ( gammaWeight * GetMaxQValOfNextPossibleActions() ) );

				actionToUpdate.qValArray[oldState[0], oldState[1], oldState[2], oldState[3], oldState[4]] = newQVal;

				Debug.Log("new q value!: " + actionToUpdate.name + " " + newQVal);
			}

		}


	}

	float GetMaxQValOfNextPossibleActions(){
		float maxQVal = 0;
		int[] currentState = game.myAIStateController.stateArray;

		for ( int i = 0; i < aiActionList.Count; i++){

			float tempQVal = aiActionList[i].qValArray[currentState[0], currentState[1], currentState[2], currentState[3], currentState[4]];

			if( tempQVal > maxQVal){
				maxQVal = tempQVal;
			}
		}

		return maxQVal;

	}

	void CheckPlayerStatus(){
		currentNumRewards = 0;
		currentAction = null;

		if(MyPlayer.shieldOn){
			currentAction = "shield";
		}
		else if(MyPlayer.isJumping){
			currentAction = "jump";
		}
		else if(MyPlayer.isDucking){
			currentAction = "duck";
		}
		else if(MyPlayer.isMovingForward){
			currentAction = "moveForward";
		}
		else if(MyPlayer.isMovingBackward){
			currentAction = "moveBackward";
		}
	}

	void IncrementNumPlayerRewards(){
		currentNumRewards++;
	}

	void DecrementNumPlayerRewards(){
		currentNumRewards--;
	}

	/*
	//getting hurt
	//moving further from target
	void CheckPlayerStatus_Negative(){
		//Debug.Log("NEGATIVE PLAYER STATUS");
		if(MyPlayer.shieldOn){
			UpdateProbabilityNegative("shield");
		}
		else if(MyPlayer.isJumping){
			UpdateProbabilityNegative("jump");
		}
		else if(MyPlayer.isDucking){
			UpdateProbabilityNegative("duck");
		}
		else if(MyPlayer.isMovingForward){
			UpdateProbabilityNegative("moveForward");
			Debug.Log("negative moving forward");
		}
		else if(MyPlayer.isMovingBackward){
			UpdateProbabilityNegative("moveBackward");
		}
	}

	//hurting turret
	//rebounding bullet (instead of hurting turret?)
	//moving forward
	void CheckPlayerStatus_Positive(){
		//Debug.Log("POSITIVE PLAYER STATUS");
		if(MyPlayer.shieldOn){
			UpdateProbabilityPositive("shield");
		}
		else if(MyPlayer.isJumping){
			UpdateProbabilityPositive("jump");
		}
		else if(MyPlayer.isDucking){
			UpdateProbabilityPositive("duck");
		}
		else if(MyPlayer.isMovingForward){
			UpdateProbabilityPositive("moveForward");
			Debug.Log("positive moving forward");
		}
		else if(MyPlayer.isMovingBackward){
			UpdateProbabilityPositive("moveBackward");
		}
	}
*/

	public GameObject FindInActionList(string name){
		for(int i = 0; i < actionObjList.Count; i++){
			if(actionObjList[i].name == name || actionObjList[i].name == (name + "(Clone)")){
				//Debug.Log("found an object!" + actionObjList[i].name);
				return actionObjList[i];
			}
		}
		//Debug.Log("null object, looking for" + name);
		return null;
	}
/*
	public void UpdateProbabilityPositive(string actionName){
		//Debug.Log("updating POSITIVE probability" + actionName);
		GameObject actionToUpdate = FindInActionList(actionName);
		if(actionToUpdate != null){
			actionToUpdate.GetComponent<AIAction_New>().UpdateWeightedProbability(Positive_reward);
		}
	}

	public void UpdateProbabilityNegative(string actionName){
		//Debug.Log("updating NEGATIVE probability" + actionName);
		GameObject actionToUpdate = FindInActionList(actionName);
		if(actionToUpdate != null){
			actionToUpdate.GetComponent<AIAction_New>().UpdateWeightedProbability(Negative_reward);
		}
	}
*/
	List<AIAction_New> GetAIActionList(){
		List<AIAction_New> AIActionList = new List<AIAction_New>();
		for(int i = 0; i < transform.childCount; i++){
			GameObject child = transform.GetChild(i).gameObject;
			if(child.tag == "AIAction"){
				AIAction_New action = child.GetComponent<AIAction_New>();
				AIActionList.Add(action);
			}
		}

		return AIActionList;
	}

	void LoadProbabilities(){
		MyReaderRecorder.ReadActionsFromFile(aiActionList, newFolderName);
	}

	void RecordProbabilities(){
		MyReaderRecorder.WriteActionsToFile(aiActionList, newFolderName);
	}

	void OnApplicationQuit(){
		Debug.Log("recording");
		RecordProbabilities();
		CleanUp();
	}

}
