using System.Collections;
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
	}
	
	// Update is called once per frame
	void Update () {
		if (!inCombat) {
			Move (speed);
		}
	}

	public override void  MakeAttack(List<AbstractCreature> targets){

		int attackDamage = 0;
		attackDamage = Random.Range (1, maxAttackDamage + 1);
		AbstractCreature target = targets[Random.Range(0, targets.Count)];
		Debug.Log("Enemy attacking " + target.name);
		Debug.Log (target.name + " has"  + target.health + "health");
		target.UnderAttack(attackDamage);
	}

	public override void OnDeath(){
		Destroy (this.gameObject);
		//Animate Death
	}

	public override void Move(float speed){
		if (Time.time - lastMoveTimeStamp > moveCD) {
			moveDirection = Random.Range (1, 5);
			lastMoveTimeStamp = Time.time;
		}

		Animator ani = GetComponent<Animator>();

		switch (moveDirection) {
		case 1:
				ani.SetInteger("Direction", 1);
				rb2d.velocity = new Vector2 (speed, 0);
				break;
		case 2:
				ani.SetInteger("Direction", 2);
				rb2d.velocity = new Vector2 (-speed, 0);
				break;
		case 3:
				ani.SetInteger("Direction", 3);
				rb2d.velocity = new Vector2 (0, speed);
				break;
		case 4:
				ani.SetInteger("Direction", 4);
				rb2d.velocity = new Vector2 (0, -speed);
				break;
		}
	}

	public override bool TurnOver()
	{
		return true;
	}

	public override void CombatStarted ()
	{
		speed = 0;
		swapToCombatSprite ();
	}

	public override void CombatEnded ()
	{
		base.CombatEnded ();
		revertToNormalSprite();
	}

	public virtual void swapToCombatSprite(){

	}

	public virtual void revertToNormalSprite(){

	}

	public override void StartTurn(List<AbstractCreature> targets){}
}
