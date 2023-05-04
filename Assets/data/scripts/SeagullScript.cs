using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SeagullScript : MonoBehaviour {
	public GameManager gm;
	public float speed = 2f;
	public float retreatSpeed = 7f;
	public bool justFed = false;
	public float justFedTimer = 0;
	public Transform nest;

	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (justFed || gm.player.meal == null) {
			var step = retreatSpeed * Time.deltaTime; // calculate distance to move
				transform.position = Vector3.MoveTowards(transform.position, nest.position, step);
			
				justFedTimer += Time.deltaTime;

				if (justFedTimer > 5) {
					justFed = false;
				}
				
		}
		else {
			if (gm.player.meal != null) {
				var step = speed * Time.deltaTime; // calculate distance to move
				transform.position = Vector3.MoveTowards(transform.position, gm.player.transform.position, step);
			}
		}
	}


	private void OnTriggerEnter(Collider other) {
		PlayerScript player = null;
		if (other.transform.tag == "Player") {
			player = other.transform.GetComponent<PlayerScript>();
		}

		else if (other.transform.tag == "subplayer") {
			player = other.transform.parent.transform.GetComponent<PlayerScript>();
		}

		if (player != null && player.meal != null && justFed == false && gm.gullAttackTimeout == 0) {
			justFed = true;
			justFedTimer = 0;
			gm.GullStoleFood();
		}
	}
}