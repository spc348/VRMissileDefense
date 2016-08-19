using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour
{

	private GameObject _player;
	private bool _movingToPlayer = false;
	// Use this for initialization
	void Start ()
	{
		_player = GameObject.Find ("Player");
	}

	void OnCollisionEnter (Collision coll)
	{
//		print ("loot hit: " + coll.gameObject.name);
		if (!_movingToPlayer) {
			if (coll.gameObject.CompareTag ("Ground") || coll.gameObject.CompareTag ("Target")) {
				LeanTween.move (gameObject, _player.transform.position, 1f).setEase (LeanTweenType.easeInExpo);
				_movingToPlayer = true;
			}
		}

		if (coll.gameObject.CompareTag ("Player")) {
			coll.gameObject.GetComponent<WeaponsManager>().increaseLootCount(1);

			Destroy (gameObject);
		}
	}


}
