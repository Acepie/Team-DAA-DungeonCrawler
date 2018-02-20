using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : AbstractCreature
{

    Rigidbody2D rb2d;

    // Used for combat UI
    public CombatTextController ctc;
    public PlayerUIController playerUIController;

    bool turnEnded;
    public PlayerData data;
    private SkillHandler skillHandler;

    void Awake()
    {
        SpawnPlayer();
        rb2d = GetComponent<Rigidbody2D>();
        skillHandler = GetComponent<SkillHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCombat)
        {
            Move(speed);
        }
    }

    override public void Move(float speed)
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");


        if (Mathf.Abs(moveHorizontal) > 0.1 || Mathf.Abs(moveVertical) > 0.1)
        {
            rb2d.velocity = new Vector2(moveHorizontal, moveVertical) * speed;
            // Update animation controller
            if (moveHorizontal > 0)
            {
                //East
                this.GetComponent<Animator>().SetInteger("Direction", 1);
            }
            else if (moveHorizontal < 0)
            {
                //West
                this.GetComponent<Animator>().SetInteger("Direction", 2);
            }
            else if (moveVertical > 0)
            {
                //North
                this.GetComponent<Animator>().SetInteger("Direction", 3);
            }
            else
            {
                //South
                this.GetComponent<Animator>().SetInteger("Direction", 4);
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
        }

        if (!Input.anyKey)
        {
            // If nothing is being pressed, then move to idle animation
            this.GetComponent<Animator>().SetInteger("Direction", -1);
        }
    }

    /* Coroutine that process the player's Turn. Check for end turn condition (currently set to after using any skill/attack).
	While end turn condition is not met we get player input to determine what they can do. Options are: End the turn, move their character, or use a skill
	*/

    public override IEnumerator PerformTurn(List<AbstractCreature> validTargetList)
    {

        //Reduce current CD of any skills on CD by 1
        skillHandler.decrementSkillsCooldown();

        turnEnded = false;
        bool targetSelected = false;
        bool hasMoved = false;
        AbstractCreature potentialTarget = null;

        //Potential values needed for multiattacks
        int maxEnemiesHit = 1;
        int enemiesUnderAttack = 0;
        List<AbstractCreature> targetsBeingAttacked = new List<AbstractCreature>();

        while (!turnEnded)
        {
            if (potentialTarget == null)
            {
                this.ctc.updateText("Click Enemy to mark for attack.\n Target: ");
            }

            yield return new WaitUntil(() => Input.anyKey);
            if (Input.GetKeyDown(KeyCode.E))
            {
                turnEnded = true;
            }

            if (Input.GetKeyDown(KeyCode.M) && !hasMoved)
            {
                ctc.updateText("Move action intiated. Click where you want to go!");
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                Camera cam = Camera.main;
                transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
                hasMoved = true;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                //Use some item
            }

            if (Input.GetMouseButtonDown(0))
            {
                potentialTarget = ClickOnTarget();
                if (potentialTarget == null)
                {
                    continue;
                }
                this.ctc.updateText("Click Enemy to mark for attack.\nTarget: " +
                potentialTarget.name + "\n Health: " + potentialTarget.currentHealth +
                "\n\nPress 1 to preform attack. Press 2 for a slam attack!");
                if (potentialTarget != null && enemiesUnderAttack != maxEnemiesHit)
                {
                    targetsBeingAttacked.Add(potentialTarget);
                    enemiesUnderAttack++;
                    targetSelected = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && targetSelected && enemiesUnderAttack == maxEnemiesHit)
            {
                skillHandler.performSkillAtIndex(1, potentialTarget, data);
                //MakeAttack (targetsBeingAttacked);
                turnEnded = skillHandler.skillPerformed;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && targetSelected)
            {

                skillHandler.performSkillAtIndex(2, potentialTarget, data);
                turnEnded = skillHandler.skillPerformed;
            }
        }

        yield return null;
    }

    private AbstractCreature ClickOnTarget()
    {
        AbstractCreature targetCreature = null;
        Camera cam = Camera.main;
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.GetComponent<AbstractCreature>() is AbstractEnemy)
        {
            targetCreature = hit.collider.gameObject.GetComponent<AbstractCreature>();

        }

        return targetCreature;
    }

    public override void OnDeath()
    {
        Debug.Log("Defeated!!!");
    }

    public override bool TurnOver()
    {
        return turnEnded;
    }

    public override void StartTurn()
    {
    }

    public void SpawnPlayer()
    {
        transform.position = GameObject.Find("PlayerSpawnPoint").transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            if (currentHealth + 5 > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += 5;
            }
            playerUIController.PickupEvent("Healed for 5 hit points!");
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Weapon"))
        {
            playerUIController.PickupEvent("Picked up a weapon! Your strength has increased by 1 point!");
            data.attackpower += 1;
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            inCombat = true;
            GameObject combat = new GameObject("CombatInstance");
            combat.transform.position = transform.position;
            combat.AddComponent<CombatController>();

            // Turn on the UI
            ctc.enableText();
            ctc.updateText("Click Enemy to mark for attack: ");
        }
    }

    public override void CombatEnded()
    {
        base.CombatEnded();
        skillHandler.resetCooldowns();
    }
}
