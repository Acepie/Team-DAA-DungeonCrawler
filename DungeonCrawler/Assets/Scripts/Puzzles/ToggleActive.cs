using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBySwitch : SwitchActivated
{
    public override void OnSwitch()
    {
        gameObject.SetActive(false);
    }
}
