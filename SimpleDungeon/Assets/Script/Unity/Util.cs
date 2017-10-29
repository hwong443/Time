using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util{

	public static bool isEnemy(string tag1, string tag2){
		if(tag1.Equals(tag2))
			return false;
		if((tag1.Equals("Friendly") || tag1.Equals("Enemy"))
			&& (tag2.Equals("Friendly") || tag2.Equals("Enemy")))
			return true;
		return false;
	}

	public static bool isCharacter(string tag){
		return tag.Equals("Friendly") || tag.Equals("Enemy") || tag.Equals("Netural");
	}
}
