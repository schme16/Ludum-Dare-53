using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour {
	public GameManager gm;
	public int deliveryID;
	public bool fade = false;

	// Start is called before the first frame update
	void Start() {
		gm = FindFirstObjectByType<GameManager>();
	}

	// Update is called once per frame
	void Update() {
		//TODO make this fade the delivery zone out
		if (fade) {
		}
	}

	private void OnTriggerEnter(Collider other) {
		PlayerScript player = null;
		if (other.transform.CompareTag("Player")) {
			player = other.transform.GetComponent<PlayerScript>();
		}

		else if (other.transform.CompareTag("subplayer")) {
			player = other.transform.parent.transform.GetComponent<PlayerScript>();
		}

		if (player != null && player.meal != null) {
			gm.handOverMeal(player.meal, player.meal.deliveryID == deliveryID);
		}
	}
}