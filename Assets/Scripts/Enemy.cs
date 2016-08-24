using UnityEngine;
using System.Collections;

public abstract class Enemy : Entity
{

	[SerializeField] protected AudioSource _audSource;
	[SerializeField] protected Collider _collider;
	[SerializeField] protected Rigidbody _rb;
	[SerializeField] protected Renderer _renderer;
	[SerializeField] protected LineRenderer _lineRenderer;

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
	[SerializeField] protected GameObject _teslaNodePrefab;

	[SerializeField] protected Color _origColor;
	[SerializeField] protected Color _teslaColor;
	protected bool _hittable = true;
	protected bool _targetSet = false;
	[SerializeField] protected float _moveSpeed = 8f;
	[SerializeField] protected float _rotateSpeed = 10f;
	[SerializeField] protected TeslaNode[] teslaNodes;

	// Use this for initialization
	public virtual void Start ()
	{
		teslaNodes = GetComponentsInChildren<TeslaNode> ();
		_moveSpeed = _moveSpeed * Time.deltaTime;
		_rotateSpeed = _rotateSpeed * Time.deltaTime;
		tryGetTarget ();
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
			StartCoroutine (showDamageColor ());
			showDooberSplash (damage);
			_health -= damage;
//			updateHealthBar ();
			if (_health <= 0) {
				StartCoroutine (delayedDieCoroutine (true));
			}
		}
	}

	public void doTesla (float damage, int teslaCount)
	{
		teslaCount++;
		_renderer.material.color = _teslaColor;
		Collider[] colliders = Physics.OverlapSphere (transform.position, 120);
//		_lineRenderer.SetPosition(0, transform.position);

		if (teslaCount < 3) {
			
			for (int i = 0; i < colliders.Length; i++) {
				teslaNodes [i].lineRenderer.SetPosition (0, transform.position);
				teslaNodes [i].lineRenderer.SetPosition (1, colliders [i].transform.position);
				colliders [i].GetComponent<Enemy> ().doTesla	(damage * .5f, teslaCount);
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

	protected void tryGetTarget ()
	{
		if (TargetManager.Instance.targetGOs.Count > 0) {
			_target = TargetManager.Instance.getTarget ();
		} else {
			die (false);
		}
	}

	protected void releaseReward ()
	{
		GameObject lootGO = Instantiate (_lootPrefab, transform.transform.position, Quaternion.identity) as GameObject;	
	}
}
