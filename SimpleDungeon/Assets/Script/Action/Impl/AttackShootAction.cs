using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShootAction : Action {

	private Sprite sprite;

	// default
	public AttackShootAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 0f;
		this.type = Type.Attack;
		this.actionDuration = 1f;
		this.isCancelable = false;
		this.isRepeatable = false;
		this.isSpeedChangable = true;
		this.animationName = "Character_Attack";

		//===== new attribute here =======

		Init();
	}

	public void SetSprite(string spriteName){
		sprite =  Resources.Load(spriteName, typeof(Sprite)) as Sprite;
		Debug.Log(spriteName);
		Debug.Log(sprite);
	}

	public override void StartEffect(){
		Debug.Log ("StartEffect");

		GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/EndPointAttackArea")) as GameObject;
		EndPointAttackArea arrow = obj.GetComponent<EndPointAttackArea> ();
		arrow.Init();
		arrow.SetSprite(sprite);
		arrow.SetForce(1000*owner.faceDir);
		arrow.SetAttacker(owner);
		arrow.SetAttack(new ShortWeapon(15));
		Vector3 ownerPosi = owner.transform.position;
		arrow.transform.position = new Vector3(ownerPosi.x + owner.faceDir*owner.sr.bounds.extents.x, ownerPosi.y, arrow.transform.position.z);


		/*
		GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/NonProjectionAttackArea")) as GameObject;
		NonProjectingAttackArea arrow = obj.GetComponent<NonProjectingAttackArea> ();
		arrow.Init();
		arrow.SetSprite(sprite);
		arrow.SetForce(1000*owner.faceDir);
		arrow.SetAttacker(owner);
		arrow.SetAttack(new ShortWeapon(15));
		Vector3 ownerPosi = owner.transform.position;
		arrow.transform.position = new Vector3(ownerPosi.x + owner.faceDir*owner.sr.bounds.extents.x, ownerPosi.y, arrow.transform.position.z);
		*/

		/*
		GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/ProjectionAttackArea")) as GameObject;
		ProjectingAttackArea arrow = obj.GetComponent<ProjectingAttackArea> ();
		arrow.Init();
		arrow.SetSprite(sprite);
		arrow.SetForce(1000*owner.faceDir);
		arrow.SetAttacker(owner);
		arrow.SetAttack(new ShortWeapon(15));
		Vector3 ownerPosi = owner.transform.position;
		arrow.transform.position = new Vector3(ownerPosi.x + owner.faceDir*owner.sr.bounds.extents.x, ownerPosi.y, arrow.transform.position.z);
		*/
		
	}

	public override void UpdateEffect(){

	}

	public override void EndEffect(){
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
