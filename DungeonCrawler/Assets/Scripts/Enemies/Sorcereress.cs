using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcereress : AbstractEnemy {
	SpriteRenderer sr;
	Animator anim;
	public Sprite combatsprite;
	public RuntimeAnimatorController combatAnimControl;

	void Awake(){
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
	}


	public override void swapToCombatSprite(){
		sr.sprite = combatsprite;
		anim.runtimeAnimatorController = combatAnimControl;
	}
}
