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
	private int _enemyPoints = 25;

	int _debugEnemyCount;

	public TargetManager targetManager;

	public enum EnemyType
	{
		KAMIKAZE,
		SHOOTER
	}


	public GameObject[] targets;
	private List<EnemyType> _waveEnemyTypes = new List<EnemyType> ();

	private Dictionary<EnemyType, int> _enemyMarketDictionary = new Dictionary<EnemyType, int> ();
	private Dictionary<EnemyType, ObjectPoolerScript> _enemyPoolerDict = new Dictionary<EnemyType, ObjectPoolerScript> ();

	[SerializeField] private Slider _totalEnemyHealthSlider;

	void OnEnable ()
	{
		GameEventManager.StartListening ("CheckEnemyList", checkEnemyList);
		Enemy.OnTakeDamage += updateWaveHealthBar;
	}

	void OnDisable ()
	{
		GameEventManager.StopListening ("CheckEnemyList", checkEnemyList);
		Enemy.OnTakeDamage += updateWaveHealthBar;
	}



	// Use this for initialization
	void Start ()
	{	
		initEnemyPoolerDictionary ();
		initEnemyMarketDictionary ();
		getEnemiesForRound ();
		StartCoroutine(beginWave ());
	}

	IEnumerator beginWave ()
	{
		_debugEnemyCount = 0;
		for (int i = 0; i < _waveEnemyTypes.Count; i++) {
			spawnEnemy (_waveEnemyTypes [i]);	
			yield return null;
		}

		_totalEnemyHealthSlider.maxValue = _totalEnemyHealthForWave;
		_totalEnemyHealthSlider.value = _totalEnemyHealthForWave;

	}

	void spawnEnemy (EnemyType enemyType)
	{
		Vector3 spawnPos = new Vector3 (Random.Range (-600f, 600f), Random.Range (100, 600f), Random.Range (200f, 300f));
		GameObject portal = _portalPooler.GetPooledObject ();
		portal.transform.position = spawnPos;
		portal.SetActive (true);
//		float portalOpenTime = 1f;
//		yield return new WaitForSeconds (portalOpenTime);

		GameObject enemy = _enemyPoolerDict [enemyType].GetPooledObject ();
		enemy.transform.position = portal.transform.position;
		enemy.SetActive (true);
		_totalEnemyHealthForWave += enemy.GetComponent<Enemy> ().Health;
		enemy.gameObject.name = "WaveEnemy" + _debugEnemyCount.ToString ();
		enemy.GetComponent<Enemy> ().fadeIn ();


	}


	void checkEnemyList ()
	{
	}

	void getEnemiesForRound ()
	{
		List<EnemyType> keyList = new List<EnemyType> (_enemyMarketDictionary.Keys);
		while (_enemyPoints > 0) {
			int index = Random.Range (0, _enemyMarketDictionary.Count);
			EnemyType randomEnemy = keyList [index];
			_waveEnemyTypes.Add (randomEnemy);
			_enemyPoints -= _enemyMarketDictionary [randomEnemy];
		}
	}

	public void initEnemyPoolerDictionary ()
	{
		_enemyPoolerDict.Add (EnemyType.KAMIKAZE, _kamikazeEnemyPooler);
	}

	public void initEnemyMarketDictionary ()
	{
		_enemyMarketDictionary.Add (EnemyType.KAMIKAZE, 5);
	}

	public void endWave ()
	{
		print ("Wave complete");
//		_waveEnemyTypes.Clear ();

	}

	void adjustEnemyDicitonary (int wave)
	{
		//scale monster health semi-exponentially
		//scale monster damage output semi-logaritmically
		switch (wave) {

		case 1:
			break;		
		case 2:
			_enemyMarketDictionary.Add (EnemyType.SHOOTER, 5);
			break;
		case 3:
			_enemyMarketDictionary.Remove (EnemyType.KAMIKAZE);
			break;
		}
	}

	public void updateWaveHealthBar (float damage)
	{

		if (_totalEnemyHealthSlider.value - damage > 0) {
			_totalEnemyHealthSlider.value -= damage;
		} else {
			_totalEnemyHealthSlider.value = 0;
			endWave ();
		}
	}

}