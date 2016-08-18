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


	[SerializeField] protected Color _origColor;

	protected bool _targetSet = false;
	[SerializeField] protected float _moveSpeed = 8f;
	[SerializeField] protected float _rotateSpeed = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void move ();
	public abstract void attack ();
}
