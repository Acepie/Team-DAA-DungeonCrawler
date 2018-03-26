using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractProjectile : AbstractSkill
{
    public int projectileSpeed;
    protected Vector3 projectileDirection;
    public Rigidbody2D projectile;
    protected Vector3 startPos;
    protected int projectileDamage;


    protected void fireProjectle(Vector3 endPos, int damage)
    {
        
        projectileDirection = endPos - parent.transform.position;
        Rigidbody2D projectileClone = (Rigidbody2D)Instantiate(projectile, parent.transform.position, parent.transform.rotation);
        projectileClone.velocity = projectileSpeed * projectileDirection;

        projectileClone.GetComponent<ProjectilePhysics>().startPos = parent.transform.position;
        projectileClone.GetComponent<ProjectilePhysics>().skillUser = parent;
        projectileClone.GetComponent<ProjectilePhysics>().ProjectileDamage = damage;
    }


}
