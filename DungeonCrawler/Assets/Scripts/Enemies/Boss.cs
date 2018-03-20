using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemy {

    public GameObject dropKey;

    public override void OnDeath()
    {
        GameObject keyItem = Instantiate(dropKey);
        keyItem.GetComponent<KeyItem>().keyName = "LevelEndDoorKey";
        keyItem.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}
