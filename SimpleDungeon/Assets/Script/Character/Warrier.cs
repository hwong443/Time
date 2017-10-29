using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrier : Character {

	protected void Start(){
		base.Start();
		actionTable [Action.Type.Attack] = new AttackAction (this);
		actionTable [Action.Type.JumpAttack] = new JumpAttackAction (this);
		actionTable [Action.Type.Defend] = new DefendAction (this);

		AttackArea aa = GetComponentInChildren<AttackArea> ();
		aa.SetAttacker(this);
		aa.SetAttack(new SimpleAttack(20));
	}

	protected void Defend(bool isTrigger){
		if(isTrigger){
			if(isGrounded)
				actionTable [Action.Type.Defend].TriggerAction ();
		}
		else if(isDefending() && !isTrigger){
			actionTable [Action.Type.Defend].CancelAction();
		}
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
		Defend(true);
	}
	public override void releaseL(){
		Defend(false);
	}

	protected bool isDefending(){
		return currentAction is DefendAction;
	}
}
