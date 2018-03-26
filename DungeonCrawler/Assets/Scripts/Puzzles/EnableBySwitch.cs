using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBySwitch : SwitchActivated
{
    public override void OnSwitch()
    {
        gameObject.SetActive(true);
    }
}
