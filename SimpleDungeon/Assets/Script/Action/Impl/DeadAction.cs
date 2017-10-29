using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAction : Action {

	private Collider2D colli;
	public float destoryTime;

	// default
	public DeadAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 0f;
		this.type = Type.Dead;
		this.isCancelable = true;
		this.isRepeatable = false;
		this.isSpeedChangable = true;
		this.animationName = owner.GetClassName()+"_Dead";

		this.destoryTime = 5000;

		Init();
	}


	public override void StartEffect(){
		Debug.Log("StartEffect");
	}

	public override void UpdateEffect(){
		
		Debug.Log("UpdateEffect");
	}

	public override void EndEffect(){
		Debug.Log(destoryTime);
		owner.DestoryAfterMS(destoryTime);
	}

	protected override void SetActionConstrain(){
		owner.canJump = false;
		owner.canWalk = false;
		owner.canTurn = false;
	}

	public override bool SwitchActionCheck(Action.Type type){
		isActionDone = true;
		SetTriggerNextAction(false);
		StopAction ();
		return true;
	}
}
