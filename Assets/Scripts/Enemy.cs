﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : Entity
{

	[SerializeField] protected AudioSource _audSource;
	[SerializeField] protected Collider _collider;
	[SerializeField] protected Rigidbody _rb;
	[SerializeField] protected Renderer _renderer;
	[SerializeField] protected Light _light;
	public GameObject target {
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
//	private bool _teslaCountIncremented = false;
	private bool _gotTeslaColliders = false;
	[SerializeField] protected float _moveSpeed = 8f;
	[SerializeField] protected float _rotateSpeed = 10f;
	[SerializeField] protected TeslaNode[] teslaNodes;
	private Collider[] _teslaColliders;
	private List<Enemy> _enemiesInTeslaRange = new List<Enemy> ();

	public delegate void TakeDamageEvent (float damage);

	public static event TakeDamageEvent OnTakeDamage;


	public virtual void OnEnable ()
	{
		Target.OnGameOver += gameOverDie; 
		WeaponsManager.OnCancelTesla += cancelTesla;
		initialize ();
	}

	public virtual void OnDisable ()
	{
		Target.OnGameOver -= gameOverDie;
		WeaponsManager.OnCancelTesla -= cancelTesla;
		_renderer.material.color = new Color (_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 0f);
	}

	// Use this for initialization
	public virtual void Start ()
	{
		_renderer.material.color = new Color (_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 0f);
		teslaNodes = GetComponentsInChildren<TeslaNode> ();
		tryGetTarget ();
	}

	public abstract void move ();

	public abstract void attack ();

	public virtual void initialize ()
	{
		_hittable = true;
		_renderer.material.color = _origColor;
		_renderer.enabled = true;
		_collider.enabled = true;
		_health = _origMaxHealth * EnemyManager.Instance.healthMultiplier;
		gameObject.SetActive (true);
		_rb.velocity = Vector3.zero;
	}

	public void fadeIn ()
	{
		LeanTween.alpha (gameObject, 1, 5f);
	}

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

			float realDamage = getRealDamage (damage); 
			showDooberSplash (realDamage);
			_health -= realDamage;

			OnTakeDamage (realDamage);

			if (_health <= 0) {
				StartCoroutine (delayedDieCoroutine (true));
			}
		}
	}

	float getRealDamage (float damage)
	{
		//apply damage
		
		float rDamage = 0;
		if (_health - damage > 0) {
			rDamage = damage;
		} else {
			rDamage = _health;
		}
		return rDamage;
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
//		_teslaCountIncremented = false;
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

	public void gameOverDie () {
		StartCoroutine (dieCoroutine(false));
	}

	public void delayedDie (bool killedByPlayer)
	{
		StartCoroutine (delayedDieCoroutine (killedByPlayer));
	}

	protected IEnumerator delayedDieCoroutine (bool killedByPlayer)
	{
		_hittable = false;
		yield return new WaitForSeconds (.5f);
		StartCoroutine (dieCoroutine (killedByPlayer));
	}

	protected IEnumerator dieCoroutine (bool killedByPlayer)
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

		if (!killedByPlayer) {
			//remove remaing health from bar
			OnTakeDamage (_health);
		}

//		yield return null;

		gameObject.SetActive (false);	
		yield return null;
	}

	protected void tryGetTarget ()
	{

			_target = TargetManager.Instance.getTarget ();
//		} else {
//			die (false);
//		}
	}

	protected void releaseReward ()
	{
		UpgradesManager.Instance.dropPowerUp (transform.position);
	}
}
