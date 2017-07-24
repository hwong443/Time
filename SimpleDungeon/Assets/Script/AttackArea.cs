using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour {
	public Attackable attacker;
	public Attack attack = new ShortWeapon(10);

	void OnTriggerEnter2D(Collider2D colli) {
		Debug.Log (colli.name);
		colli.SendMessage("TakeDamage", attack, SendMessageOptions.DontRequireReceiver);
	}
}
