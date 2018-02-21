using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemy : AbstractCreature
{

    private float moveDuration;
    private float lastMoveTimeStamp;
    public float moveCD = 1.0f;
    private int maxAttackDamage = 1;


    private float aggroRadius;
    private int moveDirection;

    Rigidbody2D rb2d;

    Animator ani;

    public RuntimeAnimatorController combatAnimControl;
    public RuntimeAnimatorController normalAnimControl;

    void Awake()
    {
        ani = GetComponent<Animator>();
    }



    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        lastMoveTimeStamp = -moveCD;
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

    public override IEnumerator PerformTurn(List<AbstractCreature> targets)
    {
        MakeAttack(targets);
        yield return null;
    }
    public void MakeAttack(List<AbstractCreature> targets)
    {

        int attackDamage = 0;
        attackDamage = Random.Range(1, maxAttackDamage + 1);
        AbstractCreature target = targets[Random.Range(0, targets.Count)];
        target.UnderAttack(attackDamage);
    }

    public override void OnDeath()
    {
        Destroy(this.gameObject);
        //Animate Death
    }

    public override void Move(float speed)
    {
        if (Time.time - lastMoveTimeStamp > moveCD)
        {
            moveDirection = Random.Range(1, 5);
            ani.SetInteger("Direction", moveDirection);
            lastMoveTimeStamp = Time.time;
        }

        switch (moveDirection)
        {
            case 1:
                rb2d.velocity = new Vector2(speed, 0);
                break;
            case 2:
                rb2d.velocity = new Vector2(-speed, 0);
                break;
            case 3:
                rb2d.velocity = new Vector2(0, speed);
                break;
            case 4:
                rb2d.velocity = new Vector2(0, -speed);
                break;
        }
    }

    public override bool TurnOver()
    {
        return true;
    }

    public override void CombatStarted()
    {
        speed = 0;
        swapToCombatAnimations();
    }

    public override void CombatEnded()
    {
        base.CombatEnded();
        revertToNormalAnimations();
    }

    private void swapToCombatAnimations()
    {
        ani.runtimeAnimatorController = combatAnimControl;
    }

    private void revertToNormalAnimations()
    {
        ani.runtimeAnimatorController = normalAnimControl;
    }

    public override void StartTurn() { }
}
