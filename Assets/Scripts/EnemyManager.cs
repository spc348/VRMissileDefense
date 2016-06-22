using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	[SerializeField] private ObjectPoolerScript _objectPooler;
	[SerializeField] private GameObject _worldSpaceCanvas;
	public int numEnemies = 100;

	public GameObject[] targets;
	[SerializeField] private List<Enemy> enemies = new List<Enemy>();



	// Use this for initialization
	void Start () {
		StartCoroutine(spawnEnemies());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator spawnEnemies() {

		for (int i = 0; i < numEnemies; i++) {
			Vector3 spawnPos = new Vector3 (Random.Range(-50f, 50f), Random.Range(30f, 100f), Random.Range(-50f, 50f));
			GameObject enemyGO = _objectPooler.GetPooledObject();
			enemyGO.name = enemyGO.name + i.ToString();
			enemyGO.transform.position = transform.position;
			enemyGO.transform.SetParent(_worldSpaceCanvas.transform);
			Enemy enemy = enemyGO.GetComponent<Enemy>();
			enemy.Target = targets[Random.Range(0,targets.Length-1)];
			enemyGO.SetActive(true);
			enemies.Add(enemy);
			yield return new WaitForSeconds (.1f);
		}
		
	}

	void checkEnemyCount() {


	}
}
