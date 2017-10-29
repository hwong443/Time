using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour {
	public Character owner;
	public GameObject HPBar;

	public int STR = 10;
	public int AGI = 10;
	public int INT = 10;

	private int maxHP = 100;
	private int maxMP = 100;
	private int maxSP = 100;
	private float recHP = 1.0f;
	private float recMP = 1.0f;
	private float recSP = 1.0f;
	public float HP = 100;
	public float MP = 100;
	public float SP = 100;

	public int moveSpeed = 5;
	public int jumpSpeed = 15;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(!IsDead()){
			UpdateHP ();
			UpdateMP ();
			UpdateSP ();
		}	
		HPBar.transform.localScale = new Vector2(HP/maxHP,1);
	}

	void UpdateHP(){
		HP += recHP*Time.deltaTime;
		if (HP > maxHP)
			HP = maxHP;
		else if(HP < 0)
			HP = 0;
	}
	void UpdateMP(){
		MP += recMP*Time.deltaTime;
		if (MP > maxMP)
			MP = maxMP;
		else if(MP < 0)
			MP = 0;
	}
	void UpdateSP(){
		SP += recSP*Time.deltaTime;
		if (SP > maxSP)
			SP = maxSP;
		else if(SP < 0)
			SP = 0;
	}

	public void Damage(int d){
		if(IsDead()) return;

		HP -= d;

		if(HP <= 0){
			owner.Die();
			HP = 0;
		}
	}

	public bool IsDead(){
		return HP <= 0;
	}

}
