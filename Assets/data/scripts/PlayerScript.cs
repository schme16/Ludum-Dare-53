using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	// Start is called before the first frame update
	public float speed = 1;
	public float speedModifier = 0;
	public float turnSpeed = 2;
	public MealScript meal;
	public Transform[] wheels;
	public Transform mealHolder;
	Rigidbody rb;
	private Camera cam;

	void Start() {
		cam = Camera.main;
		rb = transform.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update() {
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");

		if (x != 0) {
			Quaternion rotation = Quaternion.Euler(0, transform.eulerAngles.y + (x * turnSpeed), 0);
			rb.MoveRotation(rotation);
		}

		if (y != 0) {
			rb.AddForce(transform.forward * (y * (speed + speedModifier)));
		}

		var velocity = Vector3.Dot(rb.velocity, transform.forward);

		wheels[0].Rotate(new Vector3(0, velocity, 0));
		wheels[1].Rotate(new Vector3(0, velocity, 0));
		wheels[2].Rotate(new Vector3(0, 0, velocity));
	}
}