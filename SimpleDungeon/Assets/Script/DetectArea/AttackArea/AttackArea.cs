using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour {
	private Attackable attacker;
	private Attack attack = new SimpleAttack(10);

	public void SetAttacker(Attackable attacker){
		this.attacker = attacker;
	}
	public void SetAttack(Attack attack){
		this.attack = attack;
	}

	void OnTriggerEnter2D(Collider2D colli) {
		if(attacker!=null && colli.tag != ((MonoBehaviour)attacker).tag){
			colli.SendMessage("TakeDamage", attack, SendMessageOptions.DontRequireReceiver);
		}
	}
}
