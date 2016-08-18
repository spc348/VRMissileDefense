using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{

	[SerializeField] private ObjectPoolerScript _kamikazeEnemyPooler;
	// Use this for initialization
	void Start ()
	{
		GameObject player = GameObject.Find ("Player");
		transform.LookAt (2 * transform.position - player.transform.position);
		open ();
	}


	public void open ()
	{
		StartCoroutine (openCoroutine ());
	}

	IEnumerator openCoroutine() {
		LeanTween.scale (gameObject, Vector3.one * 20f, 5f).setEase (LeanTweenType.easeInOutExpo);
		yield return new WaitForSeconds(5f);
		StartCoroutine (spawnKamikazeEnemiesCoroutine (10));

	}

//	public void spawnKamikazeEnemies (int numEnemies)
//	{
//
//		StartCoroutine (spawnKamikazeEnemiesCoroutine (numEnemies));
//	}

	IEnumerator spawnKamikazeEnemiesCoroutine (int numEnemies)
	{
		for (int i = 0; i < numEnemies; i++) {
			GameObject kamikazeEnemy = _kamikazeEnemyPooler.GetPooledObject ();
			kamikazeEnemy.transform.position = transform.position;
			yield return new WaitForSeconds (.1f);
		}
	}
}
