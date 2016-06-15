using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {


	[SerializeField] private float _timeToWait = 2f;

	// Use this for initialization
	void Start () {
		StartCoroutine(delayedDestroy());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator delayedDestroy() {
		yield return new WaitForSeconds (2f);
		Destroy(gameObject);
	}
}
