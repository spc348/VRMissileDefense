using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{

	public int _totalEnemyPower;
	public int round = 1;
	private int numEnemies = 4;
	private int numPortals = 4;
	public TargetManager targetManager;

	[SerializeField] private ObjectPoolerScript _portalPooler;
	[SerializeField] private ObjectPoolerScript _kamikazeEnemyPooler;

	[SerializeField] private GameObject _canvas;

	public GameObject[] targets;


	public enum EnemyType
	{
		KAMIKAZE,
		SHOOTER
	}

	private bool _isFirstRound = true;


	public List<Enemy> waveEnemies;
	private List <EnemyType> _availableEnemyTypes = new List<EnemyType>();

	void OnEnable ()
	{
		GameEventManager.StartListening ("CheckEnemyList", checkEnemyList);
	}

	void OnDisable ()
	{
		GameEventManager.StopListening ("CheckEnemyList", checkEnemyList);
	}



	// Use this for initialization
	void Start ()
	{
//		StartCoroutine (startNewRound ());
//		_isFirstRound = false;

//		StartCoroutine(openPortals ());
		beginWave();
	}

	void beginWave() {


		//From a pool of possible enemies
		//Grab 

		for (int i = 0; i < 10; i++) {

			StartCoroutine(spawnEnemy ());
//			waveEnemies.Add (_kamikazeEnemyPooler.GetPooledObject ());		
		}



	}

	IEnumerator spawnEnemy() {
		Vector3 spawnPos = new Vector3 (Random.Range (-600f, 600f), Random.Range (100, 1200f), Random.Range (-200f, -300f));
		GameObject portal = _portalPooler.GetPooledObject ();
		portal.transform.position = spawnPos;
		portal.SetActive (true);
		float portalOpenTime = 2f;
		yield return new WaitForSeconds(portalOpenTime);
		GameObject enemy = _kamikazeEnemyPooler.GetPooledObject ();
		enemy.transform.position = portal.transform.position;
		enemy.SetActive (true);
		enemy.GetComponent<Enemy> ().fadeIn();
	}

	IEnumerator openPortals ()
	{
		for (int i = 0; i < numPortals; i++) {

			Vector3 spawnPos = new Vector3 (Random.Range (-600f, 600f), Random.Range (100, 1200f), Random.Range (-500f, -600f));
			GameObject portal = _portalPooler.GetPooledObject ();
			portal.transform.position = spawnPos;
			portal.SetActive (true);
		}

		yield return null;
	}

	public void setCanvasAsParent(GameObject go) {
		go.transform.SetParent (_canvas.transform);
	}



	void checkEnemyList ()
	{
	}




	//	IEnumerator startNewRound ()
	//	{
	//
	//   	yield return StartCoroutine (showRoundText ());
	//		GameObject enemyShipGO = _shipObjectPooler.GetPooledObject ();
	//		enemyShipGO.transform.position = _shipSpawnPoints [Random.Range (0, _shipSpawnPoints.Length)].transform.position;
	//
	//		EnemyShip enemyShip = enemyShipGO.GetComponent<EnemyShip> ();
	//		enemyShips.Add (enemyShip);
	//		enemyShipGO.SetActive (true);
	//		enemyShip.enemyManager = this;
	//		enemyShip.moveToShipDestination (_enemyShipDestination, numEnemies);
	//
	//		Vector3 swarmSpawnPos = _shipSpawnPoints [Random.Range (0, _shipSpawnPoints.Length)].transform.position;
	//		GameObject swarm = _swarmObjectPooler.GetPooledObject ();
	//		swarm.transform.position = swarmSpawnPos;
	//		swarm.GetComponent<EnemySwarm> ().enemyManager = this;
	//		swarm.SetActive (true);
	//	}

//	IEnumerator showRoundText ()
//	{
//
//		if (!_isFirstRound) {
////			_roundText.text = "Round Complete";
////			LeanTween.value (_roundText.gameObject, _roundText.color, Color.black, .5f).setOnUpdate ((Color _c) => {
//				_roundText.color = _c;
//			});
//		}
//		yield return new WaitForSeconds (2f);
////		LeanTween.value (_roundText.gameObject, _roundText.color, Color.clear, .5f).setOnUpdate ((Color _c) => {
////			_roundText.color = _c;
//		});
//		yield return new WaitForSeconds (1f);
//		_roundText.text = "Round " + round;
//		LeanTween.value (_roundText.gameObject, _roundText.color, Color.black, .5f).setOnUpdate ((Color _c) => {
//			_roundText.color = _c;
//		});
//
//		yield return new WaitForSeconds (2f);
//		LeanTween.value (_roundText.gameObject, _roundText.color, Color.clear, .5f).setOnUpdate ((Color _c) => {
//			_roundText.color = _c;
//		});
//
//		_enemyCountText.text = "ENEMIES: " + numEnemies;
//
//	}
}