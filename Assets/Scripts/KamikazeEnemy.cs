using UnityEngine;
using System.Collections;

public class KamikazeEnemy : Enemy {

	private bool _stunned;
	private float _stunnedCountdown = 5f;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (_targetSet) {
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
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (_target.transform.position - transform.position), _rotateSpeed * Time.fixedDeltaTime);
			transform.position += transform.forward * _moveSpeed * Time.deltaTime;
			//			Debug.DrawLine (transform.position, hit.point, Color.cyan);
		}
//		else {
//			print ("getting new target");
//			_target = enemyManager.targetManager.targetGOs [Random.Range (0, enemyManager.targetManager.targetGOs.Count)];
//
//		}
	}
		
	public override void attack() {

	}
}
