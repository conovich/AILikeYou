using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AIController_New: MonoBehaviour {
	
	public PlayerController_New MyPlayer;
	public AIReaderRecorder_New MyReaderRecorder;

	public float Positive_reward;
	public float Negative_reward;

	public bool ShouldCreateNewActions;
	public GameObject AIAction_BasicPrefab;

	// Use this for initialization
	void Start () {

		if(ShouldCreateNewActions){
			InstantiateNewActions();
		}
		else{
			LoadProbabilities();
		}
		LinkActions();
	}


	//ACTION CREATION AND LINKING

	public string[] actionNames;
	public string newFolderName;

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
			}
		}
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
		CheckPlayerStatus();
	}

	//TODO: REFACTOR THE HELL OUT OF THIS
	void CheckPlayerStatus(){
		/*if(MyPlayer.IsHit){
	
		}
		if(MyPlayer.HasHitEnemy){

		}
		
		MyPlayer.IsHit = false;
		MyPlayer.HasHitEnemy = false;
		*/
	}

	public GameObject FindInActionList(string name){
		for(int i = 0; i < actionObjList.Count; i++){
			if(actionObjList[i].name == name){
				return actionObjList[i];
			}
		}
		return null;
	}

	public void UpdateProbabilityPositive(string actionName){
		Debug.Log("updating POSITIVE probability");
		GameObject actionToUpdate = FindInActionList(actionName);
		if(actionToUpdate != null){
			actionToUpdate.GetComponent<AIAction>().UpdateWeightedProbability(Positive_reward);
		}
	}

	public void UpdateProbabilityNegative(string actionName){
		Debug.Log("updating NEGATIVE probability");
		GameObject actionToUpdate = FindInActionList(actionName);
		if(actionToUpdate != null){
			actionToUpdate.GetComponent<AIAction>().UpdateWeightedProbability(Negative_reward);
		}
	}

	public List<AIAction_New> GetAIActionList(){
		/*Object[] AssetsInActionFolder = Resources.LoadAll("Prefabs/AIActions/" + newFolderName);
		
		List<AIAction_New> AIActionList = new List<AIAction_New>();
		
		for(int i = 0; i < AssetsInActionFolder.Length; i++){
			if(PrefabUtility.GetPrefabType(AssetsInActionFolder[i]) != PrefabType.None){
				AIAction_New action = ((GameObject)AssetsInActionFolder[i]).GetComponent<AIAction_New>();
				if(action != null){
					AIActionList.Add(action);
				}
			}
		}*/
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
		MyReaderRecorder.ReadActionsFromFile(GetAIActionList(), newFolderName);
	}

	void RecordProbabilities(){
		MyReaderRecorder.WriteActionsToFile(GetAIActionList(), newFolderName);
	}

	void OnApplicationQuit(){
		Debug.Log("recording");
		RecordProbabilities();
		CleanUp();
	}

}