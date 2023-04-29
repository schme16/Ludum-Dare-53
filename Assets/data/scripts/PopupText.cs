using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour {
	// Start is called before the first frame update
	public float lifetime = 3;
	public TextMeshProUGUI text;
	float time = 0;
	private Vector3 size;
	void Start() {
		text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
		size = text.transform.localScale;
	}

	// Update is called once per frame
	void Update() {
		time += Time.deltaTime;
		text.transform.localScale = Vector3.Lerp(size,  size * 5, time / lifetime); 
		text.color = Color.Lerp(new Color(255,255,255,1), new Color(255,255,255,0), time / lifetime); 

		if (time > lifetime) {
			Destroy(gameObject);
		}
	}
}