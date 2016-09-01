using UnityEngine;
using System.Collections;

public class AirShooterEnemy : Enemy
{

	[SerializeField] private ObjectPoolerScript _projectilePooler;
	[SerializeField] private GameObject bulletSpawnPos;

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
			float distanceFromTarget = Vector3.Distance (_target.transform.position, transform.position);
			if (distanceFromTarget > 10) {
				_rb.velocity = transform.forward * _moveSpeed;
			} else {
				_rb.velocity = Vector3.zero;
			}
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
			Vector3 dir = _target.transform.position - bulletSpawnPos.transform.position;
			bullet.SetActive (true);
			bullet.GetComponent<Rigidbody> ().AddForce (dir * 1, ForceMode.Impulse); 
		}

	}
}
