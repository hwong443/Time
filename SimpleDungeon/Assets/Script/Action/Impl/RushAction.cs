using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAction : Action {

	private int direction;

	// default
	public RushAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 1f;
		this.type = Type.Rush;
		this.isCancelable = true;
		this.isRepeatable = false;
		this.isSpeedChangable = true;
		this.animationName = "Character_Walk";

		//===== new attribute here =======

		Init();

		this.actionDuration = 0.15f;
	}

	public void SetDirection(Enum.Direction d){
		direction = (int)d;
	}

	public override void StartEffect(){
		owner.passiveMoveDir = direction;
	}

	public override void UpdateEffect(){
		owner.passiveMoveSpeed = 1800 * Time.deltaTime;
	}

	public override void EndEffect(){
		owner.passiveMoveDir = 0;
	}

	protected override void SetActionConstrain(){
		
		owner.canJump = false;
		owner.canWalk = false;
		owner.canTurn = false;
	}

	public override bool SwitchActionCheck(Action.Type type){
		if (type == Action.Type.Attack) {
			//isActionDone = true;
			SetTriggerNextAction (false);
			StopAction ();
			return true;
		}
		return false;
	}
}
