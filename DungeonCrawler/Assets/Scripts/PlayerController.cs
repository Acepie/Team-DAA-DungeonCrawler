using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractCreature {


	Rigidbody2D rb2d;
	bool hasAttacked;
	int target;
	int numTargets;
	bool readyToAttack;
	public int attackPower;

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
		// TODO actual target selection
		if (Input.GetKeyDown(KeyCode.Space)) {
			readyToAttack = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("Enemy")){
			inCombat = true;
			GameObject combat = new GameObject("CombatInstance");
			combat.AddComponent<CombatController>();
		}
	}

	public override void UnderAttack(int damageTaken)
	{
		health -= damageTaken;
	}

	public override void MakeAttack(List<AbstractCreature> targets)
	{
		if (!hasAttacked) {
			if (readyToAttack) {
				AbstractCreature targetCreature = targets[target];
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

	public IEnumerator waitPlayerLeftClick(){
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
	}

	public override void StartTurn(List<AbstractCreature> targets)
	{
		hasAttacked = false;
		target = 0;
		numTargets = targets.Count;
		readyToAttack = false;
	}
}
