using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScript : MonoBehaviour {
	public MealScript meal;
	public PhoneUIScript phone;
	public DeliveryZone zone;
	public float allocatedDeliveryTime;
	public float time;
	public bool acceptedOrder;
	public bool declinedOrder;
	public int deliveryID;
	public Transform[] takeoutIcons;
	public Transform takeoutIcon;


	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		time += Time.deltaTime;
		TimeSpan t = TimeSpan.FromSeconds((allocatedDeliveryTime - time));
		phone.countdown.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
	}
}