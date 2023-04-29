using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealScript : MonoBehaviour {
	private bool isHeld = false;
	public GameManager gm;
	public DeliveryZone deliveryZone;
	public int deliveryID;

	// Start is called before the first frame update
	void Start() {
		gm = FindFirstObjectByType<GameManager>();
	}

	// Update is called once per frame
	void Update() {
	}

	private void OnTriggerEnter(Collider other) {
		if (!isHeld) {
			PlayerScript player = null;

			if (other.transform.tag == "Player") {
				player = other.transform.GetComponent<PlayerScript>();
			}

			else if (other.transform.tag == "subplayer") {
				player = other.transform.parent.transform.GetComponent<PlayerScript>();
			}

			if (player != null) {
				gm.meal = transform.GetComponent<MealScript>();
				transform.SetParent(player.mealHolder);
				transform.localPosition = new Vector3(0, 0, 0);
			}
		}
	}
}