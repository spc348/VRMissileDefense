using UnityEngine;
using System.Collections;

public class ShooterEnemy : Enemy
{
	[SerializeField] private ObjectPoolerScript _objectPoolerScript;
	[SerializeField] private GameObject _projectileSpawnPos;
	private float _shootTimer;


	// Use this for initialization
	void Start ()
	{
		_shootTimer = Random.Range (1f, 2f);
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		_shootTimer -= Time.deltaTime;
		if (_shootTimer <= 0) {
			Shoot ();
			_shootTimer = Random.Range (1f, 5f);
		}

	}

	void Shoot ()
	{
		GameObject projectileGO = _objectPoolerScript.GetPooledObject ();
		EnemyProjectile projectile = projectileGO.GetComponent<EnemyProjectile> ();

		Vector3 shootDir = (_target.transform.position - _projectileSpawnPos.transform.position).normalized;
		projectileGO.SetActive(true);
		projectile.rb.AddForce (shootDir * 5f, ForceMode.Impulse);
	}
}
