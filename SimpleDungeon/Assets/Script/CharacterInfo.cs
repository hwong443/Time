using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour {
	public Character owner;

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

	public int moveSpeed = 10;
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
	}

	void UpdateHP(){
		HP += recHP*Time.deltaTime;
		if (HP > maxHP)
			HP = maxHP;
	}
	void UpdateMP(){
		MP += recMP*Time.deltaTime;
		if (MP > maxMP)
			MP = maxMP;
	}
	void UpdateSP(){
		SP += recSP*Time.deltaTime;
		if (SP > maxSP)
			SP = maxSP;
	}

	public void Damage(int d){
		HP -= d;
	}

	public bool IsDead(){
		return HP <= 0;
	}

}
