using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadStartScene() {
		SceneManager.LoadScene("StartScene");
	}
}
