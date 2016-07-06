using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour
{

	[SerializeField] private ObjectPoolerScript _objectPooler;
	[SerializeField] private GameObject _worldSpaceCanvas;

	[SerializeField] private GameObject _shipDestination;
	[SerializeField] public  EnemyManager enemyManager;
	private float _distanceFromDestination = Mathf.Infinity;
	private float _radius = 100f;
	private float _rotateSpeed = 20f;
	private bool _isInRadius = false;



	// Use this for initialization
	void Start ()
	{
		_worldSpaceCanvas = GameObject.Find ("WorldSpaceCanvas");
	}

	void Update ()
	{
		if (_isInRadius) {
			transform.RotateAround (_shipDestination.transform.position, Vector3.up, Time.deltaTime * _rotateSpeed);
		}
	}

	public void moveToShipDestination (GameObject shipDest, int numEnemies)
	{
		StartCoroutine (moveShipToDestinationCoroutine (shipDest, numEnemies));
	}

	IEnumerator moveShipToDestinationCoroutine (GameObject shipDest, int numEnemies)
	{
		_shipDestination = shipDest;
		LeanTween.move (gameObject, _shipDestination.transform.position, 7f);
		while (_distanceFromDestination > _radius) {
			_distanceFromDestination = Vector3.Distance (transform.position, _shipDestination.transform.position);

			yield return null;
		}
		LeanTween.cancel (gameObject);

		_isInRadius = true;
		StartCoroutine (spawnEnemiesCoroutine (numEnemies));
	}

	IEnumerator spawnEnemiesCoroutine (int numEnemies)
	{

		for (int i = 0; i < numEnemies; i++) {
			//			Vector3 spawnPos = new Vector3 (Random.Range(-50f, 50f), Random.Range(30f, 100f), Random.Range(-50f, 50f));
			Vector3 spawnPos = transform.position;
			GameObject enemyGO = _objectPooler.GetPooledObject ();
			enemyGO.name = "Enemy" + i.ToString ();
			enemyGO.transform.position = spawnPos;
			enemyGO.transform.SetParent (_worldSpaceCanvas.transform);
			Enemy enemy = enemyGO.GetComponent<Enemy> ();
			enemy.Target = enemyManager.targets [Random.Range (0, enemyManager.targets.Length)];
			enemy.initialize ();
			enemyManager.enemies.Add (enemy);
			yield return new WaitForSeconds (Random.Range(1f, 5f));
		}

	}

}
