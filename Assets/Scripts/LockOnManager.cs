using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LockOnManager : MonoBehaviour
{

	public GameObject t;
	[SerializeField] private LockOnReticle _lockOnReticle;
	//	[SerializeField] private List<LockOnReticle> pooledObjects = new List<LockOnReticle> ();

	private bool _doingLockOn = false;
	private bool _isLockedOn = false;
	float _reticleLargeScale = 10f;

	 


	// Use this for initialization
	void Start ()
	{

	}

	void Update ()
	{
//		if (Input.GetKey (KeyCode.L)) {
//			startLockOnProcess (t);
//		} else {
//			endLockOnProcess ();
//		}
	}

	public void startLockOnProcess (GameObject targ)
	{
//		StartCoroutine(lockOnCoroutine());


//		_lockOnReticle.scale = Vector3.one  * _reticleLargeScale;

//		print ("target: " + targ);
//		LockOnReticle reticle = getLockOnReticle ();

		if (!_doingLockOn) {
				
			_lockOnReticle.target = targ;
			_lockOnReticle.gameObject.SetActive (true);
			LeanTween.scale (_lockOnReticle.gameObject, Vector3.one, 1f).setOnComplete(lockOn);
			_doingLockOn = true;

		}
	}

	void lockOn() {
		_lockOnReticle.GetComponent<Image>().color = Color.red;
		WeaponsManager.Instance.isLockedOn = true;
	}

	public void endLockOnProcess ()
	{
		LeanTween.cancel (_lockOnReticle.gameObject);
		_lockOnReticle.gameObject.SetActive (false);
		_doingLockOn = false;
		_lockOnReticle.transform.localScale = Vector3.one * _reticleLargeScale;
		_lockOnReticle.GetComponent<Image>().color = Color.white;

	}

	//	public LockOnReticle getLockOnReticle ()
	//	{
	//		print (gameObject.name + "is Getting");
	//		for (int i = 0; i < pooledObjects.Count; i++) {
	//			if (!pooledObjects [i].gameObject.activeInHierarchy) {
	//				return pooledObjects [i];
	//			}
	//		}
	//
	//		return null;
	//
	//	}

}
