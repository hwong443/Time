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
		this.isCancelable = false;
		this.isRepeatable = false;
		this.isSpeedChangable = true;
		this.animationName = owner.GetClassName()+"_Attack";

		//===== new attribute here =======
		colli = owner.transform.Find ("AttackArea").GetComponent<Collider2D> ();

		Init();
	}

	public override void StartEffect(){
		colli.enabled = true;
		colli.transform.localScale = new Vector3(1, 1, 1);
	}

	public override void UpdateEffect(){
	}

	public override void EndEffect(){
		colli.enabled = false;
		colli.transform.localScale = new Vector3(0, 0, 0);
	}

	protected override void SetActionConstrain(){
		owner.canJump = false;
		owner.canTurn = false;
	}

	public override bool SwitchActionCheck(Action.Type type){
		if(Action.Type.Damage.Equals(type)){	
			SetTriggerNextAction(false);
			StopAction ();
			Debug.Log ("trigger normal action");
			return true;
		}
		return false;
	}
}
