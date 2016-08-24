using UnityEngine;
using System.Collections;

public class AirShooterEnemy : Enemy
{

	[SerializeField] private ObjectPoolerScript _projectilePooler;
	[SerializeField] private GameObject spawnPos;

	private float _nextFireTime;
	public float _fireRate = .25f;

	// Use this for initialization
	void Start ()
	{
		_target = TargetManager.Instance.getTarget ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		move ();
		attack ();
	}

	public override void move ()
	{

		if (_target != null) {
			_rb.velocity = transform.forward * _moveSpeed;
			var targetRotation = Quaternion.LookRotation (_target.transform.position - transform.position);
			_rb.MoveRotation (Quaternion.RotateTowards (transform.rotation, targetRotation, _rotateSpeed));

		} else {
			tryGetTarget ();
		}
	}

	public override void attack ()
	{


		if (Time.time > _nextFireTime) {
			_nextFireTime = Time.time + _fireRate;

			GameObject bullet = _projectilePooler.GetPooledObject ();
			bullet.transform.position = transform.position;
			Vector3 dir = _target.transform.position - spawnPos.transform.position;
			bullet.SetActive (true);
			bullet.GetComponent<Rigidbody> ().AddForce (dir * 1, ForceMode.Impulse); 
		}

	}
}
