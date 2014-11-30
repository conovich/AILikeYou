using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class AIReaderRecorder_New : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void WriteActionsToFile(List<AIAction_New> actionList, string fileName){

		if(fileName != ""){

			StreamWriter myStreamWriter = File.CreateText(fileName);

			for(int i = 0; i < actionList.Count; i++){
				for(int j = 0; j < 11; j++){
					for(int k = 0; k < 11; k++){
						for(int l = 0; l < 11; l++){
							for(int m = 0; m < 11; m++){
								for(int n = 0; n < 3; n++){
									myStreamWriter.WriteLine(actionList[i].gameObject.name + " " + j + " " + k + " " + l + " " + m + " " + n + " " + actionList[i].qValArray[j,k,l,m,n]);
								}
							}
						}
					}
				}
				myStreamWriter.WriteLine("\n");
			}
			myStreamWriter.Flush();
			myStreamWriter.Close();
		}
		else{
			Debug.Log("Empty file name!");
		}
	}

	public void ReadActionsFromFile(List<AIAction_New> actionList, string fileName){

		if(fileName != ""){
			string line = "";
			int readIndex = 0;
			StreamReader myStreamReader = new StreamReader(fileName);
			line = myStreamReader.ReadLine();

			Debug.Log("reading");
			while (line != null){
				string[] splitLine = line.Split(' ');
				if(splitLine.Length == 7){
					string name = splitLine[0];
					int healthIndex = int.Parse(splitLine[1]);
					int turretHealthIndex = int.Parse(splitLine[2]);
					int turretDistanceIndex = int.Parse(splitLine[3]);
					int bulletDistanceIndex = int.Parse(splitLine[4]);
					int bulletHeightIndex = int.Parse(splitLine[5]);
					float probability = float.Parse(splitLine[6]);
					//string probability = splitLine[1];
					//string iteration = splitLine[2];

					if(readIndex < actionList.Count){
						if(actionList[readIndex].name == name){
							actionList[readIndex].qValArray[healthIndex, turretHealthIndex, turretDistanceIndex, bulletDistanceIndex, bulletHeightIndex] = probability;
						}
						else{
							Debug.Log("Actions are out of order");
						}
					}
				}
				line = myStreamReader.ReadLine();
				readIndex++;
			}
		}
		else{
			Debug.Log("Empty file name!");
		}
		/*
		if(FileName != ""){
			string line1 = "";
			string line2 = "";
			StreamReader myStreamReader = new StreamReader(FileName);

			//first line: action name
			line1 = myStreamReader.ReadLine();
			line2 = myStreamReader.ReadLine();

			if(line1 != null && line2 != null){
				newActionTable.Add(line1, float.Parse(line2));
			}


		}
		else{
			Debug.Log("Empty file name!");
		}*/
	}
}
