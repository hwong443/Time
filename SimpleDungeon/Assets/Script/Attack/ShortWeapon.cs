using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortWeapon : Attack {
	private int damage;

	public ShortWeapon(int damage){
		this.damage = damage;
	}

	public int GetDamage(){
		return damage;
	}
}
