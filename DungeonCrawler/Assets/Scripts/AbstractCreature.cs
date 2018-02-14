using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCreature : MonoBehaviour {

	public int health;
	public float speed;

	protected bool inCombat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void Move(float speed){
	}

	protected virtual void UnderAttack(int damageTaken){}

	protected virtual void MakeAttack(){}

	protected virtual void OnDeath(){}
}
