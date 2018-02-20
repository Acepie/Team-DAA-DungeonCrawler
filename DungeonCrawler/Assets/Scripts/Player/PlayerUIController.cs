using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

	private static PlayerController player;
	public Text healthText;
	public Text eventText;
  public Image playerHealthSlider;

	private bool fadeTextPlaying;
	private IEnumerator coroutine;


	// Use this for initialization
	void Start () {
		player = GameObject.Find("player").GetComponent<PlayerController>();
		
	}
	
	// Update is called once per frame
	void Update () {
		healthText.text = "Health: " + player.currentHealth.ToString() + "/" + player.maxHealth.ToString();
        //playerHealthSlider.fillAmount = ((float)player.currentHealth) / ((float)player.maxHealth);
	}

	public void PickupEvent(string s)
	{
		if (fadeTextPlaying)
		{
			StopCoroutine(coroutine); // Stops coroutine in case multiple items are picked up in small time window
		}

		eventText.text = s;
		coroutine = fadeText(eventText, 0.5f);
		StartCoroutine(coroutine);
	}

   private IEnumerator fadeText(Text t, float duration)
	{
		fadeTextPlaying = true;
		float i = 1;
		while (i >= 0) 
		{
			t.color = new Color(t.color.r, t.color.g, t.color.b, i);
			i -= 0.1f;
			yield return new WaitForSeconds(duration);
		}
		t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
		fadeTextPlaying = false;
	}
}
