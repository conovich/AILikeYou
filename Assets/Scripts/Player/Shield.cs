using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	GameState game { get { return GameState.Instance; } }

	public float fadeSpeed;

	Color fadeInColor;
	Color fadeOutColor;

	// Use this for initialization
	void Start () {
		fadeInColor = renderer.material.color;
		fadeOutColor = new Color(fadeInColor.r, fadeInColor.g, fadeInColor.b, 0.0f);

		collider.enabled = false;
		renderer.material.color = fadeOutColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Activate(){
		collider.enabled = true;
		StartCoroutine(FadeInShield());
	}

	public void Deactivate(){
		collider.enabled = false;
		StartCoroutine(FadeOutShield());
	}


	IEnumerator FadeInShield(){
		while(renderer.material.color.a != fadeInColor.a && collider.enabled){
			renderer.material.color = Color.Lerp (renderer.material.color, fadeInColor, Time.deltaTime*fadeSpeed);
			yield return 0;
		}
	}
	
	IEnumerator FadeOutShield(){
		while(renderer.material.color.a != fadeOutColor.a && !collider.enabled){
			renderer.material.color = Color.Lerp (renderer.material.color, fadeOutColor, Time.deltaTime*fadeSpeed);
			yield return 0;
		}
	}
}
