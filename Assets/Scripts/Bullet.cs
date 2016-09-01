using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	[SerializeField] private GameObject _bulletExplosion;

	// Use this for initialization
	void Start () {
		StartCoroutine (dieAfterTime ());
	}
	


	protected void OnCollisionEnter (Collision coll)
	{
		GameObject bulletExplosion = Instantiate(_bulletExplosion, coll.transform.position, Quaternion.identity) as GameObject;
		gameObject.SetActive(false);
		if (coll.gameObject.CompareTag ("Target")) {
			coll.gameObject.GetComponent<Tower> ().takeDamage (1);
		}

	}

	IEnumerator dieAfterTime() 
	{
		if (gameObject.activeInHierarchy) {
			yield return new WaitForSeconds (10f);
			GameObject bulletExplosion = Instantiate(_bulletExplosion, transform.position, Quaternion.identity) as GameObject;
			gameObject.SetActive(false);
		}
	}
}
