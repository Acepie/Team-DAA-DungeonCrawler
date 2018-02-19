using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : AbstractCreature {


	Rigidbody2D rb2d;
	bool hasAttacked;
	private bool readyToAttack;
	public int attackPower;
	private AbstractCreature targetCreature;

	// Used for combat UI
	public CombatTextController ctc;
    public PlayerUIController playerUIController;

	void Awake(){
		SpawnPlayer ();
	}

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (!inCombat) {
			Move (speed);
		} else {
			//ProcessCombat();
		}
	}

	override public void Move(float speed){
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");


		if (Mathf.Abs(moveHorizontal) > 0.1 || Mathf.Abs(moveVertical) > 0.1) {
			rb2d.velocity = new Vector2 (moveHorizontal, moveVertical) * speed;

			// Update animation controller
			if (moveHorizontal > 0) {
				//East
				this.GetComponent<Animator>().SetInteger("Direction", 1);
			} else if (moveHorizontal < 0) {
				//West
				this.GetComponent<Animator>().SetInteger("Direction", 2);
			} else if (moveVertical > 0) {
				//North
				this.GetComponent<Animator>().SetInteger("Direction", 3);
			} else {
				//South
				this.GetComponent<Animator>().SetInteger("Direction", 4);
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

	/*private void ProcessCombat() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			//readyToAttack = targetCreature != null;
		}

		if (Input.GetMouseButtonDown(0)) {
			ClickOnTarget ();
		}
	}*/

	public override IEnumerator PerformTurn(List<AbstractCreature> validTargetList){
		bool turnEnded = false;
		bool targetSelected = false;
        int maxEnemiesHit = 1;
        int enemiesUnderAttack = 0;
        AbstractCreature potentialTarget = null;
        List<AbstractCreature> targetsBeingAttacked = new List<AbstractCreature>();

		while (!turnEnded) {
            if(potentialTarget== null)
            {
                this.ctc.updateText("Click Enemy to mark for attack.\n Target: ");
            }

            Debug.Log ("Player turn");
            yield return new WaitUntil(() => Input.anyKey);
			if(Input.GetKeyDown(KeyCode.E)){
				turnEnded = true;
			}

			if(Input.GetKeyDown(KeyCode.M)){
                Debug.Log("Move action intiated. Click where you want to go!");
                Camera cam = Camera.main;
                transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
			}

            if (Input.GetKeyDown(KeyCode.I))
            {
                //Use some item
            }

			if(Input.GetMouseButtonDown(0) && !targetSelected){
                potentialTarget = (ClickOnTarget());
                if(potentialTarget == null)
                {        
                    continue;
                }
                this.ctc.updateText("Click Enemy to mark for attack.\nTarget: " +
                potentialTarget.name + "\n Health: " + potentialTarget.currentHealth +
                "\n\nPress Space to preform attack.");
                if (potentialTarget != null && enemiesUnderAttack != maxEnemiesHit) {
                    targetsBeingAttacked.Add(potentialTarget);
                    enemiesUnderAttack++;
					targetSelected = true;
				}
			} 


			if (Input.GetKeyDown (KeyCode.Space) && targetSelected && enemiesUnderAttack == maxEnemiesHit) {
				MakeAttack (targetsBeingAttacked);
                turnEnded = true;
			}
		}

		yield return null;
	}

	private AbstractCreature ClickOnTarget(){
		targetCreature = null;
		Camera cam = Camera.main;
		RaycastHit2D hit = Physics2D.Raycast (cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider != null && hit.collider.GetComponent<AbstractCreature>() is AbstractEnemy) {
			targetCreature = hit.collider.gameObject.GetComponent<AbstractCreature> ();

		}

		return targetCreature;
	}
		
	public override void MakeAttack(List<AbstractCreature> targets)
	{
		if (!hasAttacked) {
			foreach (AbstractCreature t in targets) {
				if (t == null) {
					ctc.updateText ("No target selected. selected a target to attaack");
				} else {
					t.UnderAttack (attackPower);
					hasAttacked = true;
				}
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

	public override void StartTurn()
	{
		hasAttacked = false;
		readyToAttack = false;
	}

	public void SpawnPlayer(){
		transform.position = GameObject.Find ("PlayerSpawnPoint").transform.position;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            if (currentHealth + 5 > maxHealth)
            {
                currentHealth = maxHealth;
            } else
            {
                currentHealth += 5;
            }
            playerUIController.PickupEvent("Healed for 5 hit points!");
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Weapon"))
        {
            playerUIController.PickupEvent("Picked up a weapon! Your strength has increased by 1 point!");
            attackPower += 1;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Armor"))
        {
            playerUIController.PickupEvent("You got some armor! Your health has permanently increased by 2 points!");
            currentHealth += 2;
            maxHealth += 2;
            Destroy(other.gameObject);
        }

    }

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.CompareTag("Enemy")){
			inCombat = true;
			GameObject combat = new GameObject("CombatInstance");
			combat.transform.position = transform.position;
			combat.AddComponent<CombatController>();

			// Turn on the UI
			ctc.enableText();
			ctc.updateText("Click Enemy to mark for attack: ");
		}



    
	}
}
