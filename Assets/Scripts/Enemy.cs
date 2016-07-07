using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
	[SerializeField] public EnemyManager enemyManager;
	
	[SerializeField] private AudioSource audSource;
	[SerializeField] private AudioClip deathBoomClip;
	[SerializeField] private Collider _collider;
	[SerializeField] private GameObject _dooberSplashPrefab;

	public GameObject Target {
		get { return _target; }
		set {
			_target = value;
			_targetSet = true;
		}
	}

	protected GameObject _target;
	[SerializeField] GameObject lootPrefab;
	[SerializeField] private GameObject _deathExplosionPrefab;
	[SerializeField] private Renderer _renderer;

	[SerializeField] private Slider _healthSlider;

	public bool stunned = false;
	private bool _targetSet = false;
	[SerializeField] private Color _origColor;
	[SerializeField] private float _rotationSpeed = 10f;
	[SerializeField] private float _moveSpeed = 4f;
	private float _stunnedCountdown = 5f;
	private int _health = 100;

	void OnEnable ()
	{
		
	}

	void OnDisable ()
	{

	}
		
	// Use this for initialization
	void Start ()
	{
//		_origColor = _renderer.material.color;
		_deathExplosionPrefab.GetComponent<ParticleSystem>().startColor = _origColor;
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

//		if (Input.GetKeyDown(KeyCode.Space)) {
//			showDooberSplash();
//		}
	}

	public void initialize ()
	{
		stunned = false;
		_renderer.material.color = _origColor;
		_renderer.enabled = true;
		_collider.enabled = true;
		_health = 10;
		updateHealthBar ();
		gameObject.SetActive (true);
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
	
		GameObject explosion = Instantiate (_deathExplosionPrefab, transform.position, Quaternion.identity) as GameObject;
		explosion.name = "Explosion" + gameObject.name;
		if (killedByPlayer) {
			float r = Random.value;
			if (r <= .25f) {
				releaseReward ();
			}
		}

		gameObject.SetActive (false);
		enemyManager.enemies.Remove(this);
		GameEventManager.TriggerEvent("CheckEnemyList");
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
		_healthSlider.value = _health;
	}

	public void showDooberSplash(int amount) {
		GameObject dooberSplash = Instantiate (_dooberSplashPrefab, transform.position, Quaternion.identity) as GameObject;
		dooberSplash.GetComponent<DooberSplash>().setText(amount);
		LeanTween.moveY(dooberSplash, transform.position.y + 5f, 1f).setEase(LeanTweenType.easeOutExpo);
	}

	public void takeDamage (int damage)
	{
		StartCoroutine (showDamageColor ());
		showDooberSplash(damage);
		stunned = true;
		_health -= damage;
		updateHealthBar ();
		if (_health <= 0) {
			StartCoroutine (delayedDieCoroutine (true));
		}
	}

	IEnumerator showDamageColor ()
	{
		_renderer.material.color = Color.red;
		yield return new WaitForSeconds (.3f);
		_renderer.material.color = _origColor;
	}

	void releaseReward ()
	{
		GameObject lootGO = Instantiate (lootPrefab, transform.transform.position, Quaternion.identity) as GameObject;	
	}

	void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.CompareTag ("Target")) {
			coll.gameObject.GetComponent<Tower> ().takeDamage (1);

			die (false);
		}
		
	}


}
