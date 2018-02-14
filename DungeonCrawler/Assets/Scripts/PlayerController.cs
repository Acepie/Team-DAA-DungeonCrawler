using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractCreature {


	public float enemyCombatTrigger;

	Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		speed = 1.2f;
	}
	
	// Update is called once per frame
	void Update () {
		Move (speed);
	}

	override public  void Move(float speed){
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");


		if (Mathf.Abs(moveHorizontal) > 0.1 || Mathf.Abs(moveVertical) > 0.1) {
			
			rb2d.velocity = new Vector2 (moveHorizontal, moveVertical) * speed;
		} else {
			rb2d.velocity = new Vector2 (0, 0);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("Enemy")){
			inCombat = true;
			CombatController combat = new CombatController ();
		}
	}
}
