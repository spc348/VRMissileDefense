using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LockOnManager : MonoBehaviour
{

	public GameObject t;
	[SerializeField] private List<LockOnReticle> pooledObjects = new List<LockOnReticle> ();

	// Use this for initialization
	void Start ()
	{
		
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.L)) {
			lockOn (t);
		}
	}

	public void lockOn(GameObject targ) {
		print ("target: " + targ);
		LockOnReticle reticle = getLockOnReticle ();
		reticle.target = targ;
		reticle.gameObject.SetActive (true);
		LeanTween.scale (reticle.gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutExpo);
	}

	public LockOnReticle getLockOnReticle ()
	{
		print (gameObject.name + "is Getting");
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].gameObject.activeInHierarchy) {
				return pooledObjects [i];
			}
		}

		return null;

	}

}
