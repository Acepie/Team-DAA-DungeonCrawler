using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemy : MonoBehaviour {

	public int health;
	public float speed;

	private float moveDuration;
	private float lastMoveTimeStamp;
	public float moveCD = 1.0f;
	private int maxAttackDamage = 1;

	private int moveDirection;

	Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	void UnderAttack(int damageTaken){
		health -= damageTaken;
		if (health <= 0) {
			OnDeath();
		}
	}

	int MakeAttack(){

		int attackDamage = 0;
		attackDamage = Random.Range (1, maxAttackDamage + 1);
		return attackDamage;
	}

	void OnDeath(){
		Destroy (this.gameObject);
		//Animate Death
	}

	void Move(){
		if (Time.time - lastMoveTimeStamp > moveCD) {
			moveDirection = Random.Range (1, 5);
			lastMoveTimeStamp = Time.time;
			Debug.Log (moveDirection);
		}

		switch (moveDirection) {
		case 1:
			rb2d.velocity = new Vector2 (1, 0);
			break;
		case 2:
			rb2d.velocity = new Vector2 (-1, 0);
			break;
		case 3:
			rb2d.velocity = new Vector2 (0, 1);
			break;
		case 4:
			rb2d.velocity = new Vector2 (0, -1);
			break;
		}
	}
}
