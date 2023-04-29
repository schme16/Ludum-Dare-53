using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour {
	// Start is called before the first frame update
	public float lifetime = 5;
	public TextMeshProUGUI text;
	float time = 0;
	private float size;
	void Start() {
		text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
		size = text.fontSize;
	}

	// Update is called once per frame
	void Update() {
		text.fontSize = Mathf.Lerp(fontSize,  fontSize+ 10, time / lifetime); 
		time += Time.deltaTime;
		if (time > lifetime) {
			Destroy(gameObject);
		}
	}
}