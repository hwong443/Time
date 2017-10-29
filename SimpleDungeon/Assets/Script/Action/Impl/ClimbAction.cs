using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbAction : Action {

	private Collider2D colliUp;
	private ClimbArea climbAreaUp;
    private Collider2D colliDown;
    private ClimbArea climbAreaDown;
    private Collider2D colli;
    private ClimbArea climbArea;
    private int direction;

	// default
	public ClimbAction(Character owner): base(owner){
		//this(owner, Type.Attack, 1f, 1f, false, false, true, "Character_Attack");

		this.timeout = 0f;
		this.type = Type.Climb;
		this.isCancelable = true;
		this.isRepeatable = true;
		this.isSpeedChangable = true;
		this.animationName = owner.GetClassName()+"_Walk";

		//===== new attribute here =======
		colliUp = owner.transform.Find ("ClimbAreaUp").GetComponent<Collider2D> ();
		climbAreaUp = colliUp.GetComponent<ClimbArea> ();

        colliDown = owner.transform.Find("ClimbAreaDown").GetComponent<Collider2D>();
        climbAreaDown = colliDown.GetComponent<ClimbArea>();

		colli = colliUp;
		climbArea = climbAreaUp;

        Init();
	}

    public bool ClimbableFound() {
        return climbArea.climbingObjects > 0;
    }

	public void SetDirection(World.Direction d){
		direction = (int)d;
        if (d == World.Direction.UP) {
            colli = colliUp;
            climbArea = climbAreaUp;
        }
        else if (d == World.Direction.DOWN) {
            colli = colliDown;
            climbArea = climbAreaDown;
        }
	}

	public override void StartEffect(){
		//colli.enabled = true;
		owner.isClimbing = true;
		foreach(Collider2D collider in owner.GetIgnoreColliderMap().Keys){
			Physics2D.IgnoreCollision (collider, owner.GetCollider());
		}
	}

	public override void UpdateEffect(){
		if (climbArea.climbingObjects > 0 && owner.isGrounded) {
			owner.GetRigidbody2D().gravityScale = 0;
			//owner.rb.bodyType = RigidbodyType2D.Kinematic;
			owner.GetRigidbody2D().velocity = new Vector2 (owner.GetRigidbody2D().velocity.x, 200*Time.deltaTime*direction);
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
		owner.GetRigidbody2D().gravityScale = 5;

		foreach(Collider2D collider in owner.GetIgnoreColliderMap().Keys){
			Physics2D.IgnoreCollision (collider, owner.GetCollider(), false);
		}
	}

	protected override void SetActionConstrain(){
		//owner.canWalk = false;
	}

	public override bool SwitchActionCheck(Action.Type type){
		SetTriggerNextAction(false);
		StopAction ();
		Debug.Log ("trigger normal action");
		return true;
	}
}
