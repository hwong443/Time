using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointAttackArea : MonoBehaviour {
	private Attackable attacker;
	private Attack attack;
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private Collider2D colli;
	private float moveForce = 0.0f;
	private float moveForceX = 0.0f;
	private float moveForceY = 0.0f;
	private int faceDir = 1;
	private float goalX = 0.0f;
	private float goalY = 0.0f;
	private float aliveTime = 0.0f;
	private List<Collider2D> collisionList = new List<Collider2D>();

	void FixedUpdate(){
		aliveTime -= Time.deltaTime;
		transform.position = new Vector3(transform.position.x + moveForceX * Time.deltaTime, transform.position.y + moveForceY * Time.deltaTime, transform.position.z);
		if(aliveTime<=0){
			if(collisionList.Count>0){
				Debug.Log("attack "+collisionList[0].tag);
				collisionList[0].SendMessage("TakeDamage", attack, SendMessageOptions.DontRequireReceiver);
			}
			Destroy(gameObject);
		}
	}

	public void Init(){
		rb = GetComponent<Rigidbody2D> ();
		rb.gravityScale = 0;
		sr = GetComponent<SpriteRenderer> ();
		colli = GetComponent<Collider2D> ();
		CalSpeed();
	}

	public void SetSprite(Sprite sprite){
		sr.sprite = sprite;
	}
	public void SetGoalX(float goalX){
		this.goalX = goalX;
		CalSpeed();
	}
	public void SetGoalY(float goalY){
		this.goalY = goalY;
		CalSpeed();
	}
	public void SetForce(float moveForce){
		this.moveForce = moveForce;
	}
	public void SetAttacker(Attackable attacker){
		this.attacker = attacker;
	}
	public void SetAttack(Attack attack){
		this.attack = attack;
	}

	void CalSpeed(){
		float startX = transform.position.x;
		float startY = transform.position.y;
		float totalDistance = Mathf.Sqrt((goalX-startX)*(goalX-startX) + (goalY-startY)*(goalY-startY));

		if(moveForce != 0){
			aliveTime = totalDistance/moveForce;
			moveForceX = (aliveTime==0)? 0: (goalX-startX)/aliveTime;
			moveForceY = (aliveTime==0)? 0: (goalY-startY)/aliveTime;
		}

		// turn dir
		Vector3 newScale = transform.localScale;
		newScale.y = (goalX>startX)? 1:-1;
		//sr.flipY = (goalX<startX);

		transform.localScale = newScale;
	}

	void OnTriggerEnter2D(Collider2D colli){
		if(attacker!=null && colli.tag != ((MonoBehaviour)attacker).tag){
			collisionList.Add(colli);
		}
	}

	void OnTriggerExit2D(Collider2D colli){
		if(attacker!=null && colli.tag != ((MonoBehaviour)attacker).tag){
			collisionList.Remove(colli);
		}
	}
}
