using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using SonicBloom.Koreo;

public class Enemy : MonoBehaviour
{
	[SerializeField] public EnemyManager enemyManager;
	
	[SerializeField] protected AudioSource audSource;
	[SerializeField] protected AudioClip deathBoomClip;
	[SerializeField] protected Collider _collider;
	[SerializeField] protected Rigidbody _rb;
	[SerializeField] protected GameObject _dooberSplashPrefab;

	public GameObject Target {
		get { return _target; }
		set {
			_target = value;
			_targetSet = true;
		}
	}

	protected GameObject _target;
	[SerializeField] GameObject lootPrefab;
	[SerializeField] protected GameObject _deathExplosionPrefab;
	[SerializeField] protected Renderer _renderer;
	[SerializeField] protected SpriteRenderer _faceSpriteRenderer;
	[SerializeField] protected Slider _healthSlider;
	[SerializeField] protected CanvasGroup _healthSliderCanvasGroup;

	[SerializeField] protected bool _hittable = true;
	public bool stunned = false;
	protected bool _targetSet = false;
	[SerializeField] protected Color _origColor;
	[SerializeField] protected Color _pulseColor;
	[SerializeField] protected float _rotationSpeed = 10f;
	[SerializeField] protected float _moveSpeed = 8f;
	protected float _stunnedCountdown = 5f;
	protected int _health = 10;


		
	// Use this for initialization
	public virtual void Start ()
	{
		_deathExplosionPrefab.GetComponent<ParticleSystem> ().startColor = _origColor;
		updateHealthBar ();

//		Koreographer.Instance.RegisterForEvents ("blink", OnMusicalBlink);

	
	}

	// Update is called once per frame
	public virtual void Update ()
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

	void OnMusicalBlink (KoreographyEvent evt)
	{
//		StartCoroutine (pulse ());
	}

//	IEnumerator pulse ()
//	{
////		_hittable = true;
////		_renderer.material.color = _pulseColor;
////		yield return new WaitForSeconds (.1f);
////		LeanTween.color (gameObject, _origColor, .25f);
////		yield return new WaitForSeconds (.25f);
////		_hittable = false;
//	
//	}

	public void initialize ()
	{
		stunned = false;
		_renderer.material.color = _origColor;
		_healthSliderCanvasGroup.alpha = 1f;
		_renderer.enabled = true;
		_collider.enabled = true;
		_faceSpriteRenderer.enabled = true;
		_health = 10;

		updateHealthBar ();
		gameObject.SetActive (true);
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	public void delayedDie (bool killedByPlayer)
	{
		StartCoroutine (delayedDieCoroutine (killedByPlayer));
	}

	protected IEnumerator delayedDieCoroutine (bool killedByPlayer)
	{
		yield return new WaitForSeconds (.5f);
		StartCoroutine (die (killedByPlayer));
	}

	protected IEnumerator die (bool killedByPlayer)
	{
		_renderer.enabled = false;
		_collider.enabled = false;
		_faceSpriteRenderer.enabled = false;
		_healthSliderCanvasGroup.alpha = 0;

		GameObject explosion = Instantiate (_deathExplosionPrefab, transform.position, Quaternion.identity) as GameObject;
		explosion.name = "Explosion" + gameObject.name;
		if (killedByPlayer) {
			float r = Random.value;
			if (r <= .25f) {
				releaseReward ();
			}
		}
		yield return null;
		audSource.PlayOneShot (deathBoomClip);
		yield return new WaitForSeconds (deathBoomClip.length);
		gameObject.SetActive (false);
		enemyManager.enemies.Remove (this);
		GameEventManager.TriggerEvent ("CheckEnemyList");
	}

	public virtual void MoveTowardTarget ()
	{
		if (_target != null) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (_target.transform.position - transform.position), _rotationSpeed * Time.fixedDeltaTime);
			transform.position += transform.forward * _moveSpeed * Time.deltaTime;
//			Debug.DrawLine (transform.position, hit.point, Color.cyan);
		} else {
			print ("getting new target");
			_target = enemyManager.targetManager.targetGOs [Random.Range (0, enemyManager.targetManager.targetGOs.Count)];

		}
	}



	public void updateHealthBar ()
	{
		_healthSlider.value = _health;
	}

	public void showDooberSplash (int amount)
	{
		GameObject dooberSplash = Instantiate (_dooberSplashPrefab, transform.position, Quaternion.identity) as GameObject;
		dooberSplash.GetComponent<DooberSplash> ().setText (amount);
		LeanTween.moveY (dooberSplash, transform.position.y + 5f, 1f).setEase (LeanTweenType.easeOutExpo);
	}

	public void takeDamage (int damage)
	{
		if (_hittable) {
			print (gameObject.name + " damage: " + damage);
			StartCoroutine (showDamageColor ());
			showDooberSplash (damage);
//		stunned = true;
			_health -= damage;
			print (gameObject.name + " health: " + _health);
			updateHealthBar ();
			if (_health <= 0) {
				StartCoroutine (delayedDieCoroutine (true));
			}
		}
	}

	protected IEnumerator showDamageColor ()
	{
		_renderer.material.color = Color.red;
		yield return new WaitForSeconds (.3f);
		_renderer.material.color = _origColor;
	}

	protected void releaseReward ()
	{
		GameObject lootGO = Instantiate (lootPrefab, transform.transform.position, Quaternion.identity) as GameObject;	
	}

	protected void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.CompareTag ("Target")) {
			coll.gameObject.GetComponent<Tower> ().takeDamage (1);

//			StartCoroutine (die (false));
		}
		
	}


}
