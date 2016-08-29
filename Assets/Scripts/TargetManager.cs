using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : Singleton<TargetManager>
{

	[SerializeField] private ObjectPoolerScript _objectPooler;
	public List<GameObject> targetGOs = new List<GameObject> ();
	public List<Tower> towers = new List<Tower> ();


	// Use this for initialization
	void Awake ()
	{
		makeTargets ();
	}

	void makeTargets ()
	{
		int numTargets = 3;
		float range = 10f;

		for (int i = 0; i < numTargets; i++) {
			GameObject tower = _objectPooler.GetPooledObject ();
			tower.transform.position = new Vector3 (Random.Range (-range * 15, range * 15), Random.Range (0, range * 10), Random.Range (-range * 5, range * 5));
			tower.GetComponent<Tower> ().targetManager = this;
			tower.SetActive (true);
			targetGOs.Add (tower);
		}
	
	}

	public GameObject getTarget ()
	{
		GameObject target = targetGOs [Random.Range (0, targetGOs.Count)];
		return target;
	}


}
