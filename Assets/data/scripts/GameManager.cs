using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// Start is called before the first frame update
	public MealScript meal;
	public DeliveryZone zone;
	public GameObject popupText;
	public PlayerScript player;
	public Transform canvas;

	void Start() {
		player = FindFirstObjectByType<PlayerScript>();
	}

	// Update is called once per frame
	void Update() {
	}

	public void handOverMeal() {
		if (meal) {
			Transform pt = Instantiate(popupText).transform;
			pt.SetParent(canvas);
			
			meal.transform.SetParent(zone.transform);
			meal.transform.localPosition = new Vector3(0,0,0);
		}
	}
}