using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public Rect position;
    public Texture2D actionBarTexture;

    void OnGUI()
    {
        drawActionBar();
    }

    private void drawActionBar()
    {
        GUI.DrawTexture(new Rect(position.x * Screen.width, position.y * Screen.height, Screen.width * position.width, Screen.height * position.height), actionBarTexture);
    }
    
}
