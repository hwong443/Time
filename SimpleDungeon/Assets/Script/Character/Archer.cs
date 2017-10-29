using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character {
	protected float aimMoveSpeedX = 0.0f;
	protected float aimMoveSpeedY = 0.0f;

	protected void Start(){
		base.Start();
		actionTable [Action.Type.Attack] = new AttackShootAction (this);
		((AttackShootAction)actionTable [Action.Type.Attack]).SetSprite("Graphic/arrow");

		actionTable [Action.Type.JumpAttack] = new JumpShootAttackAction (this);
		((JumpShootAttackAction)actionTable [Action.Type.JumpAttack]).SetSprite("Graphic/arrow");

		actionTable [Action.Type.Aim] = new AimAction (this);
		((AimAction)actionTable [Action.Type.Aim]).SetSprite("Graphic/arrow");
	}

	protected override void Update(){
		if (!info.IsDead ()) {
			UpdateState ();
			UpdateMove ();
			UpdateAim();
			UpdateAnimationState ();
		}
		
		if(currentAction != null)
			currentAction.UpdateAction ();
	}

	// ==== Update ==============
	protected void UpdateAim(){
		if(isAiming()){
			((AimAction)currentAction).SetGoalX(aimMoveSpeedX * Time.deltaTime);
			((AimAction)currentAction).SetGoalY(aimMoveSpeedY * Time.deltaTime);

			// different direction
			float goalX = ((AimAction)currentAction).GetGoalX() - transform.position.x;

			if(faceDir*goalX < 0){
				if(faceDir == (int)World.Direction.LEFT)
					Turn(World.Direction.RIGHT);
				else
					Turn(World.Direction.LEFT);
			}
		}
	}


	protected void Aim(bool isTrigger){
		if(isTrigger){
			actionTable [Action.Type.Aim].TriggerAction();
		}
		else if(isAiming() && !isTrigger){
			((AimAction)actionTable [Action.Type.Aim]).Trigger(true);
			actionTable [Action.Type.Aim].CancelAction();
		}
	}

	protected void MoveAimX(World.Direction x){
		aimMoveSpeedX = info.moveSpeed * (int)x;
	}
	protected void MoveAimY(World.Direction y){
		aimMoveSpeedY = info.moveSpeed * (int)y;
	}

	// ==== receiver =============
	public override void pressQ(){

	}
	public override void releaseQ(){

	}
	public override void pressE(){

	}
	public override void releaseE(){

	}

	public override void pressW(){
		if(isAiming()){
			MoveAimY(World.Direction.UP);
		}
		else{
			base.pressW();
		}
	}
	public override void releaseW(){
		if(isAiming()){
			MoveAimY(World.Direction.STOP);
		}
		else{
			base.releaseW();
		}
	}
	public override void pressS(){
		if(isAiming()){
			MoveAimY(World.Direction.DOWN);
		}
		else{
			base.pressS();
		}
	}
	public override void releaseS(){
		if(isAiming()){
			MoveAimY(World.Direction.STOP);
		}
		else{
			base.releaseS();
		}
	}
	public override void pressA(){
		if(isAiming()){
			MoveAimX(World.Direction.LEFT);
		}
		else{
			base.pressA();
		}
	}
	public override void releaseA(){
		if(isAiming()){
			MoveAimX(World.Direction.STOP);
		}
		else{
			base.releaseA();
		}
	}
	public override void pressD(){
		if(isAiming()){
			MoveAimX(World.Direction.RIGHT);
		}
		else{
			base.pressD();
		}
	}
	public override void releaseD(){
		if(isAiming()){
			MoveAimX(World.Direction.STOP);
		}
		else{
			base.releaseD();
		}
	}
	public override void pressL(){
		Aim(true);
	}
	public override void releaseL(){
		Move(World.Direction.STOP);
		Aim(false);
	}

	protected bool isAiming(){
		return currentAction is AimAction;
	}
}
