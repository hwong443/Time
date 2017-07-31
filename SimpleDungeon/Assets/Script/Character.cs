using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Character : MonoBehaviour, Receiver, Interactable, Attackable, Damagable {

	public float moveSpeed = 0.0f;
	public float passiveMoveSpeed = 0.0f;
	public int faceDir = 1;
	public int passiveMoveDir = 0;
	public int jumpCount = 0;

	public bool isClimbing = false;
	public bool isGrounded = true;
	public bool isMoving = false;
	public bool canWalk = true;
	public bool canJump = true;
	public bool canDoubleJump = false;
	public bool canTurn = true;

	private Action currentAction;
	private string currentAnimation;

	public Rigidbody2D rb;
	public SpriteRenderer sr;
	public Collider2D colli;
	public Animator anim;
	public CharacterInfo info;

	public Dictionary<Collider2D, int> ignoreColliderMap = new Dictionary<Collider2D, int>();

	public float halfWidth;

	Dictionary<Action.Type, Action> actionTable = new Dictionary<Action.Type, Action>();
	Dictionary<string, AnimationClip> animationClipTable = new Dictionary<string, AnimationClip>();


	// Use this for initialization
	protected void Start () {

		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		colli = GetComponent<Collider2D> ();
		halfWidth = colli.bounds.size.x / 2;

		// build animations
		anim = GetComponent<Animator> ();
		AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
		foreach(AnimationClip clip in clips){
			animationClipTable [clip.name] = clip;
		}

		// build actions
		//actionTable [Action.Type.Attack] = new AttackAction (this);
		actionTable [Action.Type.Attack] = new AttackShootAction (this);
		((AttackShootAction)actionTable [Action.Type.Attack]).SetSprite("Graphic/arrow");
		//actionTable [Action.Type.JumpAttack] = new JumpAttackAction (this);
		actionTable [Action.Type.JumpAttack] = new JumpShootAttackAction (this);
		((JumpShootAttackAction)actionTable [Action.Type.JumpAttack]).SetSprite("Graphic/arrow");
		actionTable [Action.Type.Defend] = new RushAction (this);
		actionTable [Action.Type.Defend].SetNextAction (Action.Type.Attack);
		actionTable [Action.Type.Damage] = new DamageAction (this);
		actionTable [Action.Type.Climb] = new ClimbAction (this);
		actionTable [Action.Type.Rush] = new RushAction (this);
		// wall jump
		// edge + jump -> climb up
		// combo
		// pick up

		//actionTable [Action.Type.Attack].SetPlaySpeedByDuration (1f);

		AttackArea aa = GetComponentInChildren<AttackArea> ();
		aa.attacker = this;
		aa.attack = new ShortWeapon (10);

		/*
		GameObject attack = Instantiate(Resources.Load("Prefabs/Attack")) as GameObject;
		attack.transform.parent = transform;
		attack.name = "Attack";

		attackColli = attack.GetComponent<Collider2D> ();
		*/	
        

		info = GetComponent<CharacterInfo> ();
		info.owner = this;
	}

	// Update is called once per frame
	protected void Update () {
		if (info.IsDead ()) {
			return;
		}
		
		UpdateState ();
		UpdateMove ();
		UpdateAnimationState ();
		if(currentAction != null)
			currentAction.UpdateAction ();
	}

	protected void UpdateMove(){
		float activeMove = (canWalk && isMoving) ? moveSpeed * faceDir : 0;
		float passiveMove = passiveMoveSpeed * passiveMoveDir;
		rb.velocity = new Vector2 (activeMove + passiveMove,rb.velocity.y);
	}

	protected void UpdateState(){
		isGrounded = isClimbing? true: rb.velocity.y==0;
		if (isGrounded) {
			jumpCount = 0;
			canDoubleJump = false;
		}

		if(!isGrounded && jumpCount == 1){
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (faceDir==(int)Enum.Direction.LEFT)? Vector2.left: Vector2.right);
			if (hit.collider != null && hit.distance<=Mathf.Ceil(halfWidth) && hit.collider.tag == "Solid")
				canDoubleJump = true;
			
		}
	}

	// ==== control ==============
	public void Move(Enum.Direction d){
		
		if (Enum.Direction.STOP == d) {
			moveSpeed = 0;
			isMoving = false;
		} 
		// if cannot turn, then cannot move to another direction
		else if (!canTurn && faceDir != (int)d){
			moveSpeed = 0;
			isMoving = false;
		}
		else {
			moveSpeed = info.moveSpeed;
			isMoving = true;
		}
	}
	public void Turn(Enum.Direction d){
		if (canTurn && faceDir != (int)d) {
			faceDir = (int)d;
			Vector3 newScale = transform.localScale;
			newScale.x = -faceDir * Mathf.Abs (newScale.x);
			transform.localScale = newScale;
		}
	}
	public void Jump(){
		isClimbing = false;
		isGrounded = false;
		rb.velocity = new Vector2 (rb.velocity.x, info.jumpSpeed);
		++jumpCount;
	}

	public void Rush(Enum.Direction d){
		// if d != faceDir => doge (not move, just skip attack)
		// if d == faceDir => rush
		Debug.Log ("Rush");
		((RushAction)actionTable [Action.Type.Rush]).SetDirection (d);
		actionTable [Action.Type.Rush].TriggerAction ();
	}
	public void Attack(){
		Debug.Log ("Attack");
		actionTable [Action.Type.Attack].TriggerAction ();
	}

	private void JumpAttack(){
		Debug.Log ("Jump Attack");
		actionTable [Action.Type.JumpAttack].TriggerAction ();
	}

	public void Defend(){
		Debug.Log ("Defend");
		//actionTable [Action.Type.Defend].SetTriggerNextAction(true);
		actionTable [Action.Type.Defend].TriggerAction ();
	}

	public void Climb(Enum.Direction d){
		if(!isClimbing)
			actionTable [Action.Type.Climb].TriggerAction ();
	}

	public void Interact(Interactable target){
		
	}
	public void React (){
		
	}
	// ==== passive action =======
	public void TakeDamage(Attack attack){
		//info.Damage (attack.GetDamage());
		Debug.Log ("Damage");
		float time = 2f;
		actionTable [Action.Type.Damage].SetPlayerUntilAnimationTime(time);
		actionTable [Action.Type.Damage].TriggerAction ();
	}
	public void Die(){

	}

	// ==== animation ==============
	void UpdateAnimationState(){
		if(currentAction == null){

			string newAnimationName = "";

			if(!isGrounded){
				newAnimationName = "Character_Jump";
			}
			else if(isMoving){
				newAnimationName = "Character_Walk";
			}
			else{
				newAnimationName = "Character_Idel";
			}

			// start new animation
			if (!IsSameAnimation(newAnimationName, currentAnimation)) {
				anim.speed = 1;
				SetCurrentAnimation (newAnimationName);
				PlayCurrentAnimation ();
			}
		}
	}

	public void SetCurrentAnimation(){
		currentAnimation = "";
	}

	public void SetCurrentAnimation(string animationName){
		currentAnimation = animationName;
	}

	public void SetCurrentAnimationSpeed(float speed){
		anim.speed = speed;
	}

	public void PlayCurrentAnimation(){
		anim.Play(currentAnimation, -1, 0);
	}

	public AnimationClip GetAnimationClipTable(string animationName){
		return (animationClipTable.ContainsKey (animationName)) ? animationClipTable [animationName] : null;
	}

	public bool IsSameAnimation(string animation1, string animation2){
		if (animation1 == null || animation2 == null)
			return false;
		// remove _123 at the end
		Regex rgx = new Regex(@"_?\d+$");
		animation1 = rgx.Replace(animation1, "");
		animation2 = rgx.Replace(animation2, "");
		return animation1 == animation2;
	}

	// ==== action ==============
	public void SetCurrentAction(Action.Type actionType){
		currentAction = actionTable[actionType];
	}

	public void ReleaseCurrentAction(){
		currentAction = null;
	}

	public Action GetCurrentAction(){
		return currentAction;
	}

	public Action GetActionByType(Action.Type actionType){
		return actionTable[actionType];
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (isClimbing && coll.gameObject.tag == "Solid" && !Physics2D.GetIgnoreCollision (coll.collider, colli)) {
			Debug.Log ("add "+coll.gameObject.name);
			Physics2D.IgnoreCollision (coll.collider, colli);
			ignoreColliderMap[coll.collider]=0;
		} 
	}

	// ==== receiver =============
	public void pressQ(){
		
	}
	public void releaseQ(){

	}
	public void pressE(){

	}
	public void releaseE(){

	}

	public void pressW(){
		((ClimbAction)actionTable [Action.Type.Climb]).SetDirection (Enum.Direction.UP);
		if(((ClimbAction)actionTable[Action.Type.Climb]).ClimbableFound()){
			Climb(Enum.Direction.UP);
		}
	}
	public void releaseW(){
		((ClimbAction)actionTable [Action.Type.Climb]).SetDirection (Enum.Direction.STOP);
		if(((ClimbAction)actionTable[Action.Type.Climb]).ClimbableFound()){
			Climb(Enum.Direction.STOP);
		}
	}
	public void pressS(){
		((ClimbAction)actionTable [Action.Type.Climb]).SetDirection (Enum.Direction.DOWN);
		if(((ClimbAction)actionTable[Action.Type.Climb]).ClimbableFound()){
			Climb(Enum.Direction.DOWN);
		}
	}
	public void releaseS(){
		((ClimbAction)actionTable [Action.Type.Climb]).SetDirection (Enum.Direction.STOP);
		if(((ClimbAction)actionTable[Action.Type.Climb]).ClimbableFound()){
			Climb(Enum.Direction.STOP);
		}
	}
	public void pressA(){
		Move(Enum.Direction.LEFT);
		Turn(Enum.Direction.LEFT);
	}
	public void releaseA(){
		Move(Enum.Direction.STOP);
	}
	public void pressD(){
		Move(Enum.Direction.RIGHT);
		Turn(Enum.Direction.RIGHT);
	}
	public void releaseD(){
		Move(Enum.Direction.STOP);
	}

	public void pressJ(){
		if (isGrounded) {
			Attack();
		}
		else
			JumpAttack ();
	}
	public void releaseJ(){

	}
	public void pressK(){
		if (canJump) {
			if (isGrounded) {
				Jump();
			} 
			else if (canDoubleJump) {
				canDoubleJump = false;
				Jump();
			}
		}
	}
	public void releaseK(){

	}
	public void pressL(){

	}
	public void releaseL(){

	}

	public void pressU(){

	}
	public void releaseU(){

	}
	public void pressI(){

	}
	public void releaseI(){

	}
	public void pressO(){

	}
	public void releaseO(){

	}
}
