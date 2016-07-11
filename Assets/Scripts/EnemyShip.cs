using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour
{

	[SerializeField] private ObjectPoolerScript _objectPooler;
	[SerializeField] private GameObject _worldSpaceCanvas;

	[SerializeField] private GameObject _shipDestination;
	[SerializeField] private GameObject _shipExitDestination;
	[SerializeField] public  EnemyManager enemyManager;
	public bool hasDroppedOffAllEnemies = false;
	private bool _isInRadius = false;
	private float _distanceFromDestination = Mathf.Infinity;
	private float _radius = 100f;
	private float _rotateSpeed = 20f;

	void OnEnable() {
		hasDroppedOffAllEnemies = false;	
	}

	// Use this for initialization
	void Start ()
	{
		_worldSpaceCanvas = GameObject.Find ("WorldSpaceCanvas");
		_shipExitDestination = GameObject.Find ("EnemyShipExitDestination");
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
//			enemy.Target = enemyManager.targets [Random.Range (0, enemyManager.targets.Length)];

			enemy.Target = enemyManager.targetManager.targetGOs [Random.Range (0, enemyManager.targetManager.targetGOs.Count)];

			enemy.initialize ();
			enemy.enemyManager = enemyManager;
			enemyManager.enemies.Add (enemy);

			yield return new WaitForSeconds (Random.Range (1f, 5f));
		}

		StartCoroutine (exit ());
		hasDroppedOffAllEnemies = true;
	}

	IEnumerator exit ()
	{
		_isInRadius = false;
		LeanTween.move (gameObject, _shipExitDestination.transform.position, 5f);
		yield return new WaitForSeconds (5f);
		gameObject.SetActive (false);
	}

}
