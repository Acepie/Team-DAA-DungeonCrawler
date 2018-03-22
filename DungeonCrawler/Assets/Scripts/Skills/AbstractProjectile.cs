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


    protected void fireProjectle(Vector3 endPos)
    {
        
        projectileDirection = endPos - skillUser.transform.position;
        Rigidbody2D projectileClone = (Rigidbody2D)Instantiate(projectile, skillUser.transform.position, skillUser.transform.rotation);
        projectileClone.velocity = projectileSpeed * projectileDirection;
        projectileClone.GetComponent<ProjectilePhysics>().startPos = skillUser.transform.position;
        projectileClone.GetComponent<ProjectilePhysics>().skillUser = skillUser;
    }


}
