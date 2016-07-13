﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySwarm : MonoBehaviour
{

	public EnemyManager enemyManager;

	private List<SwarmEnemy> _enemiesInSwarm = new List<SwarmEnemy> ();
	private GameObject _worldSpaceCanvas;

	void Awake ()
	{
		_worldSpaceCanvas = GameObject.Find ("WorldSpaceCanvas");
	}

	void OnEnable ()
	{
		_enemiesInSwarm.AddRange (GetComponentsInChildren<SwarmEnemy> ());	
		enemyManager.enemies.AddRange (GetComponentsInChildren<SwarmEnemy> ());
		initEnemies ();
	}

	public void initEnemies ()
	{
		setEnemiesTarget ();
		for (int i = 0; i < _enemiesInSwarm.Count; i++) {
			_enemiesInSwarm [i].enemyManager = enemyManager;
			_enemiesInSwarm [i].gameObject.transform.SetParent (_worldSpaceCanvas.transform);
		}
	}

	public void setEnemiesTarget ()
	{
		GameObject target = enemyManager.targetManager.targetGOs [Random.Range (0, enemyManager.targetManager.targetGOs.Count)];

		for (int i = 0; i < _enemiesInSwarm.Count; i++) {
			_enemiesInSwarm [i].Target = target;
		}	
	}

}
