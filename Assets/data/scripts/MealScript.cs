using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using Random = UnityEngine.Random;

public class MealScript : MonoBehaviour {
	private bool isHeld = false;
	private bool playerInRange = false;
	public bool delivered = false;
	public GameManager gm;
	public MealSpawner parent;
	public DeliveryZone deliveryZone;
	public int deliveryID;
	public TextMeshProUGUI text;
	public PlayerScript player;
	public string prefabName;


	// Start is called before the first frame update
	void Start() {
		deliveryID = Random.Range(1000, 9999);
		gm = FindFirstObjectByType<GameManager>();
		player = FindFirstObjectByType<PlayerScript>();
		text = GameObject.Find("/Game Manager/phone-canvas/Collect meal").GetComponent<TextMeshProUGUI>();
	}

	// Update is called once per frame
	void Update() {
		if (!isHeld && playerInRange && player.meal == null && Input.GetKeyDown(KeyCode.E)) {
			isHeld = true;
			gm.phone.dropFoodButton.SetActive(true);
			player.meal = transform.GetComponent<MealScript>();
			transform.SetParent(player.mealHolder);
			transform.localPosition = new Vector3(0, 0, 0);
			transform.rotation = new Quaternion(0, 0, 0, 0);

			text.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other) {
		PlayerScript player = null;
		if (other.transform.tag == "Player") {
			player = other.transform.GetComponent<PlayerScript>();
		}

		else if (other.transform.tag == "subplayer") {
			player = other.transform.parent.transform.GetComponent<PlayerScript>();
		}

		if (player != null && player.meal == null && !delivered && gm.order.deliveryID != 0 &&
			gm.order.deliveryID != null) {
			playerInRange = true;
			text.gameObject.SetActive(playerInRange);
			text.text = "Collect meal #" + deliveryID + " (E)";
		}
	}

	private void OnTriggerExit(Collider other) {
		PlayerScript player = null;
		if (other.transform.tag == "Player") {
			player = other.transform.GetComponent<PlayerScript>();
		}

		else if (other.transform.tag == "subplayer") {
			player = other.transform.parent.transform.GetComponent<PlayerScript>();
		}

		if (player != null) {
			playerInRange = false;
			text.gameObject.SetActive(playerInRange);
		}
	}
}