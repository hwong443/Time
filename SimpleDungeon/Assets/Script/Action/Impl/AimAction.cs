using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAction : Action {

	private Sprite sprite;
	private float goalX;
	private float goalY;
	private bool trigger;
	private GameObject aim;

	// default
	public AimAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 0f;
		this.type = Type.Aim;
		this.actionDuration = 1f;
		this.isCancelable = true;
		this.isRepeatable = true;
		this.isSpeedChangable = true;
		this.animationName = owner.GetClassName()+"_Skill";

		//===== new attribute here =======
		trigger = false;
		goalX = 0.0f;
		goalY = 0.0f;
	
		Init();
	}

	public void SetSprite(string spriteName){
		sprite =  Resources.Load(spriteName, typeof(Sprite)) as Sprite;
		Debug.Log(spriteName);
		Debug.Log(sprite);
	}
	public void SetGoalX(float goalX){
		this.goalX += goalX;
	}
	public void SetGoalY(float goalY){
		this.goalY += goalY;
	}
	public float GetGoalX(){
		return this.goalX;
	}
	public float GetGoalY(){
		return this.goalY;
	}
	public void Trigger(bool trigger){
		this.trigger = trigger;
	}
	public override void StartEffect(){
		aim = GameObject.Instantiate(Resources.Load("Prefabs/AimArea")) as GameObject;
		Vector3 ownerPosi = owner.transform.position;
		trigger = false;
		goalX = ownerPosi.x;// + owner.faceDir*owner.sr.bounds.extents.x;
		goalY = ownerPosi.y;
		aim.transform.position = new Vector3(goalX, goalY, aim.transform.position.z);
	}
	public override void UpdateEffect(){
		// update + postion 
		aim.transform.position = new Vector3(goalX, goalY, aim.transform.position.z);
	}

	public override void EndEffect(){
		if(trigger){
			GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/EndPointAttackArea")) as GameObject;
			EndPointAttackArea arrow = obj.GetComponent<EndPointAttackArea> ();
			arrow.Init();
			Vector3 ownerPosi = owner.transform.position;
			arrow.transform.position = new Vector3(
				ownerPosi.x + owner.faceDir*owner.GetSpriteRenderer().bounds.extents.x, 
				ownerPosi.y, 
				arrow.transform.position.z);

			arrow.SetSprite(sprite);
			arrow.SetForce(World.ARROW_SPEED);
			arrow.SetAttacker(owner);
			arrow.SetAttack(new SimpleAttack(15));
			arrow.SetGoalX(goalX);
			arrow.SetGoalY(goalY);


			//arrow.transform.position = new Vector3(goalX, goalY, arrow.transform.position.z);
		}
		GameObject.Destroy(aim);
	}

	protected override void SetActionConstrain(){
		owner.canJump = false;
		owner.canWalk = false;
	}

	public override bool SwitchActionCheck(Action.Type type){
		isActionDone = true;
		SetTriggerNextAction(false);
		StopAction ();
		return true;
	}
}
