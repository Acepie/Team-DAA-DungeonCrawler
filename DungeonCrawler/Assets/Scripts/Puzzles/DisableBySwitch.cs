using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : SwitchActivated
{
    public override void OnSwitch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
