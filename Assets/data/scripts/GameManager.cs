using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// Start is called before the first frame update
	public GameObject popupText;
	public PlayerScript player;
	public Transform canvas;
	public OrderScript order;
	public PhoneUIScript phone;
	public float rating;
	public MealScript meal;
	public int ratings;

	void Start() {
		player = FindFirstObjectByType<PlayerScript>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			showPopupWithFade("Test!");
			createOrder();
		}
	}

	public void handOverMeal(MealScript meal, bool correctOrder) {
		meal.transform.SetParent(order.zone.transform);
		meal.transform.localPosition = new Vector3(0, 0, 0);
		transform.rotation = new Quaternion(0, 0, 0, 0);
		//order.zone.fade = true;
		order.zone = null;
		meal.delivered = true;
		player.meal = null;
		order.meal = null;

		//If it's the correct order
		if (correctOrder) {
			//Was it under the allocated delivery time?
			if (order.time <= order.allocatedDeliveryTime) {
				showPopupWithFade("Order Delivered Successfully!");
				cancelOrder(Random.Range(4f, 5f));
			}

			//Was it late?
			else {
				showPopupWithFade("Order Delivered Late!");
				cancelOrder(Random.Range(2f, 3.5f));
			}
		}

		//Was it the wrong order?
		else {
			showPopupWithFade("Wrong Order Delivered!");
			cancelOrder(Random.Range(0.5f, 1.5f));
		}
	}

	public void showPopupWithFade(string text) {
		GameObject pt = Instantiate(popupText);
		pt.transform.SetParent(canvas);

		var script = pt.GetComponent<PopupText>();

		if (text != null) {
			script.text = text;
		}
	}

	//Accepts the order
	public void acceptOrder(MealScript meal) {
		if (order.meal == null) {
			phone.ChangeScreen(2);

			//TODO: make this calculate a new time based on number of delivered orders + a random variance
			order.allocatedDeliveryTime = 60f;

			//Pick a random meal
			order.meal = meal; //meals[Random.Range(0, meals.Length - 1)];

			//Get a list of all zones
			var zones = Object.FindObjectsOfType<DeliveryZone>();

			//Pick a random zone
			order.zone = zones[Random.Range(0, zones.Length - 1)];
			foreach (var zone in zones) {
				zone.gameObject.SetActive(false);
			}
			order.zone.gameObject.SetActive(true);

			//Set the delivery ID
			order.deliveryID = order.meal.deliveryID;
			order.meal.deliveryID = order.deliveryID;
			order.zone.deliveryID = order.deliveryID;
			phone.UpdateOrderNumberText(order.deliveryID);

			//Reset the timer
			order.time = 0;

			//Reset the order accepted/declined flags
			order.acceptedOrder = true;
			order.declinedOrder = false;


			//Show the meal preview
			foreach (var mealPreview in phone.mealPreviews) {
				Debug.Log(mealPreview.name == order.meal.prefabName);
				mealPreview.gameObject.SetActive(false);
				if (mealPreview.name == order.meal.prefabName) {
					mealPreview.SetActive(true);
				}
			}
		}
	}

	public void createOrder() {
		//Get a list of all meals
		var meals = Object.FindObjectsOfType<MealScript>();
		meal = meals[Random.Range(0, meals.Length - 1)];

		//Show incoming order UI
		phone.ChangeScreen(1);
		phone.show = true;
	}

	public void buttonAcceptOrder() {
		acceptOrder(meal);
	}

	public void cancelOrder(float rating) {
		dropFood();
		
		//null out the meal
		order.meal = null;
		
		//Change back to the logo screen
		phone.ChangeScreen(0);
		
		//Hide the phone
		phone.show = false;
		
		//Pick a random zone
		order.zone = null;

		//Set the delivery ID
		order.deliveryID = 0;
		phone.UpdateOrderNumberText(0);

		//Reset the timer
		order.time = 0;

		//Reset the order accepted/declined flags
		order.acceptedOrder = false;
		order.declinedOrder = true;
		updateRating(rating != null && rating != 0 ? rating : 0.5f);
	}

	public void dropFood() {
		if (order.meal.gameObject != null) {
			Destroy(order.meal.gameObject);
		}
		player.meal = null;
	}

	//Creates the order
	public void updateRating(float value) {
		if (ratings > 0) {
			rating = ((rating * ratings) + value) / (ratings + 1);
		}
		else {
			rating = value;
		}

		ratings++;
	}
}