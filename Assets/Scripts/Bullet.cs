using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

	[SerializeField] private GameObject _bulletExplosion;
	[SerializeField] Rigidbody _rb;
	[SerializeField] Renderer _renderer;
	[SerializeField] Light _light;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (dieAfterTime ());
	}

	void OnEnable ()
	{
		_renderer.enabled = true;
		_light.enabled = true;
	}

	protected void OnCollisionEnter (Collision coll)
	{
		StartCoroutine (dieCoroutine (coll));
//		GameObject bulletExplosion = Instantiate (_bulletExplosion, coll.transform.position, Quaternion.identity) as GameObject;
//		gameObject.SetActive (false);
		if (coll.gameObject.CompareTag ("Target")) {
			coll.gameObject.GetComponent<Target> ().takeDamage (1);
		}

	}

	IEnumerator dieCoroutine(Collision coll) {
		_rb.velocity = Vector3.zero;
		_light.enabled = false;
		GameObject bulletExplosion = Instantiate (_bulletExplosion, coll.transform.position, Quaternion.identity) as GameObject;

		float trailLiveTime = .21f;
		yield return new WaitForSeconds (trailLiveTime);
		gameObject.SetActive (false);
	}

	IEnumerator dieAfterTime ()
	{
		if (gameObject.activeInHierarchy) {
			yield return new WaitForSeconds (10f);
			_rb.velocity = Vector3.zero;
			_light.enabled = false;
			GameObject bulletExplosion = Instantiate (_bulletExplosion, transform.position, Quaternion.identity) as GameObject;

			float trailLiveTime = .21f;
			yield return new WaitForSeconds (trailLiveTime);
			gameObject.SetActive (false);
		}
	}
}
