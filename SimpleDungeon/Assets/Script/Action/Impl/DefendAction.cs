using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendAction : Action {

	private Sprite sprite;
	private GameObject shield;

	// default
	public DefendAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 0f;
		this.type = Type.Defend;
		this.isCancelable = true;
		this.isRepeatable = true;
		this.isSpeedChangable = true;
		this.animationName = owner.GetClassName()+"_Skill";

		//===== new attribute here =======
	
		Init();
	}

	public void SetSprite(string spriteName){
		sprite =  Resources.Load(spriteName, typeof(Sprite)) as Sprite;
	}

	public override void StartEffect(){
		if(shield == null){
			shield = GameObject.Instantiate(Resources.Load("Prefabs/Shield")) as GameObject;
			Vector3 ownerPosi = owner.transform.position;
			shield.transform.position = new Vector3(
				(shield.transform.position.x + ownerPosi.x) * owner.faceDir, 
				shield.transform.position.y + ownerPosi.y, 
				shield.transform.position.z);
			shield.transform.parent = owner.transform;
		}

		shield.SetActive(true);
	}
	public override void UpdateEffect(){
		// update + postion 
		//aim.transform.position = new Vector3(goalX, goalY, aim.transform.position.z);
	}

	public override void EndEffect(){
		shield.SetActive(false);
	}

	protected override void SetActionConstrain(){
		owner.canJump = false;
		owner.canWalk = false;
	}

	public override bool SwitchActionCheck(Action.Type type){
		SetTriggerNextAction(false);
		StopAction ();
		return true;
	}
}
