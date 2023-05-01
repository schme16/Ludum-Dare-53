using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {
	public Animator animator;
	public string LevelToLoad;

	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		//if ()
	}

	public void FadeToLevel() {
		animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete() {
		SceneManager.LoadScene(LevelToLoad);
	}
}