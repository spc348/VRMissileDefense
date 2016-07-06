using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
	public GameObject _target;
	[SerializeField] private GameObject _explosionParticlesPrefab;
	[SerializeField] private Rigidbody _rb;
	private float _rotationSpeed = 10f;

	// Use this for initialization

	void Start ()
	{
		_target = GameObject.Find ("EnemyShip");
	}
	
	// Update is called once per frame
	void Update ()
	{
		accelerateTowardEnemy ();
	}

	void rotateTowardEnemy (float rotateTime)
	{
		Vector3 direction = _target.transform.position - transform.position;
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		direction = q.eulerAngles;
		LeanTween.rotate (gameObject, direction, rotateTime); 
	}

	void accelerateTowardEnemy ()
	{
		Vector3 direction = _target.transform.position - transform.position;
		direction.Normalize ();
		_rb.AddForce (direction * 6, ForceMode.Impulse);
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (_target.transform.position - transform.position), _rotationSpeed * Time.fixedDeltaTime);
	}

	void explode ()
	{
		GameObject exp = Instantiate (_explosionParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		Destroy (gameObject);
	}

	void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.CompareTag ("Enemy")) {
			explode ();
		}
	}
}
