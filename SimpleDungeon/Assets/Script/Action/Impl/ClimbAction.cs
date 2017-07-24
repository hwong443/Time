﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbAction : Action {

	private Collider2D colli;
	private ClimbArea climbArea;
	private int direction;

	// default
	public ClimbAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 0f;
		this.type = Type.Climb;
		this.actionDuration = 1f;
		this.isCancelable = true;
		this.isRepeatable = true;
		this.isSpeedChangable = true;
		this.animationName = "Character_Walk";

		//===== new attribute here =======
		colli = owner.transform.Find ("ClimbArea").GetComponent<Collider2D> ();
		climbArea = colli.GetComponent<ClimbArea> ();

		Init();
	}

	public void SetDirection(Enum.Direction d){
		direction = (int)d;
	}

	public override void StartEffect(){
		//colli.enabled = true;
		owner.isClimbing = true;
		foreach(Collider2D collider in owner.ignoreColliderMap.Keys){
			Physics2D.IgnoreCollision (collider, owner.colli);
		}
	}

	public override void UpdateEffect(){
		if (climbArea.climbingObjects > 0 && owner.isGrounded) {
			owner.rb.gravityScale = 0;
			//owner.rb.bodyType = RigidbodyType2D.Kinematic;
			owner.rb.velocity = new Vector2 (owner.rb.velocity.x, 200*Time.deltaTime*direction);
		} 
		else {
			isActionDone = true;
		}
	}

	public override void EndEffect(){
		//colli.enabled = false;
		//climbArea.climbingObjects = 0;
		owner.isClimbing = false;
		//owner.rb.velocity = new Vector2 (owner.rb.velocity.x, 0);
		owner.rb.gravityScale = 5;

		foreach(Collider2D collider in owner.ignoreColliderMap.Keys){
			Physics2D.IgnoreCollision (collider, owner.colli, false);
		}
	}

	protected override void SetActionConstrain(){
		owner.canWalk = false;
	}

	public override bool SwitchActionCheck(Action.Type type){
		isActionDone = true;
		SetTriggerNextAction(false);
		StopAction ();
		Debug.Log ("trigger normal action");
		return true;
	}
}
