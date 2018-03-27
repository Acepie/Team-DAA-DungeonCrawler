using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{

    private static CombatData data;
    public Text healthText;
    public Text eventText;
    public Slider playerHealthSlider;
    public GameObject arrow;

    private Canvas canv;
    private bool fadeTextPlaying;
    private IEnumerator coroutine;


    // Use this for initialization
    void Start()
    {
        canv = GameObject.Find("Canvas").GetComponent<Canvas>();
        data = GameObject.Find("player").GetComponent<PlayerController>().data;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + data.currentHealth.ToString() + "/" + data.maxHealth.ToString();
        playerHealthSlider.value = ((float)data.currentHealth) / ((float)data.maxHealth);
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

    public GameObject drawCombatArrows(AbstractCreature target)
    {
        var newArrow = Instantiate(this.arrow, new Vector3(0,0,0), Quaternion.identity);
        //newArrow.transform.parent = this.canv.transform;
        newArrow.transform.SetParent(this.canv.transform, false);
        Vector3 camPos = Camera.main.WorldToScreenPoint(target.transform.position);
        camPos.y += 13;
        camPos.x -= 13;
        newArrow.GetComponent<Image>().rectTransform.position = camPos;
        return newArrow;
    }
}
