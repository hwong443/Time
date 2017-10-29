using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public Receiver receiver = null;
	public string characterObject = null;

	public void SetReceiver(){
		Character character = GameObject.Find (characterObject).GetComponent<Character> ();
		SetReceiver(character);
	}
	public void SetReceiver(string target){
		Character character = GameObject.Find (target).GetComponent<Character> ();
		SetReceiver(character);
	}
	public void SetReceiver(Character character){
		if(character != null){
			if(character.AI != null){
				Destroy(character.AI);
				character.AI = this;
			}
			receiver = character;
		}
	}
}
