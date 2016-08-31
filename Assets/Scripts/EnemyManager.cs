using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
	[SerializeField] private GameObject _canvas;

	[SerializeField] private ObjectPoolerScript _portalPooler;
	[SerializeField] private ObjectPoolerScript _kamikazeEnemyPooler;

	private bool _isFirstRound = true;
	public float _totalEnemyHealthForWave;
	public int round = 1;
	private int numEnemies = 4;
	private int numPortals = 4;
	public int damageMultiplier = 1;
	public int healthMultiplier = 1;

	public TargetManager targetManager;

	public enum EnemyType
	{
		KAMIKAZE,
		SHOOTER
	}


	public GameObject[] targets;
	private List<EnemyType> _waveEnemyTypes;

	private Dictionary<EnemyType, int> _enemyDictionary = new Dictionary<EnemyType, int> ();
	private Dictionary<EnemyType, ObjectPoolerScript> _enemyPoolerDict = new Dictionary<EnemyType, ObjectPoolerScript> ();

	private int _enemyPoints = 5;

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
		initEnemyPoolerDictionary ();
		initEnemyDictionary ();
		getEnemiesForRound ();
		beginWave ();
	}

	void beginWave ()
	{
		for (int i = 0; i < _waveEnemyTypes.Count; i++) {
			StartCoroutine (spawnEnemy (_waveEnemyTypes[i]));
		}
		_waveEnemyTypes.Clear ();
	}

	IEnumerator spawnEnemy (EnemyType enemyType)
	{
		Vector3 spawnPos = new Vector3 (Random.Range (-600f, 600f), Random.Range (100, 1200f), Random.Range (-200f, -300f));
		GameObject portal = _portalPooler.GetPooledObject ();
		portal.transform.position = spawnPos;
		portal.SetActive (true);
		float portalOpenTime = 2f;
		yield return new WaitForSeconds (portalOpenTime);
		GameObject enemy = _enemyPoolerDict[enemyType].GetPooledObject ();
		enemy.transform.position = portal.transform.position;
		enemy.SetActive (true);
		enemy.GetComponent<Enemy> ().fadeIn ();
		_totalEnemyHealthForWave += enemy.GetComponent<Enemy> ().Health;
	}


	void checkEnemyList ()
	{
	}

	void getEnemiesForRound ()
	{
		List<EnemyType> keyList = new List<EnemyType> (_enemyDictionary.Keys);
		while (_enemyPoints > 0) {
			int index = Random.Range (0, _enemyDictionary.Count);
			EnemyType randomEnemy = keyList [index];
			_waveEnemyTypes.Add (randomEnemy);
			_enemyPoints--;
		}
	}

	public void initEnemyPoolerDictionary() {
		_enemyPoolerDict.Add (EnemyType.KAMIKAZE, _kamikazeEnemyPooler);
	}

	public void initEnemyDictionary ()
	{
		_enemyDictionary.Add (EnemyType.KAMIKAZE, 5);
	}

	public void endWave ()
	{
	
	}

	void adjustEnemyDicitonary (int wave)
	{
		//scale monster health semi-exponentially
		//scale monster damage output semi-logaritmically
		switch (wave) {

		case 1:
			break;		
		case 2:
			_enemyDictionary.Add (EnemyType.SHOOTER, 10);
			break;
		case 3:
			_enemyDictionary.Remove (EnemyType.KAMIKAZE);
			break;
		}


	}

}