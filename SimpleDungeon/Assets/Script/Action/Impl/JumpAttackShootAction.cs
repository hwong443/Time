using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpShootAttackAction : Action {

	private Sprite sprite;
	private bool trigger;

	// default
	public JumpShootAttackAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 0f;
		this.type = Type.JumpAttack;
		this.actionDuration = 1f;
		this.isCancelable = true;
		this.isRepeatable = true;
		this.isSpeedChangable = true;
		this.animationName = owner.GetClassName()+"_JumpAttack";

		//===== new attribute here =======

		Init();
	}

	public void SetSprite(string spriteName){
		sprite =  Resources.Load(spriteName, typeof(Sprite)) as Sprite;
		Debug.Log(spriteName);
		Debug.Log(sprite);
	}

	public override void StartEffect(){
		trigger = true;
	}

	public override void UpdateEffect(){
		if(owner.isGrounded){
			isActionDone = true;
			trigger = false;
		}
	}

	public override void EndEffect(){
		if(trigger){
			GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/ProjectionAttackArea")) as GameObject;
			ProjectingAttackArea arrow = obj.GetComponent<ProjectingAttackArea> ();
			arrow.Init();
			arrow.SetSprite(sprite);
			arrow.SetForce(1000*owner.faceDir);
			arrow.SetAttacker(owner);
			arrow.SetAttack(new SimpleAttack(15));
			Vector3 ownerPosi = owner.transform.position;
			arrow.transform.position = new Vector3(
				ownerPosi.x + owner.faceDir*owner.GetSpriteRenderer().bounds.extents.x, 
				ownerPosi.y, 
				arrow.transform.position.z);
		}

	}

	protected override void SetActionConstrain(){
	}

	public override bool SwitchActionCheck(Action.Type type){
		isActionDone = true;
		SetTriggerNextAction(false);
		StopAction ();
		return true;
	}
}
