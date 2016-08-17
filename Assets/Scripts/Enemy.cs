using UnityEngine;
using System.Collections;

public abstract class Enemy : Entity {

	protected GameObject _target;

	[SerializeField] protected AudioSource _audSource;
	[SerializeField] protected Collider _collider;
	[SerializeField] protected Rigidbody _rb;
	[SerializeField] protected GameObject _dooberSplashPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void move ();
	public abstract void attack ();
}
