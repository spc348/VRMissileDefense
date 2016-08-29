using UnityEngine;
using System.Collections;

public class KamikazeEnemy : Enemy {

	private bool _stunned;
	private float _stunnedCountdown = 5f;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
			if (!_stunned) {
				move ();
			} else {
				_stunnedCountdown -= Time.deltaTime;
				if (_stunnedCountdown <= 0) {
					_stunnedCountdown = 5f;
					_stunned = false;
				}
			}

	}

	public void initialize ()
	{
		_stunned = false;
		_renderer.material.color = _origColor;
//		_healthSliderCanvasGroup.alpha = 1f;
		_renderer.enabled = true;
		_collider.enabled = true;
//		_faceSpriteRenderer.enabled = true;
		_health = 10;

//		updateHealthBar ();
		gameObject.SetActive (true);
		_rb.velocity = Vector3.zero;
	}

	public override void move() {
		if (_target != null) {
			_rb.velocity = transform.forward * _moveSpeed;
			var targetRotation = Quaternion.LookRotation(_target.transform.position - transform.position);
			_rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed));

		}
		else {
			tryGetTarget();
		}
	}
		
	public override void attack() {

	}

	protected void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.CompareTag ("Target")) {
			GameObject deathExplosion = Instantiate(_deathExplosionPrefab, coll.transform.position, Quaternion.identity) as GameObject;
			gameObject.SetActive(false);
			coll.gameObject.GetComponent<Tower> ().takeDamage (1);
		}

	}
}
