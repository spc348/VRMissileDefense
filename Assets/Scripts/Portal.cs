using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal : MonoBehaviour
{
	[SerializeField] private Dictionary <string, ObjectPoolerScript> enemyPoolDictionary = new Dictionary<string, ObjectPoolerScript> ();
	[SerializeField] private ObjectPoolerScript _kamikazeEnemyPooler;
	[SerializeField] private ObjectPoolerScript _airShooterEnemyPooler;

	// Use this for initialization
	void Start ()
	{
		initEnemyPoolDict ();
		GameObject player = GameObject.Find ("Player");
		transform.LookAt (2 * transform.position - player.transform.position);
//		open ();
	}


	public void open (string enemyType)
	{
		StartCoroutine (openCoroutine (enemyType));
	}

	IEnumerator openCoroutine (string enemyType)
	{
		yield return new WaitForSeconds (2f);
		LeanTween.scale (gameObject, Vector3.one * 10f, 2f).setEase (LeanTweenType.easeInOutExpo);
		yield return new WaitForSeconds (1f);
		close ();
	}

	void close ()
	{
		LeanTween.scale (gameObject, Vector3.zero * 10f, 5f).setEase (LeanTweenType.easeInOutExpo);
	}


	void initEnemyPoolDict ()
	{
		enemyPoolDictionary.Add ("kamikaze", _kamikazeEnemyPooler);
		enemyPoolDictionary.Add ("airShooter", _airShooterEnemyPooler);
	}




	//	public void open ()
	//	{
	//		StartCoroutine (openCoroutine ());
	//	}
	//
	//	IEnumerator openCoroutine ()
	//	{
	//		yield return new WaitForSeconds (2f);
	//		LeanTween.scale (gameObject, Vector3.one * 10f, 5f).setEase (LeanTweenType.easeInOutExpo);
	//		yield return new WaitForSeconds (5f);
	////		StartCoroutine (spawnKamikazeEnemiesCoroutine (100));
	//
	//	}
	//
	//
	//	IEnumerator spawnKamikazeEnemiesCoroutine (int numEnemies)
	//	{
	//		for (int i = 0; i < numEnemies; i++) {
	//			GameObject kamikazeEnemy = _kamikazeEnemyPooler.GetPooledObject ();
	//			kamikazeEnemy.transform.position = transform.position;
	//			kamikazeEnemy.SetActive (true);
	//			yield return new WaitForSeconds (.1f);
	//		}
	//		yield return new WaitForSeconds (1f);
	//		close ();
	//	}
	//
	//
	//
	//	void close ()
	//	{
	//		LeanTween.scale (gameObject, Vector3.zero * 10f, 5f).setEase (LeanTweenType.easeInOutExpo);
	//	}

}