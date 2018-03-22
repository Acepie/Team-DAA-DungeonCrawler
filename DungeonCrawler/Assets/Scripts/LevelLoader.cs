using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public string levelToLoad;

	public void LoadLevel() {
		SceneManager.LoadScene(levelToLoad);
	}
}
