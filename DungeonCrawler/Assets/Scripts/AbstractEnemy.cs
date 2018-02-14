﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemy : AbstractCreature {

	private float moveDuration;
	private float lastMoveTimeStamp;
	public float moveCD = 1.0f;
	private int maxAttackDamage = 1;


	private float aggroRadius;
	private int moveDirection;

	Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		speed = 1;
		
	}
	
	// Update is called once per frame
	void Update () {
		Move (speed);
	}

	protected override void UnderAttack(int damageTaken){
		health -= damageTaken;
		if (health <= 0) {
			OnDeath();
		}
	}

	protected override void  MakeAttack(){

		int attackDamage = 0;
		attackDamage = Random.Range (1, maxAttackDamage + 1);
	}

	protected override void OnDeath(){
		Destroy (this.gameObject);
		//Animate Death
	}

	public override void Move(float speed){
		if (Time.time - lastMoveTimeStamp > moveCD) {
			moveDirection = Random.Range (1, 5);
			lastMoveTimeStamp = Time.time;
		}

		switch (moveDirection) {
		case 1:
			rb2d.velocity = new Vector2 (speed, 0);
			break;
		case 2:
			rb2d.velocity = new Vector2 (-speed, 0);
			break;
		case 3:
			rb2d.velocity = new Vector2 (0, speed);
			break;
		case 4:
			rb2d.velocity = new Vector2 (0, -speed);
			break;
		}
	}
}