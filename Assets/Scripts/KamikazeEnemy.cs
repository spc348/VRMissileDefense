using UnityEngine;
using System.Collections;

public class KamikazeEnemy : Enemy {
	
	// Update is called once per frame
	void Update () {
		move ();
	}

	public override void move() {
		if (_target != null) {
			_rb.velocity = transform.forward * _moveSpeed * Time.deltaTime;
			var targetRotation = Quaternion.LookRotation(_target.transform.position - transform.position);
			_rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime));

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
			StartCoroutine (dieCoroutine (false));
			coll.gameObject.GetComponent<Target> ().takeDamage (1);
		}

	}
}
