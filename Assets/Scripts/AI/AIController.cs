using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AIController : MonoBehaviour {
	
	public PlayerController MyPlayer;
	public AIReaderRecorder MyReaderRecorder;

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
				actionObjList.Add((GameObject)AssetsInActionFolder[i]);
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
		if(MyPlayer.IsHit){
			if(MyPlayer.IsPunchingLeft){
				UpdateProbabilityNegative("punchLeft");
			}
			if(MyPlayer.IsPunchingRight){
				UpdateProbabilityNegative("punchRight");
			}
			if(MyPlayer.IsKickingLeft){
				if(MyPlayer.IsDucking){
					UpdateProbabilityNegative("kickLeftDuck");
				}
				else{
					UpdateProbabilityNegative("kickLeft");
				}
			}
			if(MyPlayer.IsKickingRight){
				if(MyPlayer.IsDucking){
					UpdateProbabilityNegative("kickLeftDuck");
				}
				UpdateProbabilityNegative("kickRight");
			}
			if(MyPlayer.IsDucking && !MyPlayer.IsKickingLeft && !MyPlayer.IsKickingRight){
				UpdateProbabilityNegative("duck");
			}
			else if(MyPlayer.IsJumping){
				UpdateProbabilityNegative("jump");
			}	
		}
		if(MyPlayer.HasHitEnemy){
			if(MyPlayer.IsPunchingLeft){
				UpdateProbabilityPositive("punchLeft");
			}
			if(MyPlayer.IsPunchingRight){
				UpdateProbabilityPositive("punchRight");
			}
			if(MyPlayer.IsKickingLeft){
				if(MyPlayer.IsDucking){
					UpdateProbabilityPositive("kickLeftDuck");
				}
				else{
					UpdateProbabilityPositive("kickLeft");
				}
			}
			if(MyPlayer.IsKickingRight){
				if(MyPlayer.IsDucking){
					UpdateProbabilityPositive("kickLeftDuck");
				}
				UpdateProbabilityPositive("kickRight");
			}
			if(MyPlayer.IsDucking && !MyPlayer.IsKickingLeft && !MyPlayer.IsKickingRight){
				UpdateProbabilityPositive("duck");
			}
			else if(MyPlayer.IsJumping){
				UpdateProbabilityPositive("jump");
			}
		}
		
		MyPlayer.IsHit = false;
		MyPlayer.HasHitEnemy = false;
		
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

	public List<AIAction> GetAIActionList(){
		Object[] AssetsInActionFolder = Resources.LoadAll("Prefabs/AIActions/" + newFolderName);
		
		List<AIAction> AIActionList = new List<AIAction>();
		
		for(int i = 0; i < AssetsInActionFolder.Length; i++){
			if(PrefabUtility.GetPrefabType(AssetsInActionFolder[i]) != PrefabType.None){
				AIAction action = ((GameObject)AssetsInActionFolder[i]).GetComponent<AIAction>();
				if(action != null){
					AIActionList.Add(action);
				}
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
		RecordProbabilities();
	}

}
