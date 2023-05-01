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
	public DeliveryZone[] zones;
	public float rating;
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
				updateRating(Random.Range(4f, 5f));
			}

			//Was it late?
			else {
				showPopupWithFade("Order Delivered Late!");
				updateRating(Random.Range(2f, 3.5f));
			}
		}

		//Was it the wrong order?
		else {
			showPopupWithFade("Wrong Order Delivered!");
			updateRating(Random.Range(0.5f, 1.5f));
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

	//Creates the order
	public void createOrder() {
		if (order.meal == null) {
			//TODO: make this calculate a new time based on number of delivered orders + a random variance
			order.allocatedDeliveryTime = 60f;

			//Get a list of all meals
			var meals = Object.FindObjectsOfType<MealScript>();

			//Pick a random meal
			order.meal = player.meal = meals[Random.Range(0, meals.Length - 1)];

			//Set the delivery ID
			order.deliveryID = Random.Range(1000, 9999);
			order.meal.deliveryID = order.deliveryID;
			order.zone.deliveryID = order.deliveryID;


			//Reset the timer
			order.time = 0;

			//Reset the order accepted/declined flags
			order.acceptedOrder = false;
			order.declinedOrder = false;

			//Show incoming order UI
			phone.show = true;
			phone.ChangeScreen(1);

			//Show the meal preview
			foreach (var mealPreview in phone.mealPreviews) {
				mealPreview.gameObject.SetActive(false);
				if (PrefabUtility.GetCorrespondingObjectFromSource(mealPreview).name ==
					PrefabUtility.GetCorrespondingObjectFromSource(order.meal).name) {
					mealPreview.gameObject.SetActive(true);
				}
			}
		}
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