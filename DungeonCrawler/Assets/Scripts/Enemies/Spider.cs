using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : AbstractEnemy
{
    public GameObject dropItem;
    [Range(0, 1)]
    public float dropRate;

    public override void OnDeath()
    {
        if (Random.value > 0.5 && dropItem) {
            GameObject item = Instantiate(dropItem, transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}
