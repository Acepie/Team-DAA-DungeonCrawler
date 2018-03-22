using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : AbstractCreature
{

    Rigidbody2D rb2d;

    // Used for combat UI
    public CombatTextController ctc;
    public PlayerUIController playerUIController;
    private SkillHandler skillHandler;
    private HashSet<string> keysFound;
    private Animator animator;

    void Awake()
    {
        SpawnPlayer();
        rb2d = GetComponent<Rigidbody2D>();
        skillHandler = GetComponent<SkillHandler>();
        keysFound = new HashSet<string>();
        data.currentHealth = data.maxHealth;
        animator = this.GetComponent<Animator>();
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
            animator.SetFloat("DirectionX", moveHorizontal);
            animator.SetFloat("DirectionY", moveVertical);
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
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

        //Potential values needed for multiattacks
        HashSet<AbstractCreature> targetsBeingAttacked = new HashSet<AbstractCreature>();

        while (!turnEnded)
        {
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
                AbstractCreature potentialTarget = ClickOnTarget();
                if (potentialTarget == null)
                {
                    targetsBeingAttacked.Clear();
                    continue;
                }

                if (!validTargetList.Contains(potentialTarget))
                {
                    continue;
                }

                if (targetsBeingAttacked.Contains(potentialTarget))
                {
                    targetsBeingAttacked.Remove(potentialTarget);
                }
                else
                {
                    targetsBeingAttacked.Add(potentialTarget);
                }

                string combatText = "Click an enemy to mark/unmark for attack.\nTargets:\n";
                foreach (var t in targetsBeingAttacked)
                {
                    combatText += t.name + ":\t Health: " + t.data.currentHealth + "\n";
                }
                combatText += "\nPress 1 to perform attack. Press 2 for a slam attack! Press 3 for a multihit attack.";
                this.ctc.updateText(combatText);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && targetsBeingAttacked.Count > 0)
            {
                skillHandler.performSkillAtIndex(1, new List<AbstractCreature>(targetsBeingAttacked), data);
                turnEnded = skillHandler.skillPerformed;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && targetsBeingAttacked.Count > 0)
            {
                skillHandler.performSkillAtIndex(2, new List<AbstractCreature>(targetsBeingAttacked), data);
                turnEnded = skillHandler.skillPerformed;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && targetsBeingAttacked.Count > 0)
            {
                skillHandler.performSkillAtIndex(3, new List<AbstractCreature>(targetsBeingAttacked), data);
                turnEnded = skillHandler.skillPerformed;
            }
        }

        yield return null;
    }

    public override void StartTurn()
    {

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SpawnPlayer()
    {
        transform.position = GameObject.Find("PlayerSpawnPoint").transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.GetComponent<AbstractPickupItem>().collect(playerUIController, data);
        }

        if (other.gameObject.CompareTag("Key"))
        {
            playerUIController.PickupEvent("You found a key! I wonder what it unlocks...");

            keysFound.Add(other.gameObject.GetComponent<KeyItem>().keyName);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Switch"))
        {
            other.gameObject.GetComponent<Switch>().ActivateSwitch();
            playerUIController.PickupEvent("A switch has activated! I wonder what it does...");
        }

        if (other.gameObject.CompareTag("EndZone"))
        {
            playerUIController.PickupEvent("Level Ended! Progressing to next level");
            other.gameObject.GetComponent<LevelLoader>().LoadLevel();
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
            LockedDoor door = other.gameObject.GetComponent<LockedDoor>();
            if (!door)
            {
                return;
            }

            if (keysFound.Contains(door.keyToUnlock))
            {
                playerUIController.PickupEvent("The door unlocks!");
                Destroy(other.gameObject);
            }
            else
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
