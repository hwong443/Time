using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Character {

	public Transform target;

	protected void Start(){
		base.Start();
		actionTable [Action.Type.Attack] = new AttackAction (this);
		actionTable [Action.Type.JumpAttack] = new JumpAttackAction (this);
		target = GameObject.Find ("Player1").transform;

	}


	// ==== receiver =============
	public override void pressQ(){

	}
	public override void releaseQ(){

	}
	public override void pressE(){

	}
	public override void releaseE(){

	}

	public override void pressW(){
		base.pressW();
	}
	public override void releaseW(){
		base.releaseW();
	}
	public override void pressS(){
		base.pressS();
	}
	public override void releaseS(){
		base.releaseS();
	}
	public override void pressA(){
		base.pressA();
	}
	public override void releaseA(){
		base.releaseA();
	}
	public override void pressD(){
		base.pressD();
	}
	public override void releaseD(){
		base.releaseD();
	}
	public override void pressL(){
	}
	public override void releaseL(){
	}
}
