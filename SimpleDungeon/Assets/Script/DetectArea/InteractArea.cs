using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractArea : MonoBehaviour {
	

	void Start(){
		
	}

	void OnTriggerEnter2D(Collider2D colli) {
		Debug.Log (colli.name);
		colli.SendMessage ("Interface", null, SendMessageOptions.DontRequireReceiver);
	}
}
