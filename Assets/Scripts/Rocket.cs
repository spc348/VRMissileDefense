using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
	[SerializeField] private AudioSource _audSource;
	[SerializeField] private AudioClip _launchSFX;
	[SerializeField] private AudioClip _explodeSFX;
	[HideInInspector] public GameObject target;
	[SerializeField] private Renderer _renderer;
	[SerializeField] private Collider _collider;
	[SerializeField] private GameObject _explosionParticlesPrefab;
	[SerializeField] private Rigidbody _rb;
	[SerializeField] private ParticleSystem _rocketTrailParticles;
	[SerializeField] private float _moveSpeed = 1000f;
	[SerializeField] private float _rotateSpeed = 1000f;
	private float _origParticleEmissionRate;
	private bool _isAlive = true;
	// Use this for initialization

	void Start ()
	{
		_origParticleEmissionRate = _rocketTrailParticles.emission.rate.constantMax;
	}

	void OnEnable ()
	{
		_renderer.enabled = true;
		_collider.enabled = true;
		_rb.isKinematic = false;
		_rocketTrailParticles.EnableEmission(true);
		_isAlive = true;
		_audSource.PlayOneShot (_launchSFX);
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

	void explode ()
	{
		_audSource.PlayOneShot (_explodeSFX);
		_rocketTrailParticles.EnableEmission (false);
		GameObject exp = Instantiate (_explosionParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		_isAlive = false;
		_renderer.enabled = false;
		_collider.enabled = false;
		_rb.isKinematic = true;

		//		gameObject.SetActive(false);
		StartCoroutine (delayedDeactivate ());
	}

	IEnumerator delayedDeactivate ()
	{
		yield return new WaitForSeconds (_rocketTrailParticles.startLifetime);
		gameObject.SetActive (false);
	}

	void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.CompareTag ("Enemy")) {
			explode ();
			coll.gameObject.GetComponent<Enemy> ().takeDamage (UpgradesManager.Instance.rocketStrength);

		}
	}

	//	public void DetachParticles()
	//	{
	//		_rocketTrailParticles.transform.parent = null;
	//		Destroy (_rocketTrailParticles,
	//
	//	}
}
