using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour {
	public string _name;
	public string _score;
	public string _index;

	private TextMeshProUGUI text;

	// Start is called before the first frame update
	void Start() {
		text = gameObject.GetComponent<TextMeshProUGUI>();
	}

	// Update is called once per frame
	void Update() {
		if (_name == null || _name.Length == 0) {
			text.text = _index + ". ";
		}
		else {
			text.text = _index + ". " + _score + " Deliveries - " + _name.Substring(0, 3) + " Stars";
		}
	}
}