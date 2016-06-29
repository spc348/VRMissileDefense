using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour {


	[SerializeField] private GameObject _shipDestination;
	[SerializeField] private EnemyManager _enemyManager;
	private float _distanceFromDestination = Mathf.Infinity;
	private float _radius = 100f;
	private float _rotateSpeed = 20f;
	private bool _isInRadius = false;

	// Use this for initialization
	void Start () {
		moveToShipDestination();
	}

	void Update() {
		if (_isInRadius) {
			transform.RotateAround(_shipDestination.transform.position, Vector3.up, Time.deltaTime * _rotateSpeed);
		}
	}
		
	void moveToShipDestination() {
		StartCoroutine(moveShipToDestinationCoroutine());
	}

	IEnumerator moveShipToDestinationCoroutine() {
		LeanTween.move(gameObject, _shipDestination.transform.position, 7f);
//		while (LeanTween.isTweening(gameObject)) {
//			yield return null;
//		}

		while (_distanceFromDestination > _radius) {
			_distanceFromDestination = Vector3.Distance(transform.position, _shipDestination.transform.position);

			yield return null;
		}
		LeanTween.cancel(gameObject);

		_isInRadius = true;

		_enemyManager.spawnEnemies();
	}


}
