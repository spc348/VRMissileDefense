using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
	[SerializeField] private EnemyManager _enemyManager;
	
	[SerializeField] private AudioSource audSource;
	[SerializeField] private AudioClip deathBoomClip;
	[SerializeField] private Collider _collider;

	public GameObject Target {
		get { return _target; }
		set {
			_target = value;
			_targetSet = true;
		}
	}

	protected GameObject _target;
	[SerializeField] private ParticleSystem _deathExplosionParticles;
	[SerializeField] private Renderer _renderer;

	[SerializeField] private Slider _healthSlider;

	public bool stunned = false;
	private bool _targetSet = false;
	private Color _origColor;
	[SerializeField] private float _rotationSpeed = 10f;
	[SerializeField] private float _moveSpeed = 4f;
	private float _stunnedCountdown = 5f;
	private int health = 10;

	// Use this for initialization
	void Start ()
	{
		_origColor = _renderer.material.color;
		_deathExplosionParticles.startColor = _origColor;
		updateHealthBar ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (_targetSet) {
			if (!stunned) {
				MoveTowardTarget ();
			} else {
				_stunnedCountdown -= Time.deltaTime;
				if (_stunnedCountdown <= 0) {
					_stunnedCountdown = 5f;
					stunned = false;
				}
			}
		}


	}

	public void delayedDie (bool killedByPlayer)
	{
		StartCoroutine (delayedDieCoroutine (killedByPlayer));
	}

	IEnumerator delayedDieCoroutine (bool killedByPlayer)
	{
		yield return new WaitForSeconds (.5f);
		die (killedByPlayer);
	}

	void die (bool killedByPlayer)
	{
		_renderer.enabled = false;
		_collider.enabled = false;

		Instantiate (_deathExplosionParticles, transform.position, Quaternion.identity);

		if (killedByPlayer) {
			releaseReward ();
		}

		gameObject.SetActive (false);
		//		audSource.PlayOneShot (deathBoomClip);
		//		Destroy (gameObject, deathBoomClip.length);
	}

	public void MoveTowardTarget ()
	{
		if (_target != null) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (_target.transform.position - transform.position), _rotationSpeed * Time.fixedDeltaTime);
			RaycastHit hit;
			float offsetAlign = 0f;
			if (Physics.Raycast (transform.position, Vector3.down, out hit)) {
				//			transform.position = new Vector3(transform.position.x, transform.position.y + hit.point.y + offsetAlign, transform.position.z);
			}
			transform.position += transform.forward * _moveSpeed * Time.deltaTime;
//			Debug.DrawLine (transform.position, hit.point, Color.cyan);
		}
	}


	public void updateHealthBar ()
	{
		_healthSlider.value = health;
	}

	public void takeDamage (int damage)
	{
		StartCoroutine (showDamageColor ());
		stunned = true;
		health -= damage;
		updateHealthBar ();
		if (health <= 0) {
			StartCoroutine(delayedDieCoroutine (true));
		}
	}

	IEnumerator showDamageColor ()
	{
		_renderer.material.color = Color.red;
		yield return new WaitForSeconds (.3f);
		_renderer.material.color = _origColor;
	}

	void releaseReward() {
		print ("releasing reward");	
	}

	void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.CompareTag ("Target")) {
			coll.gameObject.GetComponent<Tower>().takeDamage (1);

			die (false);
		}
		
	}


}
