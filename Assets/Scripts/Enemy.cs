using UnityEngine;
using System.Collections;

public abstract class Enemy : Entity {

	[SerializeField] protected AudioSource _audSource;
	[SerializeField] protected Collider _collider;
	[SerializeField] protected Rigidbody _rb;
	[SerializeField] protected Renderer _renderer;

	public GameObject Target {
		get { return _target; }
		set {
			_target = value;
			_targetSet = true;
		}
	}

	protected GameObject _target;
	[SerializeField] protected GameObject _dooberSplashPrefab;
	[SerializeField] protected GameObject _deathExplosionPrefab;
	[SerializeField] protected GameObject _lootPrefab;

	[SerializeField] protected Color _origColor;

	protected bool _hittable = true;
	protected bool _targetSet = false;
	[SerializeField] protected float _moveSpeed = 8f;
	[SerializeField] protected float _rotateSpeed = 10f;

	// Use this for initialization
	public virtual void Start () {
		Target = TargetManager.Instance.getTarget();
	}

	public abstract void move ();
	public abstract void attack ();

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
			_health -= damage;
			print (gameObject.name + " health: " + _health);
//			updateHealthBar ();
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
//		_faceSpriteRenderer.enabled = false;
//		_healthSliderCanvasGroup.alpha = 0;

		GameObject explosion = Instantiate (_deathExplosionPrefab, transform.position, Quaternion.identity) as GameObject;
		explosion.name = "Explosion" + gameObject.name;
		if (killedByPlayer) {
			float r = Random.value;
			releaseReward ();
			if (r <= .25f) {
			}
		}
		yield return null;
//		audSource.PlayOneShot (deathBoomClip);
//		yield return new WaitForSeconds (deathBoomClip.length);
		gameObject.SetActive (false);
//		enemyManager.enemies.Remove (this);
//		GameEventManager.TriggerEvent ("CheckEnemyList");
	}

	protected void releaseReward ()
	{
		GameObject lootGO = Instantiate (_lootPrefab, transform.transform.position, Quaternion.identity) as GameObject;	
	}
}
