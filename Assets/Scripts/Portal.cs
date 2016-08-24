using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{

	[SerializeField] private ObjectPoolerScript _kamikazeEnemyPooler;
	[SerializeField] private ObjectPoolerScript _airShooterEnemyPooler;

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

	IEnumerator openCoroutine ()
	{
		yield return new WaitForSeconds (2f);
		LeanTween.scale (gameObject, Vector3.one * 10f, 5f).setEase (LeanTweenType.easeInOutExpo);
		yield return new WaitForSeconds (5f);
		StartCoroutine (spawnKamikazeEnemiesCoroutine (100));

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
			kamikazeEnemy.SetActive (true);
			yield return new WaitForSeconds (.1f);
		}
		yield return new WaitForSeconds (1f);
		close ();
	}

//	IEnumerator spawnShooterEnemiesCoroutine (int numEnemies)
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


	void close ()
	{
		LeanTween.scale (gameObject, Vector3.zero * 10f, 5f).setEase (LeanTweenType.easeInOutExpo);
	}

}