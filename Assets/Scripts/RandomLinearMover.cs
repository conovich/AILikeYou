using UnityEngine;
using System.Collections;

public class RandomLinearMover : MonoBehaviour {

	public bool X;
	public bool Y;
	public bool Z;

	public float moveIncrement;

	public float maxDistance;

	// Use this for initialization
	void Start () {
		StartCoroutine(MovePositive());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator MovePositive(){
		float distanceMoved = 0;

		while(distanceMoved < maxDistance){
			if(X){
				transform.position += transform.right*moveIncrement;
			}
			if(Y){
				transform.position += transform.up*moveIncrement;
			}
			if(Z){
				transform.position += transform.forward*moveIncrement;
			}
			distanceMoved += moveIncrement;
			yield return 0;
		}
		SendMessage("FireBullet", SendMessageOptions.DontRequireReceiver); //in case a bullet needs to be fired when it turns.
		StartCoroutine(MoveNegative());
	}

	IEnumerator MoveNegative(){
		float distanceMoved = 0;
		while(distanceMoved < maxDistance){
			if(X){
				transform.position -= transform.right*moveIncrement;
			}
			if(Y){
				transform.position -= transform.up*moveIncrement;
			}
			if(Z){
				transform.position -= transform.forward*moveIncrement;
			}
			distanceMoved += moveIncrement;
			yield return 0;
		}
		SendMessage("FireBullet", SendMessageOptions.DontRequireReceiver); //in case a bullet needs to be fired when it turns.
		StartCoroutine(MovePositive());
	}
}
