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
    private SkillHandler skillHandler;
    private HashSet<string> keysFound;

    void Awake()
    {
        SpawnPlayer();
        rb2d = GetComponent<Rigidbody2D>();
        skillHandler = GetComponent<SkillHandler>();
        keysFound = new HashSet<string>();
        data.currentHealth = data.maxHealth;
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

        bool turnEnded = false;
        bool hasMoved = false;
        AbstractCreature potentialTarget = null;

        //Potential values needed for multiattacks
        HashSet<AbstractCreature> targetsBeingAttacked = new HashSet<AbstractCreature>();
        int count = 0;
        while (!turnEnded)
        {
            
            count++;
            Debug.Log(count);
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
                Debug.Log("Clicking");
                potentialTarget = ClickOnTarget();
                if (potentialTarget == null)
                {
                    targetsBeingAttacked.Clear();
                    continue;
                }

                if (!validTargetList.Contains(potentialTarget)) {
                    continue;
                }

                if (targetsBeingAttacked.Contains(potentialTarget)) {
                    targetsBeingAttacked.Remove(potentialTarget);
                } else {
                    targetsBeingAttacked.Add(potentialTarget);
                }

                string combatText = "Click an enemy to mark/unmark for attack.\nTargets:\n";
                foreach(var t in targetsBeingAttacked) {
                    combatText += t.name + ":\t Health: " + t.data.currentHealth + "\n";
                }
                combatText += "\nPress 1 to perform attack. Press 2 for a slam attack! Press 3 for a multihit attack. Press 4 for a fireball" ;
                this.ctc.updateText(combatText);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                turnEnded = skillHandler.performSkillAtIndex(1, new List<AbstractCreature>(targetsBeingAttacked), data, this);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                turnEnded =  skillHandler.performSkillAtIndex(2, new List<AbstractCreature>(targetsBeingAttacked), data, this);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                turnEnded =  skillHandler.performSkillAtIndex(3, new List<AbstractCreature>(targetsBeingAttacked), data, this);
            }

            if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                turnEnded = skillHandler.performSkillAtIndex(4, new List<AbstractCreature>(targetsBeingAttacked), data, this);
            }
        }

        yield return null;
    }

    public override void StartTurn() {
        
        string combatText = "Click an enemy to mark/unmark for attack.\n";
        this.ctc.updateText(combatText);
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

    public void SpawnPlayer()
    {
        transform.position = GameObject.Find("PlayerSpawnPoint").transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            if (data.currentHealth + 5 > data.maxHealth)
            {
                data.currentHealth = data.maxHealth;
            }
            else
            {
                data.currentHealth += 5;
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
            data.currentHealth += 2;
            data.maxHealth += 2;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Key"))
        {
            playerUIController.PickupEvent("You found a key! I wonder what it unlocks...");

            keysFound.Add(other.gameObject.GetComponent<KeyItem>().keyName);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("EndZone"))
        {
            playerUIController.PickupEvent("Level Ended! Progressing to next level");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !inCombat)
        {
            inCombat = true;
            GameObject combat = new GameObject("CombatInstance");
            combat.transform.position = transform.position;
            combat.AddComponent<CombatController>();
        }

        if (other.gameObject.CompareTag("Door"))
        {
            if (keysFound.Contains(other.gameObject.GetComponent<LockedDoor>().keyToUnlock))
            {
                playerUIController.PickupEvent("The door unlocks!");
                Destroy(other.gameObject);
            } else
            {
                playerUIController.PickupEvent("You don't have the right key to open this door");
            }
        }
    }

    public override void CombatEnded()
    {
        base.CombatEnded();
        skillHandler.resetCooldowns();
    }
}
