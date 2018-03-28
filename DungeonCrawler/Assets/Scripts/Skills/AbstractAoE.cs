using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAoE : AbstractProjectile {

    protected float explosionRadius;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected abstract void detonateAoE();
}
