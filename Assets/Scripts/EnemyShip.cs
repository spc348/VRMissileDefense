using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour {


	[SerializeField] private GameObject _shipDestination;
	[SerializeField] private EnemyManager _enemyManager;

	// Use this for initialization
	void Start () {
		moveToShipDestination();
	}
		
	IEnumerator moveShipToDestinationCoroutine() {
		LeanTween.move(gameObject, _shipDestination.transform.position, 7f);
		while (LeanTween.isTweening(gameObject)) {
			yield return null;
		}

		_enemyManager.spawnEnemies();
	}

	void moveToShipDestination() {

		StartCoroutine(moveShipToDestinationCoroutine());
	}
}
