using UnityEngine;
using System.Collections;

public class AirShooterEnemy : Enemy
{

	[SerializeField] private ObjectPoolerScript _projectilePooler;
	[SerializeField] private GameObject bulletSpawnPos;
	[SerializeField] private GameObject _eyeLeft;
	[SerializeField] private GameObject _eyeRight;
	[SerializeField] private GameObject _mouthTop;
	[SerializeField] private GameObject _mouthBottom;
	public float _fireRate = .25f;
	[SerializeField] private float _stopDistanceFromTarget = 200f;
	private float _nextFireTime;
	[SerializeField] private GameObject _mouthTopOrigPos;
	[SerializeField] private GameObject _mouthBottomOrigPos;
	 
	[SerializeField] private GameObject[] _tenactles;

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
		eyesLookAtTarget ();
	}

	public override void move ()
	{
		if (_target != null) {
			float distanceFromTarget = Vector3.Distance (_target.transform.position, transform.position);
			if (distanceFromTarget > _stopDistanceFromTarget) {
				_rb.velocity = transform.forward * _moveSpeed * Time.deltaTime;
			} else {
				_rb.velocity = Vector3.zero;
			}
			var targetRotation = Quaternion.LookRotation (_target.transform.position - transform.position);
			_rb.MoveRotation (Quaternion.RotateTowards (transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime));

		} else {
			tryGetTarget ();
		}
	}

	public override void attack ()
	{
		if (_target != null) {
			if (Time.time > _nextFireTime) {
				_nextFireTime = Time.time + _fireRate;
//				StartCoroutine (openMouthCoroutine ());
				GameObject bullet = _projectilePooler.GetPooledObject ();
				bullet.transform.position = transform.position;
				Vector3 dir = _target.transform.position - bullet.transform.position;
				bullet.SetActive (true);
				bullet.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				bullet.GetComponent<Rigidbody> ().AddForce (dir * 1, ForceMode.Impulse); 
			}	
		}
	}

	void eyesLookAtTarget ()
	{
		if (_target != null) {
			_eyeLeft.transform.LookAt (_target.transform);
			_eyeRight.transform.LookAt (_target.transform);
//			float r = Random.Range (-10f, 10f);
//			_eyeRight.GetComponent<Rigidbody> ().AddTorque (Vector3.one * r); 
//				.LookAt (_target.transform);
		}
	}

	IEnumerator openMouthCoroutine ()
	{
		openMouth ();
		yield return new WaitForSeconds (1.1f);
		closeMouth ();
	}

	void openMouth ()
	{
		float amount = .25f;
		LeanTween.moveLocalY (_mouthTop.gameObject, amount, 1f).setEase (LeanTweenType.easeOutExpo);
		LeanTween.moveLocalY (_mouthBottom.gameObject, -amount, 1f).setEase (LeanTweenType.easeOutExpo);
	}

	void closeMouth ()
	{
		float amount = 1f;
		LeanTween.move (_mouthTop.gameObject, _mouthTopOrigPos.transform.position, .25f).setEase (LeanTweenType.easeOutExpo);
		LeanTween.move (_mouthBottom.gameObject, _mouthBottomOrigPos.transform.position, .25f).setEase (LeanTweenType.easeOutExpo);
	}


}
