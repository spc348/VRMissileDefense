using UnityEngine;
using System.Collections;

public class Tenticle : MonoBehaviour {

	float r = 0;
	// Use this for initialization
	void Start () {
		r = Random.Range (.1f, 9f);
	}
	
	// Update is called once per frame
	void Update () {
		
		float y = (Mathf.Cos(Time.time * r) * 1f) - 2f;
		transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z); 
	}
}
