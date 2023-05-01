using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MealSpawner : MonoBehaviour {
	public GameObject mealPrefab;
	public GameObject currentMeal;
	public string prefabName;
	
	// Start is called before the first frame update
	void Start() {
		currentMeal = Instantiate(mealPrefab);
		currentMeal.transform.SetParent(transform);
		currentMeal.transform.localPosition = new Vector3(0, 0, 0);
		currentMeal.GetComponent<MealScript>().prefabName = mealPrefab.name;
	}

	// Update is called once per frame
	void Update() {
	}
}