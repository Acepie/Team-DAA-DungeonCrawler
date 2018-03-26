using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombatTextController : MonoBehaviour
{


    private Text t;
    private float displayTime = 2.0f;

    // Use this for initialization
    void Start()
    {
        this.t = this.GetComponent<Text>();
    }

    public void updateText(string newText)
    {
        enableText();
        t.text = newText;
    }

    public void displayWinner(string newText)
    {
        updateText(newText);
        StartCoroutine("disableText");

    }

    public void enableText()
    {
        t.enabled = true;
    }

    IEnumerator disableText()
    {
        yield return new WaitForSeconds(displayTime);
        t.enabled = false;
    }

}
