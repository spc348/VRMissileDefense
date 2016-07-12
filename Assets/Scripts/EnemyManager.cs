using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EnemyManager : MonoBehaviour
{

	public int round = 1;
	private int numEnemies = 4;
	public TargetManager targetManager;

	[SerializeField] private ObjectPoolerScript _objectPooler;
	[SerializeField] private GameObject _enemyShipDestination;

	[SerializeField] private GameObject _swarmPrefab;

	public GameObject[] targets;
	[SerializeField] private List<EnemyShip> enemyShips = new List<EnemyShip> ();
	public List<Enemy> enemies = new List<Enemy> ();
	[SerializeField] private GameObject[] _shipSpawnPoints;

	[SerializeField] private TextMeshProUGUI _enemyCountText;
	[SerializeField] private TextMeshProUGUI _roundText;

	private bool _isFirstRound = true;

	//	public delegate void ClickAction ();
	//
	//	public static event ClickAction OnClicked;
	//
	//	public delegate void CheckEnemies ();
	//
	//	public static event CheckEnemies OnCheckEnemies;

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
		StartCoroutine (startNewRound ());
		_isFirstRound = false;
	}

	public void spawnEnemies ()
	{
//		StartCoroutine(spawnEnemiesCoroutine());
	}

	IEnumerator startNewRound ()
	{

		yield return StartCoroutine (showRoundText ());
		GameObject enemyShipGO = _objectPooler.GetPooledObject ();
		enemyShipGO.transform.position = _shipSpawnPoints [Random.Range (0, _shipSpawnPoints.Length)].transform.position;

		EnemyShip enemyShip = enemyShipGO.GetComponent<EnemyShip> ();
		enemyShips.Add (enemyShip);
		enemyShipGO.SetActive (true);
		enemyShip.enemyManager = this;
		enemyShip.moveToShipDestination (_enemyShipDestination, numEnemies);

		Vector3 swarmSpawnPos = _shipSpawnPoints [Random.Range (0, _shipSpawnPoints.Length)].transform.position;
		GameObject swarm = Instantiate (_swarmPrefab, swarmSpawnPos, Quaternion.identity) as GameObject;
//		swarm.GetComponent<EnemySwarm>().
	}

	void checkEnemyList ()
	{
		_enemyCountText.text = "ENEMIES: " + enemies.Count.ToString ();
		if (allEnemiesDroppedOff ()) {
			if (enemies.Count == 0) {
				round++;
				numEnemies *= 2;
				StartCoroutine (startNewRound ());
			}
		}
	}

	bool allEnemiesDroppedOff ()
	{
		bool allDroppedOff = true;
		for (int i = 0; i < enemyShips.Count; i++) {
			if (!enemyShips [i].hasDroppedOffAllEnemies) {
				allDroppedOff = false;
			}
		}
		return allDroppedOff;
	}

	IEnumerator showRoundText ()
	{

		if (!_isFirstRound) {
			_roundText.text = "Round Complete";
			LeanTween.value (_roundText.gameObject, _roundText.color, Color.black, .5f).setOnUpdate ((Color _c) => {
				_roundText.color = _c;
			});
		}
		yield return new WaitForSeconds (2f);
		LeanTween.value (_roundText.gameObject, _roundText.color, Color.clear, .5f).setOnUpdate ((Color _c) => {
			_roundText.color = _c;
		});
		yield return new WaitForSeconds (1f);
		_roundText.text = "Round " + round;
		LeanTween.value (_roundText.gameObject, _roundText.color, Color.black, .5f).setOnUpdate ((Color _c) => {
			_roundText.color = _c;
		});

		yield return new WaitForSeconds (2f);
		LeanTween.value (_roundText.gameObject, _roundText.color, Color.clear, .5f).setOnUpdate ((Color _c) => {
			_roundText.color = _c;
		});

		_enemyCountText.text = "ENEMIES: " + numEnemies;

	}
}