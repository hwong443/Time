using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : Action {

	private Collider2D colli;

	// default
	public DamageAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 1f;
		this.type = Type.Damage;
		this.actionDuration = 0.3f;
		this.isCancelable = true;
		this.isRepeatable = false;
		this.isSpeedChangable = true;
		this.animationName = "Character_Hurt";

		//===== new attribute here =======
		//colli = owner.transform.Find ("Attack").GetComponent<Collider2D> ();
		//Debug.Log (colli.transform.name);

		Init();
	}


	public override void StartEffect(){
		// deal damage
	}

	public override void UpdateEffect(){

	}

	public override void EndEffect(){
		//colli.enabled = false;
	}

	protected override void SetActionConstrain(){
		Debug.Log ("SetActionConstrain");

		owner.canJump = false;
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
