using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	// Start is called before the first frame update
	public GameObject popupText;
	public PlayerScript player;
	public Transform canvas;
	public OrderScript order;
	public PhoneUIScript phone;
	public float rating;
	public float maxTimeBetweenOrders;
	public float timeBetweenOrders;
	public MealScript meal;
	public Transform[] nah;
	public Vector2 starOrig;
	public RectTransform star;
	public DeliveryZone[] zones;
	public int ratings;
	public int realDeliveries;
	public bool gameover = false;
	public AudioSource sound;
	public AudioClip newSound;
	public AudioClip noSound;
	public AudioClip seagull;
	public Transform arrow;
	public float gullAttackTimeout = 0;


	public int cancelledOrders;

	void Start() {
		starOrig = star.sizeDelta;
		gameover = false;
		cancelledOrders = 0;
		player = FindFirstObjectByType<PlayerScript>();
	}

	// Update is called once per frame
	void Update() {
		if (gullAttackTimeout > 0) {
			gullAttackTimeout = Mathf.Max(gullAttackTimeout - Time.deltaTime, 0);
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			showPopupWithFade("Test!");
			/*createOrder();
			buttonAcceptOrder();*/
		}

		if (meal == null) {
			timeBetweenOrders += Time.deltaTime;
			if (timeBetweenOrders > maxTimeBetweenOrders) {
				createOrder();
				acceptOrder(meal);
				timeBetweenOrders = 0;
				maxTimeBetweenOrders = Random.Range(2, 10);
			}
		}
		else {
			if (order.time > order.allocatedDeliveryTime + 30) {
				showPopupWithFade("Order Cancelled by Customer: " + 0 + " stars");
				cancelOrder(0, true);
			}
		}

		if (order.meal != null && player.meal == null) {
			arrow.gameObject.SetActive(true);
			arrow.LookAt(order.meal.transform);
		}
		else if (order.zone != null) {
			arrow.gameObject.SetActive(true);
			Vector3 oldArrowPosition = order.zone.transform.localPosition;
			arrow.LookAt(order.zone.transform);
			arrow.localPosition = oldArrowPosition;
		}
		else {
			arrow.gameObject.SetActive(false);
		}


		if (cancelledOrders  == 1) {
			nah[0].gameObject.SetActive(true);
		}

		else if (cancelledOrders  == 2) {
			nah[1].gameObject.SetActive(true);
		}

		else if (cancelledOrders == 3) {
			nah[2].gameObject.SetActive(true);
		}

		else if (cancelledOrders == 0) {
			nah[0].gameObject.SetActive(false);
			nah[1].gameObject.SetActive(false);
			nah[2].gameObject.SetActive(false);
		}


		star.sizeDelta = new Vector2(starOrig.x * (rating / 5), starOrig.y);
		if (!gameover && cancelledOrders == 3) {
			gameOver();
		}
	}

	public void handOverMeal(MealScript meal, bool correctOrder) {
		meal.transform.SetParent(order.zone.transform);
		meal.transform.localPosition = new Vector3(0, 0, 0);
		transform.rotation = new Quaternion(0, 0, 0, 0);
		meal.delivered = true;

		//If it's the correct order
		if (correctOrder) {
			//Was it under the allocated delivery time?
			if (order.time <= order.allocatedDeliveryTime) {
				var stars = Random.Range(4f, 5f);
				showPopupWithFade("Order Delivered Successfully: " + (stars.ToString("N1")) + " stars");
				cancelOrder(stars);
				realDeliveries++;
			}

			//Was it late?
			else {
				var stars = Random.Range(2f, 3.5f);
				showPopupWithFade("Order Delivered Late: " + (stars.ToString("N1")) + " stars");
				cancelOrder(stars);
				realDeliveries++;
			}
		}

		//Was it the wrong order?
		else {
			var stars = Random.Range(0.5f, 1.5f);
			showPopupWithFade("Wrong Order Delivered: " + (stars.ToString("N1")) + " stars");
			cancelOrder(stars, true);
			realDeliveries++;
		}
	}

	public void showPopupWithFade(string text) {
		GameObject pt = Instantiate(popupText);
		pt.transform.SetParent(canvas);
		pt.transform.localScale = new Vector3(1, 1, 1);
		pt.transform.position = new Vector3(0, 0, 0);

		var script = pt.GetComponent<PopupText>();

		if (text != null) {
			script.text = text;
		}
	}

	//Accepts the order
	public void acceptOrder(MealScript meal) {
		if (order.meal == null) {
			arrow.gameObject.SetActive(true);

			sound.PlayOneShot(newSound);
			phone.ChangeScreen(2);

			//TODO: make this calculate a new time based on number of delivered orders + a random variance
			order.allocatedDeliveryTime = Mathf.Max(120f - (ratings * 2), 30);

			//Pick a random meal
			order.meal = meal; //meals[Random.Range(0, meals.Length - 1)];

			//Pick a random zone
			var rand = Random.Range(1, zones.Length - 1);
			order.zone = zones[rand];
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


			//Show the meal preview
			foreach (var mealPreview in phone.mealPreviews) {
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
		//phone.ChangeScreen(2);
		phone.show = true;
	}

	public void buttonAcceptOrder() {
		acceptOrder(meal);
	}

	public void cancelOrder(float rating) {
		cancelOrder(rating, false);
	}

	public void cancelOrder(float rating, bool penalty) {
		//Add the penalty
		if (penalty) {
			sound.PlayOneShot(noSound);
			cancelledOrders++;
		}

		//Remove the current food from the player
		dropFood();

		//Hide the arrow
		arrow.gameObject.SetActive(false);

		//Null out the meal
		order.meal = null;
		meal = null;

		//Change back to the logo screen
		phone.ChangeScreen(0);

		//Hide the phone
		phone.show = false;

		//Hide and unset the zone
		if (order.zone != null) {
			order.zone.gameObject.SetActive(false);
			order.zone = null;
		}

		//Set the delivery ID
		order.deliveryID = 0;
		phone.UpdateOrderNumberText(0);

		//Reset the timer
		order.time = 0;


		//Update the star rating
		updateRating(rating != null && rating != 0 ? rating : 0.5f);
	}

	public void UICancelOrder() {
		showPopupWithFade("Order Cancelled: 0 stars");
		cancelOrder(0, true);
	}

	public void GullStoleFood() {
		showPopupWithFade("Order Stolen by Seagulls: 0 stars");
		gullAttackTimeout = 3;
		cancelOrder(0, true);
		sound.PlayOneShot(seagull);
	}

	public void dropFood() {
		if (player.meal != null) {
			phone.dropFoodButton.SetActive(false);
			player.meal.parent.Spawn();
			Destroy(player.meal.gameObject);
		}

		player.meal = null;
	}

	public void gameOver() {
		gameover = true;
		int lastPB = PlayerPrefs.GetInt("pbDeliveries");
		if (realDeliveries > lastPB) {
			PlayerPrefs.SetInt("pbDeliveries", realDeliveries);
			PlayerPrefs.SetString("pb",
				realDeliveries + " Deliveries - " + rating.ToString().Substring(0, 3) + " Stars");
		}

		StartCoroutine(getRequest(rating.ToString(), realDeliveries.ToString()));
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


	IEnumerator getRequest(string stars, string deliveries) {
		Debug.Log(11111);
		UnityWebRequest uwr =
			UnityWebRequest.Get("https://ldjam53.shanegadsby.com/post-score?score=" + deliveries + "&name=" + stars);
		yield return uwr.SendWebRequest();

		if (uwr.isNetworkError) {
			Debug.Log("Error While Sending: " + uwr.error);
		}
		else {
			Debug.Log("Received: " + uwr.downloadHandler.text);
		}

		Debug.Log(22222);
		SceneManager.LoadScene("GameOver");
	}
}