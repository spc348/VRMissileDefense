using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
	[SerializeField] private GameObject _canvas;

	[SerializeField] private ObjectPoolerScript _portalPooler;
	[SerializeField] private ObjectPoolerScript _kamikazeEnemyPooler;
	[SerializeField] private ObjectPoolerScript _airShooterEnemyPooler;

	[SerializeField] private Text _waveText;
	[SerializeField] private CanvasGroup _gameOverCanvasGroup;

	private bool _gameOver = false;
	public float _totalEnemyHealthForWave;
	public int _wave = 1;
	private int numEnemies = 4;
	private int numPortals = 4;
	public int damageMultiplier = 1;
	public int healthMultiplier = 1;
	private int _startEnemyPoints = 25;
	private int _enemyPoints = 25;

	int _debugEnemyCount;

	public TargetManager targetManager;

	public enum EnemyType
	{
		KAMIKAZE,
		AIR_SHOOTER
	}

	private List<EnemyType> _waveEnemyTypes = new List<EnemyType> ();

	private Dictionary<EnemyType, int> _enemyMarketDictionary = new Dictionary<EnemyType, int> ();
	private Dictionary<EnemyType, ObjectPoolerScript> _enemyPoolerDict = new Dictionary<EnemyType, ObjectPoolerScript> ();

	[SerializeField] private Slider _totalEnemyHealthSlider;

	void OnEnable ()
	{
		Target.OnGameOver += showGameOverMenu;
		Enemy.OnTakeDamage += updateWaveHealthBar;
	}

	void OnDisable ()
	{
		Target.OnGameOver -= showGameOverMenu;
		Enemy.OnTakeDamage -= updateWaveHealthBar;
	}

	// Use this for initialization
	void Start ()
	{	
		initEnemyPoolerDictionary ();
		initEnemyMarketDictionary ();
		StartCoroutine (beginWave ());
	}

	IEnumerator beginWave ()
	{
		getEnemiesForRound ();

		for (int i = 0; i < _waveEnemyTypes.Count; i++) {
			spawnEnemy (_waveEnemyTypes [i]);	
			yield return null;
		}

		_totalEnemyHealthSlider.maxValue = _totalEnemyHealthForWave;
		LeanTween.value (_totalEnemyHealthSlider.gameObject, 0f, _totalEnemyHealthSlider.maxValue, 1f).setOnUpdate ((float _h) => {
			_totalEnemyHealthSlider.value = _h;	
		}); 

	}

	void spawnEnemy (EnemyType enemyType)
	{
		Vector3 spawnPos = new Vector3 (Random.Range (-600f, 600f), Random.Range (100, 600f), Random.Range (200f, 300f));
		GameObject portal = _portalPooler.GetPooledObject ();
		portal.transform.position = spawnPos;
		portal.SetActive (true);

		GameObject enemy = _enemyPoolerDict [enemyType].GetPooledObject ();
		enemy.transform.position = portal.transform.position;
		enemy.SetActive (true);
		_totalEnemyHealthForWave += enemy.GetComponent<Enemy> ().Health;
		enemy.gameObject.name = "WaveEnemy";
		enemy.GetComponent<Enemy> ().fadeIn ();
	}


	void getEnemiesForRound ()
	{
		List<EnemyType> keyList = new List<EnemyType> (_enemyMarketDictionary.Keys);
		//This allows manager to overspend, should probably fix. 
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
		_enemyPoolerDict.Add (EnemyType.AIR_SHOOTER, _airShooterEnemyPooler);
	}

	public void initEnemyMarketDictionary ()
	{
		_enemyMarketDictionary.Add (EnemyType.KAMIKAZE, 5);
	}

	IEnumerator endWave ()
	{
		_waveEnemyTypes.Clear ();
		_totalEnemyHealthForWave = 0;
		adjustEnemyDicitonary (++_wave);

		float timeTilNextRound = 5f;
		while (timeTilNextRound > 0) {
			timeTilNextRound -= Time.deltaTime;
			_waveText.text = "NEXT WAVE IN: " + Mathf.Round (timeTilNextRound).ToString ();
			yield return null;
		}
			
		setWaveText ();
		StartCoroutine (beginWave ());
	}

	void adjustEnemyDicitonary (int wave)
	{
		_enemyPoints = Mathf.RoundToInt (_startEnemyPoints * Mathf.Pow (1.1f, (float)wave));


		//multiply by 1.1^wave
		//scale monster health semi-exponentially
		//scale monster damage output semi-logaritmically
		switch (wave) {

		case 3:
			_enemyMarketDictionary.Add (EnemyType.AIR_SHOOTER, 10);
			break;
		}
	}

	public void updateWaveHealthBar (float damage)
	{
		if (_totalEnemyHealthSlider.value - damage > 0) {
			_totalEnemyHealthSlider.value -= damage;
		} else {
			_totalEnemyHealthSlider.value = 0;
			StartCoroutine (endWave ());
		}
	}

	public void setWaveText ()
	{
		_waveText.text = "WAVE: " + _wave.ToString ();
	}

	public void showGameOverMenu ()
	{
		_gameOver = true;
		_gameOverCanvasGroup.interactable = true;
		_gameOverCanvasGroup.blocksRaycasts = true;
		LeanTween.value (_gameOverCanvasGroup.gameObject, _gameOverCanvasGroup.alpha, 1f, 1f).setOnUpdate ((float _a) => {
			_gameOverCanvasGroup.alpha = _a;
		});	
	}
}