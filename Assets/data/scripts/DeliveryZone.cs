using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour {
	public GameManager gm;
	public int deliveryID;

	// Start is called before the first frame update
	void Start() {
		gm = FindFirstObjectByType<GameManager>();
	}

	// Update is called once per frame
	void Update() {
	}

	private void OnTriggerEnter(Collider other) {
		if (gm.meal != null) {
			gm.handOverMeal(deliveryID);
		}
	}
}