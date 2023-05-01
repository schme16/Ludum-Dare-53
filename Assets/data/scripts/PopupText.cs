using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour {
	// Start is called before the first frame update
	public float lifetime = 3;
	public TextMeshProUGUI textMesh;
	public string text;
	public Transform currentScreen;
	public Transform[] screens;
	float time = 0;
	public Vector3 size;
	public Transform rect;

	void Start() {
		textMesh = transform.GetComponentInChildren<TextMeshProUGUI>();
		textMesh.text = text;
		size = textMesh.transform.localScale;
	}

	// Update is called once per frame
	void Update() {
		transform.localPosition = new Vector3(0,0,0);
		time += Time.deltaTime;
		textMesh.transform.localScale = Vector3.Lerp(size, size * 5, time / lifetime);
		textMesh.color = Color.Lerp(new Color(255, 255, 255, 1), new Color(255, 255, 255, 0), time / lifetime);

		if (time > lifetime) {
			Destroy(gameObject);
		}
	}
}