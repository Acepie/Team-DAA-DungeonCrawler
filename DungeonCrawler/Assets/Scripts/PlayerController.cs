using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractCreature {


	Rigidbody2D rb2d;
	bool hasAttacked;
	private bool readyToAttack;
	public int attackPower;
	private AbstractCreature targetCreature;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!inCombat) {
			Move (speed);
		} else {
			ProcessCombat();
		}
	}

	override public void Move(float speed){
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");


		if (Mathf.Abs(moveHorizontal) > 0.1 || Mathf.Abs(moveVertical) > 0.1) {
			rb2d.velocity = new Vector2 (moveHorizontal, moveVertical) * speed;

			// Update animation controller
			if (moveVertical > 0) {
				this.GetComponent<Animator>().SetInteger("Direction", 0);
			} else if (moveVertical < 0) {
				this.GetComponent<Animator>().SetInteger("Direction", 1);
			} else if (moveHorizontal > 0) {
				this.GetComponent<Animator>().SetInteger("Direction", 2);
			} else {
				this.GetComponent<Animator>().SetInteger("Direction", 3);
			}

		} else {
			rb2d.velocity = new Vector2 (0, 0);

			// Set the animation to idle
			//this.GetComponent<Animator>().SetInteger("Direction", -1);
		}

		if (!Input.anyKey)
		{
			// If nothing is being pressed, then move to idle animation
			this.GetComponent<Animator>().SetInteger("Direction", -1);
		}
	}

	private void ProcessCombat() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			readyToAttack = targetCreature != null;
		}

		if (Input.GetMouseButtonDown(0)) {
			Camera cam = Camera.main;
			RaycastHit2D hit = Physics2D.Raycast (cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				targetCreature = hit.collider.gameObject.GetComponent<AbstractCreature> ();
				Debug.Log (targetCreature.name);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.CompareTag("Enemy")){
			inCombat = true;
			GameObject combat = new GameObject("CombatInstance");
			combat.transform.position = transform.position;
			combat.AddComponent<CombatController>();
		}
	}

	public override void MakeAttack(List<AbstractCreature> targets)
	{
		if (!hasAttacked) {
			if (readyToAttack) {
				Debug.Log("Player attacking " + targetCreature.name);
				targetCreature.UnderAttack(attackPower);
				hasAttacked = true;
			}
		}
	}

	public override void OnDeath()
	{
		Debug.Log("Defeated!!!");
	}

	public override bool TurnOver()
	{
		return hasAttacked;
	}

	public override void StartTurn(List<AbstractCreature> targets)
	{
		hasAttacked = false;
		readyToAttack = false;
	}
}
