using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemy : AbstractCreature
{
    private float lastMoveTimeStamp;
    public float moveCD = 1.0f;

    private float aggroRadius;
    private int moveDirection;

    Rigidbody2D rb2d;

    Animator ani;

    public RuntimeAnimatorController combatAnimControl;
    public RuntimeAnimatorController normalAnimControl;

    public GameObject dropItem;
    [Range(0, 1)]
    public float dropRate;

    // Use this for initialization
    void Awake()
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
        int attackDamage = Random.Range(1, data.attackpower + 1);
        AbstractCreature target = targets[Random.Range(0, targets.Count)];
        target.UnderAttack(attackDamage);
        yield return null;
    }

    public override void OnDeath()
    {
        if (Random.value > 0.5 && dropItem) {
            GameObject item = Instantiate(dropItem, transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
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

    public override void CombatStarted()
    {
        base.CombatStarted();
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
}
