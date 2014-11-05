using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class AIAction : MonoBehaviour {

	public float q_probability;
	public float k_iteration;

	//weighted
	public float alphaWeight = 0.1f;

	public AIAction(float initProbability){
		q_probability = initProbability;
		Debug.Log("setting init probability");
	}

	void Awake(){
		Debug.Log(q_probability);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void SetQProbability(float newProbability){
		q_probability = newProbability;
	}

	//Qk+1= Qk + 1/(k+1) [rk+1-Qk]
	public void UpdateProbability(float reward){
		k_iteration++;
		float newQ_probability = q_probability + (1.0f/k_iteration) * (reward - q_probability);
		SetQProbability(newQ_probability);
	}

	//Qk=Qvk-1+a[rk-Qk-1]
	public void UpdateWeightedProbability(float reward){
		Debug.Log ("before: " + q_probability);
		k_iteration++;
		q_probability = q_probability + alphaWeight*(reward - q_probability);
		Debug.Log ("after: " + q_probability);
	}
}
