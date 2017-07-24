using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackAction : Action {

	private Collider2D colli;

	// default
	public JumpAttackAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 3f;
		this.type = Type.JumpAttack;
		this.actionDuration = 1f;
		this.isCancelable = false;
		this.isRepeatable = false;
		this.isSpeedChangable = true;
		this.animationName = "Character_JumpAttack";

		//===== new attribute here =======
		colli = owner.transform.Find ("AttackArea").GetComponent<Collider2D> ();

		Init();
	}

	public override void StartEffect(){
		Debug.Log ("StartEffect");
		colli.enabled = true;
	}

	public override void UpdateEffect(){

	}

	public override void EndEffect(){
		Debug.Log ("EndEffect");
		colli.enabled = false;
	}

	protected override void SetActionConstrain(){
		Debug.Log ("SetActionConstrain");

		owner.canWalk = false;
		owner.canTurn = false;
	}

	public override bool SwitchActionCheck(Action.Type type){
		isActionDone = true;
		SetTriggerNextAction(false);
		StopAction ();
		Debug.Log ("trigger normal action");
		return true;
	}
}
