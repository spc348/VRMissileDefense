using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	[SerializeField] private AudioSource _audSource;

	// Use this for initialization
	void Start () {
		StartCoroutine (delayedStart());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator delayedStart() {
		yield return new WaitForSeconds (5f);
		_audSource.Play ();
	}
}
