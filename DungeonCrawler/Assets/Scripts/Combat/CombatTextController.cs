using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombatTextController : MonoBehaviour {


	private Text t;
	private float startTime;
	private float displayTime = 2.0f;
	private bool needsToDisable;

	// Use this for initialization
	void Start () {
		this.t = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		// determine if we need to disable the text field. 
		if (needsToDisable && (Time.time - startTime >= displayTime)) {
			// If we've waited long enough
			t.text = "";
			t.enabled = false;
		}
	}

	public void updateText(string newText) {
		enableText();
		t.text = newText;
	}

	public void displayWinner(string newText) {
		enableText();
		t.text = newText;
		this.startTime = Time.time;
		needsToDisable = true;

	}

	public void enableText() {
		t.enabled = true;
		needsToDisable = false;
	}

	public void disableText() {
		t.enabled = false;
	}
}
