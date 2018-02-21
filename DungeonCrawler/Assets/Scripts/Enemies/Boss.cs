using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemy {

    public override void OnDeath()
    {
        GameObject keyItem = (GameObject)Instantiate(Resources.Load("Key"));
        keyItem.GetComponent<KeyItem>().keyName = "LevelEndDoorKey";
        keyItem.transform.position = this.transform.position;
        Destroy(this.gameObject);
        
    }
}
