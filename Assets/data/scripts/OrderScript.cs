using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScript : MonoBehaviour {
	public MealScript meal;
	public DeliveryZone zone;
	public float allocatedDeliveryTime;
	public float time;
	public bool acceptedOrder;
	public bool declinedOrder;
	public Transform[] takeoutIcons;
	public Transform takeoutIcon;


	// Start is called before the first frame update
	void Start() {
		acceptedOrder = false;
		declinedOrder = false;
	}

	// Update is called once per frame
	void Update() {
		if (acceptedOrder) {
			time += Time.deltaTime;
		}
	}
}