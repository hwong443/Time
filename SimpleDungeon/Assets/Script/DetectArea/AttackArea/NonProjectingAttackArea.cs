using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonProjectingAttackArea : MonoBehaviour {
	private Attackable attacker;
	private Attack attack;
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private Collider2D colli;
	private float moveForceX = 0.0f;
	private float aliveTime = 5.0f;

	void Update(){
		aliveTime -= Time.deltaTime;
		if(aliveTime<=0)
			Destroy(gameObject);
	}

	public void Init(){
		rb = GetComponent<Rigidbody2D> ();
		rb.gravityScale = 0;
		sr = GetComponent<SpriteRenderer> ();
		colli = GetComponent<Collider2D> ();
	}

	public void SetSprite(Sprite sprite){
		sr.sprite = sprite;
	}
	public void SetForce(float moveForceX){
		this.moveForceX = moveForceX;
		rb.AddForce(Vector2.right * moveForceX);
		TurnDir();
	}
	public void SetAliveTime(float aliveTime){
		this.aliveTime = aliveTime;
	}
	public void SetAttacker(Attackable attacker){
		this.attacker = attacker;
	}
	public void SetAttack(Attack attack){
		this.attack = attack;
	}

	void TurnDir(){
		Vector3 newScale = transform.localScale;
		newScale.y = (moveForceX>0)? 1:-1;
		transform.localScale = newScale;
	}

	void OnTriggerEnter2D(Collider2D colli) {
		// skip same party target
		if(attacker!=null && colli.tag != ((MonoBehaviour)attacker).tag){
			colli.SendMessage("TakeDamage", attack, SendMessageOptions.DontRequireReceiver);
			Destroy(gameObject);
		}
	}
}
