using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : AbstractEnemy
{
    void Start()
    {
        data = new CombatData(30, 5);
        exp = 40;
    }
}
