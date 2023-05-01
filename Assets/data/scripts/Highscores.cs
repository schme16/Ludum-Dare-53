using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.Collections;
using TMPro;
using UnityEngine.Networking;

public class parseJSON {
	public string score;
	public string name;
}

public class Highscores : MonoBehaviour {
	public Transform[] highscoreEntries;
	public Transform highscoreWrapper;
	public TextMeshProUGUI pb;

	// Update is called once per frame
	void Update() {
	}


	// Sample JSON for the following script has attached.
	void Start() {
		StartCoroutine(getRequest());
		pb.text = PlayerPrefs.GetString("pb");
	}

	private void Processjson(string jsonString) {
		int i = 0;
		foreach (JsonData score in JsonMapper.ToObject(jsonString)) {
			Transform hsGO = highscoreEntries[i];
			Highscore hs = hsGO.GetComponentInChildren<Highscore>();
			//hsGO.SetParent(highscoreWrapper);
			//hsGO.localScale = new Vector3(1, 1, 1);
			//hsGO.localPosition = new Vector2(hsGO.localPosition.x, hsGO.localPosition.y - (75f * i));
			hs._name = score["name"].ToString();
			hs._score = score["score"].ToString();
			i++;
		}
	}

	IEnumerator getRequest() {
		UnityWebRequest uwr = UnityWebRequest.Get("https://ldjam53.shanegadsby.com/get-scores");
		yield return uwr.SendWebRequest();

		if (uwr.isNetworkError) {
		}
		else {
			Processjson(uwr.downloadHandler.text);
		}
	}
}