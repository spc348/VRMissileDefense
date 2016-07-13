using UnityEngine;
using System.Collections;

public class SwarmEnemy : Enemy
{
	

	public override void MoveTowardTarget ()
	{

		if (_target != null) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (_target.transform.position - transform.position), _rotationSpeed * Time.fixedDeltaTime);
			transform.position += transform.forward * _moveSpeed * Time.deltaTime;
			//Debug.DrawLine (transform.position, hit.point, Color.cyan);
		} 
	}

	public void setTarget (GameObject targetToSet)
	{
		_target = targetToSet;
	}
}
