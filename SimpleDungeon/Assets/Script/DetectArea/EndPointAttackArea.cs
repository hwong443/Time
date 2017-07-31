using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointAttackArea : MonoBehaviour {
	public Attackable attacker;
	public Attack attack = new ShortWeapon(10);
	public Rigidbody2D rb;
	public SpriteRenderer sr;
	public Collider2D colli;
	public float moveSpeed = 0.0f;
	public float moveForceX = 0.0f;
	public float moveForceY = 0.0f;
	public int faceDir = 1;
	public float goalX = 0.0f;
	public float goalY = 0.0f;
	public float aliveTime = 0.0f;
	public List<Collider2D> collisionList = new List<Collider2D>();

	void Update(){
		aliveTime -= Time.deltaTime;
		if(aliveTime<=0){
			if(collisionList.Count>0)
				collisionList[0].SendMessage("TakeDamage", attack, SendMessageOptions.DontRequireReceiver);

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
	public void SetForce(float moveSpeed){
		this.moveSpeed = moveSpeed;
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
		if(moveSpeed != 0){
			aliveTime = totalDistance/moveSpeed;
			moveForceX = (aliveTime==0)? 0: (goalX-startX)/aliveTime;
			moveForceY = (aliveTime==0)? 0: (goalY-startY)/aliveTime;
			rb.AddForce(new Vector2(moveForceX, moveForceY));
		}

		// turn dir
		Vector3 newScale = transform.localScale;
		newScale.y = (goalX>startX)? 1:-1;
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
