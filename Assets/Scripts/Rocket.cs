using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
	[HideInInspector] public GameObject target;
	[SerializeField] private Renderer _renderer;
	[SerializeField] private Collider _collider;
	[SerializeField] private GameObject _explosionParticlesPrefab;
	[SerializeField] private Rigidbody _rb;
	[SerializeField] private ParticleSystem _rocketTrailParticles;
	[SerializeField] private float _moveSpeed = 1000f;
	[SerializeField] private float _rotateSpeed = 1000f;

	private bool _isAlive = true;
	// Use this for initialization

	void OnEnable ()
	{
		
//		_rb.isKinematic = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		move ();
	}

	public void move ()
	{
		if (target != null) {
			if (_isAlive) {
				
				_rb.velocity = transform.forward * _moveSpeed * Time.deltaTime;
				var targetRotation = Quaternion.LookRotation (target.transform.position - transform.position);
				_rb.MoveRotation (Quaternion.RotateTowards (transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime));
			}

		}
	
	}
	//	void rotateTowardEnemy (float rotateTime)
	//	{
	//		Vector3 direction = _target.transform.position - transform.position;
	//		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
	//		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
	//		direction = q.eulerAngles;
	//		LeanTween.rotate (gameObject, direction, rotateTime);
	//	}
	//
	//	void accelerateTowardEnemy ()
	//	{
	//		Vector3 direction = _target.transform.position - transform.position;
	//		direction.Normalize ();
	//		_rb.AddForce (direction * 6, ForceMode.Impulse);
	//		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (_target.transform.position - transform.position), _rotationSpeed * Time.fixedDeltaTime);
	//	}

	void explode ()
	{
		GameObject exp = Instantiate (_explosionParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		DetachParticles();
		gameObject.SetActive(false);
//		StartCoroutine (delayedDeactivate ());
	}

//	IEnumerator delayedDeactivate ()
//	{
//		yield return new WaitForSeconds (_rocketTrailParticles.startLifetime);
//		gameObject.SetActive (false);
//	}
//
	void OnCollisionEnter (Collision coll)
	{
		print ("rocketHitting:   " + coll.gameObject.name);

		if (coll.gameObject.CompareTag ("Enemy")) {
			explode ();
			coll.gameObject.GetComponent<Enemy> ().takeDamage (UpgradesManager.Instance.rocketStrength);

		}
	}

	public void DetachParticles()
	{
		// This splits the particle off so it doesn't get deleted with the parent
		_rocketTrailParticles.transform.parent = null;

		// this stops the particle from creating more bits
//		ParticleSystemEmissionType emission = new ParticleSystemEmissionType();
//		emission.rate = 0;
//		_rocketTrailParticles.emission.rate = emission;

		// This finds the particleAnimator associated with the emitter and then
		// sets it to automatically delete itself when it runs out of particles
		_rocketTrailParticles.GetComponent<ParticleAnimator>().autodestruct = true;
	}
}
