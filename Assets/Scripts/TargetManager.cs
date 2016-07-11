using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour {

	[SerializeField] private ObjectPoolerScript _objectPooler;
	public List<GameObject> targetGOs = new List<GameObject>();
	public List<Tower> towers = new List<Tower>();


	// Use this for initialization
	void Start () {
		makeTargets();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void makeTargets() {
		int numTargets = 3;
		float range  = 10f;

		for (int i = 0; i < numTargets; i++) {
			GameObject tower = _objectPooler.GetPooledObject();
			tower.transform.position = new Vector3 (Random.Range(-range*15, range*15), Random.Range(0, range * 10), Random.Range(-range*5, range*5));
			tower.GetComponent<Tower> ().targetManager = this;
			tower.SetActive(true);
			targetGOs.Add (tower);
		}

	}


}
