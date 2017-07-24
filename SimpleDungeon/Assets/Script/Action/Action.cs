using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action{
	public enum Type{
		Attack, JumpAttack, Defend, Damage, Climb, Rush
	}

	protected Character owner;

	protected float timeout;
	protected float actionDuration;
	protected List<float> animationPart;
	protected int currentAnimationPart;
	protected float animationDuration;
	protected float playSpeed;
	protected float playTimePass;
	protected float lastCalledTime;
	protected Action nextAction;
	protected bool triggerNextAction;

	protected bool isCancelable;
	protected bool isRepeatable;
	protected bool isSpeedChangable;
	protected bool isActionDone;

	protected string animationName;
	protected Type type;

	// part 1 init
	protected Action(Character owner){
		this.owner = owner;
		playTimePass = 0f;
		lastCalledTime = 0f;
		isActionDone = false;
		triggerNextAction = false;
		actionDuration = 1f;
		animationPart = new List<float> ();
		currentAnimationPart = 0;
		playSpeed = 1;
	}

	// part 2 init
	protected void Init(){		
		animationDuration = owner.GetAnimationClipTable (animationName).length;
		animationPart.Add(animationDuration);

		// if play one time, sync with actural animation duration
		actionDuration = (isRepeatable)? Mathf.Infinity: animationDuration/playSpeed;

	}

	public Type GetType(){
		return type;
	}



	// ===== animation action =============
	// use custom animation time
	public void SetPlayerUntilAnimationTime(float time){
		// repeatable animation cannot set animation end frame 
		if (!isRepeatable && time >= 0) {
			actionDuration = time/playSpeed;
		} 
		else {
			Debug.Log ("Invalid Time: "+time);
			Debug.Log ("isRepeatable: "+isRepeatable);
		}
	}
	// use default animation time
	public void SetPlayerUntilAnimationPart(int part){
		// repeatable animation cannot set animation end frame 
		if (!isRepeatable && (part >= 0 || part>=animationPart.Count)) {
			currentAnimationPart = part;
			actionDuration = animationPart[currentAnimationPart]/playSpeed;
		} 
		else {
			Debug.Log ("Invalid Part: "+part);
			Debug.Log ("isRepeatable: "+isRepeatable);
		}
	}
	public void SetPlaySpeedBySpeed(float speedFactor){
		if (isSpeedChangable) {
			playSpeed = speedFactor;
			actionDuration = animationDuration / playSpeed;
			Debug.Log ("playSpeed: " + playSpeed);

			if (owner.GetCurrentAction () == this)
				owner.SetCurrentAnimationSpeed (playSpeed);
		} 
		else {
			Debug.Log ("isSpeedChangable: "+isSpeedChangable);
		}
	}
	public void SetPlaySpeedByDuration(float duraction){
		if (isSpeedChangable) {
			actionDuration = duraction;
			playSpeed = animationDuration/actionDuration;
			Debug.Log ("playSpeed: "+playSpeed);

			if (owner.GetCurrentAction () == this)
				owner.SetCurrentAnimationSpeed (playSpeed);
		}
		else {
			Debug.Log ("isSpeedChangable: "+isSpeedChangable);
		}
	}
	// ===== animation action =============



	// ===== main action function =============
	public bool TriggerAction(){

		Action currentAction = owner.GetCurrentAction();

		if (Time.time - lastCalledTime >= timeout) {

			// start new action
			if (currentAction == null) {
				StartAction ();
				return true;
			}
			// switch from old action
			else if(currentAction.SwitchAction (this.type)){
				currentAction.StopAction ();
				StartAction ();
				return true;
			}
		} 
		else {
			Debug.Log ("timeout passed: " + (Time.time - lastCalledTime));
			Debug.Log ("timeout: " + timeout);
		}
		return false;
	}
	public void UpdateAction(){
		if(!isActionDone){
			// one time action
			if(!isRepeatable){
				playTimePass += Time.deltaTime;
				if(playTimePass>=actionDuration){
					isActionDone = true;
					StopAction();
				}
			}

			UpdateEffect();
		}
		else{
			StopAction();
			//Debug.Log("Action ["+animationName+"] is not working.");
		}
	}
	public bool CancelAction(){
		if (isActionDone || isCancelable) {
			isActionDone = true;
			StopAction ();
			return true;
		}
		return false;
	}
	public bool SwitchAction(Action.Type type){
		// can cancel action
		if (isActionDone || isCancelable) {
			isActionDone = true;
			return true;
		} 

		// can override action
		bool isNextAction = (triggerNextAction && nextAction != null && type == nextAction.GetType ());
		if (isNextAction) {
			isActionDone = true;
			return true;
		}
		else
			return SwitchActionCheck (type);
	}
	protected void StartAction(){
		// set action
		isActionDone = false;
		playTimePass = 0f;
		owner.SetCurrentAction (this.type);

		// set animation
		owner.SetCurrentAnimation (animationName);
		owner.SetCurrentAnimationSpeed (playSpeed);
		owner.PlayCurrentAnimation ();

		SetActionConstrain ();
		StartEffect ();
	}
	protected void StopAction(){
		EndEffect();

		isActionDone = true;
		// set action to null
		owner.ReleaseCurrentAction();
		owner.SetCurrentAnimation();
		ResetActionConstain();

		// is single animation then play full clip, else play the first part
		currentAnimationPart = (animationPart.Count > 1) ? 1 : 0;
		lastCalledTime = Time.time;

		TriggerNextAction ();
		SetTriggerNextAction (false);
	}	
	// ===== main action function =============



	// ===== next action =============
	public void SetNextAction(Action.Type nextActionType){
		nextAction = owner.GetActionByType(nextActionType);
	}

	public void ReleaseNextAction(){
		nextAction = null;
	}

	protected void TriggerNextAction(){
		if (triggerNextAction && nextAction != null) {
			Debug.Log ("TriggerNextAction");
			nextAction.TriggerAction ();
		}
	}

	public void SetTriggerNextAction(bool trigger){
		triggerNextAction = trigger;
	}
	// ===== next action =============

	protected void ResetActionConstain(){
		owner.canJump = true;
		owner.canWalk = true;
		owner.canTurn = true;
	}

	protected virtual void SetActionConstrain (){}
	public virtual void StartEffect(){}
	public virtual void UpdateEffect(){}
	public virtual void EndEffect(){}


	public virtual bool SwitchActionCheck(Action.Type type){
		isActionDone = true;
		SetTriggerNextAction(false);
		StopAction ();
		Debug.Log ("trigger normal action");
		return true;
	}
}
