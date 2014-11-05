using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class AIReaderRecorder : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void WriteActionsToFile(List<AIAction> actionList, string fileName){

		if(fileName != ""){

			StreamWriter myStreamWriter = File.CreateText(fileName);

			for(int i = 0; i < actionList.Count; i++){
				myStreamWriter.WriteLine(actionList[i].gameObject.name + " " + actionList[i].q_probability + " " + actionList[i].k_iteration);
			}

			myStreamWriter.Close();
		}
		else{
			Debug.Log("Empty file name!");
		}
	}

	public void ReadActionsFromFile(List<AIAction> actionList, string fileName){

		if(fileName != ""){
			string line = "";
			int readIndex = 0;
			StreamReader myStreamReader = new StreamReader(fileName);
			line = myStreamReader.ReadLine();


			while (line != null){
				string[] splitLine = line.Split(' ');
				string name = splitLine[0];
				string probability = splitLine[1];
				string iteration = splitLine[2];

				if(readIndex < actionList.Count){
					if(actionList[readIndex].name == name){
						actionList[readIndex].q_probability = float.Parse(probability);
						actionList[readIndex].k_iteration = float.Parse(iteration);
					}
					else{
						Debug.Log("Actions are out of order");
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
