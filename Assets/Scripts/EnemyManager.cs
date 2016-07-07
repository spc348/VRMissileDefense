using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{

	public int round = 1;
	private int numEnemies = 2;

	[SerializeField] private ObjectPoolerScript _objectPooler;
	[SerializeField] private GameObject _enemyShipDestination;

	public GameObject[] targets;
	[SerializeField] private List<EnemyShip> enemyShips = new List<EnemyShip> ();
	public List<Enemy> enemies = new List<Enemy> ();
	[SerializeField] private GameObject[] _shipSpawnPoints;


//	public delegate void ClickAction ();
//
//	public static event ClickAction OnClicked;
//
//	public delegate void CheckEnemies ();
//
//	public static event CheckEnemies OnCheckEnemies;

	void OnEnable ()
	{
		GameEventManager.StartListening ("CheckEnemyList", CheckEnemyList);
	}

	void OnDisable ()
	{
		GameEventManager.StopListening ("CheckEnemyList", CheckEnemyList);
	}

	// Use this for initialization
	void Start ()
	{
		StartNewRound ();
	}

	public void spawnEnemies ()
	{
//		StartCoroutine(spawnEnemiesCoroutine());
	}

	void StartNewRound ()
	{
		GameObject enemyShipGO = _objectPooler.GetPooledObject ();
		enemyShipGO.transform.position = _shipSpawnPoints [Random.Range (0, _shipSpawnPoints.Length)].transform.position;

		EnemyShip enemyShip = enemyShipGO.GetComponent<EnemyShip> ();
		enemyShipGO.SetActive (true);
		enemyShip.enemyManager = this;
		enemyShip.moveToShipDestination (_enemyShipDestination, numEnemies);
	}

	void CheckEnemyList ()
	{
		print ("enemiesCount: " + enemies.Count);
		if (enemies.Count == 0) {
			print ("enemies out");
		}
	}
}