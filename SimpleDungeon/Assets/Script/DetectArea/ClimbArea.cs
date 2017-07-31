using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbArea : MonoBehaviour {
	public int climbingObjects;

	void OnTriggerEnter2D(Collider2D colli) {
		if (colli.tag == "Climbable") {
			++climbingObjects;
		}
	}

	void OnTriggerExit2D(Collider2D colli) {
		if (colli.tag == "Climbable" && climbingObjects > 0) {
			--climbingObjects;
		}
	}
}
