using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneUIScript : MonoBehaviour {
	public GameManager gm;
	public PlayerScript player;
	public DeliveryZone zone;
	public OrderScript order;
	public bool show = false;
	public bool hovered = false;
	public float showHideSpeed = 1;
	public float showY;
	public float hideY;
	public float peakY;
	public int screen;
	public GameObject[] screens;
	public GameObject[] mealPreviews;
	public RectTransform rect;


	// Start is called before the first frame update
	void Start() {
		rect = transform as RectTransform;
		rect.position = new Vector3(rect.position.x, hideY, rect.position.z);

		foreach (var t in screens) {
			t.SetActive(false);
		}

		ChangeScreen(0);
	}

	// Update is called once per frame
	void Update() {
		float y = Mathf.Lerp(rect.position.y, (show ? showY : hovered ? peakY : hideY), Time.deltaTime * showHideSpeed);
		rect.position = new Vector3(rect.position.x, y, rect.position.z);
	}

	public void ShowUI(bool state) {
		show = state;
	}

	public void ToggleUI() {
		show = !show;
	}

	public void ChangeScreen(int index) {
		//TODO: make it animate between states

		if (screens[screen] != null) {
			screens[screen].SetActive(false);
		}

		if (screens[index] != null) {
			screens[index].SetActive(true);
		}

		screen = index;
	}
}