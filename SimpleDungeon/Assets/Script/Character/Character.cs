using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Character : MonoBehaviour, Receiver, Interactable, Attackable, Damagable {

	private string className = "";
	public float moveSpeed = 0.0f;
	public float passiveMoveSpeed = 0.0f;
	public int faceDir = 1;
	public int passiveMoveDir = 0;
	public int jumpCount = 0;

	public bool isClimbing = false;
	public bool isGrounded = true;
	public bool isMoving = false;
	public bool isBlocking = false;
	public int blockingDir = 0;
	public bool canWalk = true;
	public bool canJump = true;
	public bool canDoubleJump = false;
	public bool canTurn = true;

	protected Action currentAction;
	protected string currentAnimation;

	protected Rigidbody2D rb;
	protected SpriteRenderer sr;
	protected Collider2D colli;
	protected Animator anim;
	protected CharacterInfo info;

	protected Dictionary<Collider2D, int> ignoreColliderMap = new Dictionary<Collider2D, int>();

	protected float halfWidth;
	protected float halfHeight;

	protected Dictionary<Action.Type, Action> actionTable = new Dictionary<Action.Type, Action>();
	protected Dictionary<string, AnimationClip> animationClipTable = new Dictionary<string, AnimationClip>();

	public Controller AI;

	// Use this for initialization
	protected void Start () {
		className = this.GetType().ToString();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		colli = GetComponent<Collider2D> ();
		halfWidth = colli.bounds.extents.x;
		halfHeight = colli.bounds.extents.y;

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
		//actionTable [Action.Type.Defend] = new RushAction (this);
		//actionTable [Action.Type.Defend].SetNextAction (Action.Type.Attack);
		actionTable [Action.Type.Damage] = new DamageAction (this);
		actionTable [Action.Type.Climb] = new ClimbAction (this);
		actionTable [Action.Type.Dead] = new DeadAction(this);
		//actionTable [Action.Type.Rush] = new RushAction (this);
		// wall jump
		// edge + jump -> climb up
		// combo
		// pick up

		//actionTable [Action.Type.Attack].SetPlaySpeedByDuration (1f);

		// default attack
		AttackArea aa = GetComponentInChildren<AttackArea> ();
		aa.SetAttacker(this);
		aa.SetAttack(new SimpleAttack(10));
        
		info = GetComponent<CharacterInfo> ();
		info.owner = this;
		info.HPBar = transform.Find("HP/Cur HP").gameObject;

		
		SimpleAIController simpleAI = gameObject.AddComponent<SimpleAIController>();
		simpleAI.SetReceiver(this);		
		simpleAI.transform.parent = transform;
		AI = simpleAI;
	}

	// Update is called once per frame
	protected virtual void Update () {
		if (!info.IsDead ()) {
			UpdateState ();
			 DetectTarget();
			UpdateMove ();
			UpdateAnimationState ();
		}
		
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
			if(IsBlockedFace())
				canDoubleJump = true;
		}

		isBlocking = IsBlocked();
		blockingDir = faceDir;
	}

	protected bool IsBlocked(){
		if(isMoving){
			if(IsBlockedFace() || IsGoingToFall()){
				return true;
			}
		}
		return false;
	}

	protected bool IsGoingToFall(){	
		if(!isGrounded)	
			return false;

		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.05f);
		if (hits.Length>0){
			bool isLeftDir = faceDir==(int)World.Direction.LEFT;

			if(hits.Length == 1){
				Collider2D colli = hits[0].collider;
				float width = colli.bounds.extents.x;
				return (isLeftDir)?
						transform.position.x-halfWidth <= colli.transform.position.x-width
						:transform.position.x+halfWidth >= colli.transform.position.x+width;
			}
			else{
				float curX = 0f;
				float maxX = (isLeftDir)? Mathf.Infinity:-Mathf.Infinity;

				foreach(RaycastHit2D hit in hits){
					if(isLeftDir){
						curX = hit.collider.transform.position.x-hit.collider.bounds.extents.x;
						if(maxX>curX)
							maxX = curX;
					}
					else{
						curX = hit.collider.transform.position.x+hit.collider.bounds.extents.x;
						if(maxX<curX)
							maxX = curX;
					}
				}
				return (isLeftDir)?
						 transform.position.x-halfWidth <= maxX
						:transform.position.x+halfWidth >= maxX;
			}
		}
		return false;
	}
	protected bool IsBlockedFace(){
		RaycastHit2D hit = Physics2D.Raycast(transform.position, (faceDir==(int)World.Direction.LEFT)? Vector2.left: Vector2.right);
		return (hit.collider != null && hit.distance-halfWidth<=0.05f && hit.collider.tag == "Solid");
	}

	// ==== control ==============
	public void Move(World.Direction d){
		
		if (World.Direction.STOP == d) {
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
	public void Turn(World.Direction d){
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

	protected void Rush(World.Direction d){
		// if d != faceDir => doge (not move, just skip attack)
		// if d == faceDir => rush
		((RushAction)actionTable [Action.Type.Rush]).SetDirection (d);
		actionTable [Action.Type.Rush].TriggerAction ();
	}
	public void Attack(){
		actionTable [Action.Type.Attack].TriggerAction ();
	}

	protected void JumpAttack(){
		actionTable [Action.Type.JumpAttack].TriggerAction ();
	}

	protected virtual void ReleaseAttack(){
		if(currentAction is AttackShootAction || currentAction is JumpShootAttackAction){
			currentAction.CancelAction();
		}
	}

	public void Climb(World.Direction d){
		((ClimbAction)actionTable [Action.Type.Climb]).SetDirection (d);
		if(((ClimbAction)actionTable[Action.Type.Climb]).ClimbableFound()){

			if(!isClimbing)
				actionTable [Action.Type.Climb].TriggerAction ();
		}

	}

	public void Interact(Interactable target){
		
	}
	public void React (){
		
	}
	// ==== passive action =======
	public void TakeDamage(Attack attack){
		info.Damage (attack.GetDamage());
		if(info.IsDead())
			return;

		float time = attack.GetDamage()/10f;
		time = 2f;
		actionTable [Action.Type.Damage].SetPlayerUntilAnimationTime(time);
		actionTable [Action.Type.Damage].TriggerAction ();		
	}
	public void Die(){
		if(currentAction != null)
			currentAction.StopAction();
		actionTable [Action.Type.Dead].TriggerAction ();
	}

	// ==== animation ==============
	protected void UpdateAnimationState(){
		if(currentAction == null){

			string newAnimationName = "";

			if(!isGrounded){
				newAnimationName = className+"_Jump";
			}
			else if(isMoving){
				newAnimationName = className+"_Walk";
			}
			else{
				newAnimationName = className+"_Idel";
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
		Debug.Log(currentAnimation);
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

	protected void OnCollisionEnter2D(Collision2D coll) {
		if (isClimbing && coll.gameObject.tag == "Solid" && !Physics2D.GetIgnoreCollision (coll.collider, colli)) {
			Debug.Log ("add "+coll.gameObject.name);
			Physics2D.IgnoreCollision (coll.collider, colli);
			ignoreColliderMap[coll.collider]=0;
		} 
	}

	// ==== receiver =============
	public virtual void pressQ(){
		
	}
	public virtual void releaseQ(){

	}
	public virtual void pressE(){

	}
	public virtual void releaseE(){

	}

	public virtual void pressW(){
		Climb(World.Direction.UP);
	}
	public virtual void releaseW(){
		Climb(World.Direction.STOP);
	}
	public virtual void pressS(){
		Climb(World.Direction.DOWN);
	}
	public virtual void releaseS(){
		Climb(World.Direction.STOP);
	}
	public virtual void pressA(){
		Move(World.Direction.LEFT);
		Turn(World.Direction.LEFT);
	}
	public virtual void releaseA(){
		Move(World.Direction.STOP);
	}
	public virtual void pressD(){
		Move(World.Direction.RIGHT);
		Turn(World.Direction.RIGHT);
	}
	public virtual void releaseD(){
		Move(World.Direction.STOP);
	}

	public virtual void pressJ(){
		if (isGrounded) {
			Attack();
		}
		else
			JumpAttack ();
	}
	public virtual void releaseJ(){
		ReleaseAttack();
	}
	public virtual void pressK(){
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
	public virtual void releaseK(){

	}
	public virtual void pressL(){

	}
	public virtual void releaseL(){

	}

	public virtual void pressU(){

	}
	public virtual void releaseU(){

	}
	public virtual void pressI(){

	}
	public virtual void releaseI(){

	}
	public virtual void pressO(){

	}
	public virtual void releaseO(){

	}

	// ============= getter setters
	public Rigidbody2D GetRigidbody2D(){
		return rb;
	}
	public Collider2D GetCollider(){
		return colli;
	}
	public SpriteRenderer GetSpriteRenderer(){
		return sr;
	}
	public Dictionary<Collider2D, int> GetIgnoreColliderMap(){
		return ignoreColliderMap;
	}
	public CharacterInfo GetCharacterInfo(){
		return info;
	}
	public string GetClassName(){
		return className;
	}

	public void DestoryAfterMS(float ms){
		DestoryAllChildren(this.gameObject, ms);
	}

	protected void DestoryAllChildren(GameObject gameObject, float ms){
		foreach (Transform child in gameObject.transform){
			DestoryAllChildren(child.gameObject, ms);
        }
		Destroy(gameObject, ms/1000);
	}

	public World.Direction DirectionTo(Character target){
		if(target == null)
			return (faceDir==-1)? World.Direction.LEFT: World.Direction.RIGHT;

		if(target.colli.bounds.center.x < this.colli.bounds.center.x)
			return World.Direction.LEFT;
		else
			return World.Direction.RIGHT;
	}

	public Character DetectTarget(){
		bool isLeftDir = faceDir == (int)World.Direction.LEFT;
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, isLeftDir? Vector2.left: Vector2.right, 5);
		foreach(RaycastHit2D hit in hits){
			if(!Util.isCharacter(hit.collider.tag))
				return null;
				
			if(Util.isEnemy(tag, hit.collider.tag)){
				Debug.Log(hit.collider.transform.GetComponent<Character>());
				return hit.collider.transform.GetComponent<Character>();
			}
		}
		return null;
	}
}
