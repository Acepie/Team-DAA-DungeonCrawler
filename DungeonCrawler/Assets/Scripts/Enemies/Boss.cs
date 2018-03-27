using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemy {

    public GameObject dropKey;

    void Start()
    {
        data = new CombatData(125, 10);
        exp = 150;
    }

    public override void OnDeath()
    {
        GameObject keyItem = Instantiate(dropKey, transform.position, Quaternion.identity);
        keyItem.GetComponent<KeyItem>().keyName = "LevelEndDoorKey";
        Destroy(this.gameObject);
    }
}
