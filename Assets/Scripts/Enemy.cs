using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	[SerializeField] protected Color _origColor;
	[SerializeField] protected Color _teslaColor;
	protected bool _hittable = true;
	protected bool _targetSet = false;
	public bool isTeslaing = false;
	private bool _teslaCountIncremented = false;
	private bool _gotTeslaColliders = false;
	[SerializeField] protected float _moveSpeed = 8f;
	[SerializeField] protected float _rotateSpeed = 10f;
	public float power;
	[SerializeField] protected TeslaNode[] teslaNodes;
	private Collider[] _teslaColliders;
	private List<Enemy> _enemiesInTeslaRange = new List<Enemy> ();

	void OnEnable ()
	{
		WeaponsManager.OnCancelTesla += cancelTesla;
	}

	void OnDisable ()
	{
		WeaponsManager.OnCancelTesla -= cancelTesla;
	}

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



	public void showDooberSplash (float amount)
	{
		GameObject dooberSplash = Instantiate (_dooberSplashPrefab, transform.position, Quaternion.identity) as GameObject;
		dooberSplash.GetComponent<DooberSplash> ().setText (amount);
		LeanTween.moveY (dooberSplash, transform.position.y + 10f, 1.5f).setEase (LeanTweenType.easeOutExpo);
	}

	public void takeDamage (float damage)
	{
		damage = Mathf.Round (damage);
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
		isTeslaing = true;
		_renderer.material.color = _teslaColor;

		takeDamage (damage);

		if (!_gotTeslaColliders) {
			_teslaColliders = Physics.OverlapSphere (transform.position, 120);
		
			_enemiesInTeslaRange.Clear ();
			for (int i = 0; i < _teslaColliders.Length; i++) {
				Collider tColl = _teslaColliders [i];
				if (tColl.CompareTag ("Enemy")) {	
					if (!tColl.GetComponent<Enemy> ().isTeslaing) {

						_enemiesInTeslaRange.Add (tColl.GetComponent<Enemy> ());	
					}
				}
			}
			_gotTeslaColliders = true;
		}

 
		if (teslaCount < 2) {
			if (_enemiesInTeslaRange.Count > 0) {
				for (int i = 0; i < _enemiesInTeslaRange.Count; i++) {
					if (i < UpgradesManager.Instance.numTeslaBranches) {
						Enemy enemy = _enemiesInTeslaRange [i];
						TeslaNode tnode = teslaNodes [i];
						tnode.lineRenderer.enabled = true;
						tnode.lineRenderer.SetPosition (0, transform.position);
						tnode.lineRenderer.SetPosition (1, enemy.transform.position);
						enemy.doTesla (damage * .5f, ++teslaCount);
					}
				}	
			}
		}
	}

	public void cancelTesla ()
	{
		
		_gotTeslaColliders = false;
		_teslaCountIncremented = false;
		isTeslaing = false;
		for (int i = 0; i < teslaNodes.Length; i++) {
			teslaNodes [i].lineRenderer.enabled = false;
			_renderer.material.color = _origColor;
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
