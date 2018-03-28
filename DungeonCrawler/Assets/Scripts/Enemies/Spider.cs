using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : AbstractEnemy
{
    void Start()
    {
        data = new CombatData(50, 10);
        exp = 40;
    }
}
