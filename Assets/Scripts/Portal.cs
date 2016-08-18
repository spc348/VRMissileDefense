using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.Find ("Player");
		transform.LookAt (2*transform.position - player.transform.position);
		open ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void open() {
		LeanTween.scale(gameObject, Vector3.one * 20f, 5f).setEase(LeanTweenType.easeInOutExpo);
	}
}
