using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	[SerializeField] private GameObject _bulletExplosion;

	// Use this for initialization
	void Start () {
	
	}
	


	protected void OnCollisionEnter (Collision coll)
	{
		print ("bullet hit: " + coll.gameObject);
		GameObject bulletExplosion = Instantiate(_bulletExplosion, coll.transform.position, Quaternion.identity) as GameObject;
		gameObject.SetActive(false);
		if (coll.gameObject.CompareTag ("Target")) {
			coll.gameObject.GetComponent<Tower> ().takeDamage (1);
		}

	}
}
