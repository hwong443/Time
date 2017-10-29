using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttack : Attack {
	private int damage;

	public SimpleAttack(int damage){
		this.damage = damage;
	}

	public int GetDamage(){
		return damage;
	}
}
