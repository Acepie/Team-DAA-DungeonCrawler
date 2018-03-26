using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcereress : AbstractEnemy
{

    void Start()
    {
        data = new CombatData(40, 20);
        exp = 50;
    }

}
