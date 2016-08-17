using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmEnemy : OldEnemy
{
	public List<SwarmEnemy> enemiesInSwarm;

	public void Awake ()
	{
		_rb = GetComponent<Rigidbody> ();
	}

	public override void Update ()
	{
		base.Update ();
		separate (enemiesInSwarm);
	}

	public void separate (List<SwarmEnemy> others)
	{
		float desiredSeparation = _collider.bounds.size.x;
		Vector3 sum = new Vector3 ();
		int count = 0;

		foreach (SwarmEnemy other in others) {

			if (other == null) {
				print ("my other is null: " + gameObject.name);
			}
			if (other.gameObject.activeInHierarchy) {
				float distance = Vector3.Distance (transform.position, other.transform.position);
				if ((distance > 0) && (distance < desiredSeparation)) {
					Vector3 difference = other.transform.position - transform.position;
					difference.Normalize ();
					difference = difference / distance;
					sum += difference;
					count++;
				} 
			}

			if (count > 0) {
				sum = sum / count;
				sum.Normalize ();

				sum = sum * _moveSpeed;
				Vector3 steer = _rb.velocity - sum;
				if (_rb.velocity.magnitude < 2f) {
					_rb.AddForce (steer);
				} 
			}
		}
	}

	public override void MoveTowardTarget ()
	{

		if (_target != null) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (_target.transform.position - transform.position), _rotationSpeed * Time.fixedDeltaTime);
			_rb.AddRelativeForce (0, 0, 2f);
			//			Vector3 dir = 
//			transform.position += transform.forward * _moveSpeed * Time.deltaTime;
			//Debug.DrawLine (transform.position, hit.point, Color.cyan);
		} 
	}

	public void setTarget (GameObject targetToSet)
	{
		_target = targetToSet;
	}
}
