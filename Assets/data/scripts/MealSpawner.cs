using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MealSpawner : MonoBehaviour {
	public GameObject mealPrefab;
	public GameObject currentMeal;

	// Start is called before the first frame update
	void Start() {
		Spawn();
	}

	public void Spawn() {
		currentMeal = Instantiate(mealPrefab);
		currentMeal.transform.SetParent(transform);
		currentMeal.transform.localPosition = new Vector3(0, 0, 0);
		MealScript meal = currentMeal.GetComponent<MealScript>();
		meal.prefabName = mealPrefab.name;
		meal.parent = this;
		currentMeal.transform.rotation = transform.rotation;
	}

	// Update is called once per frame
	void Update() {
	}
}