using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour {

	[SerializeField] private ObjectPoolerScript _objectPooler;
	public List<GameObject> targets = new List<GameObject>();

	// Use this for initialization
	void Start () {
		makeTargets();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void makeTargets() {
		int numTargets = 10;
		float range  = 10f;

		for (int i = 0; i < numTargets; i++) {
			GameObject tower = _objectPooler.GetPooledObject();
			tower.transform.position = new Vector3 (Random.Range(-range, range), Random.Range(0, range * 10), Random.Range(-range*2, range*2));
			tower.SetActive(true);
		}

	}
}
