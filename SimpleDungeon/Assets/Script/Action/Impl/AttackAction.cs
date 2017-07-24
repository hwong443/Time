using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action {

	private Collider2D colli;

	// default
	public AttackAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 0f;
		this.type = Type.Attack;
		this.actionDuration = 1f;
		this.isCancelable = false;
		this.isRepeatable = false;
		this.isSpeedChangable = true;
		this.animationName = "Character_Attack";

		//===== new attribute here =======
		colli = owner.transform.Find ("AttackArea").GetComponent<Collider2D> ();
		Debug.Log (colli.transform.name);

		Init();
	}

	public override void StartEffect(){
		Debug.Log ("StartEffect");
		colli.enabled = true;
		colli.transform.localScale = new Vector3(1, 1, 1);
	}

	public override void UpdateEffect(){

	}

	public override void EndEffect(){
		Debug.Log ("EndEffect");
		colli.enabled = false;
		colli.transform.localScale = new Vector3(0, 0, 0);
	}

	protected override void SetActionConstrain(){
		Debug.Log ("SetActionConstrain");

		owner.canJump = false;
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
