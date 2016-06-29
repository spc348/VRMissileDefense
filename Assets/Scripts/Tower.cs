using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tower : MonoBehaviour
{
	[SerializeField] private ObjectPoolerScript _objectPoolerScript;

	[SerializeField] private GameObject _deathParticlesPrefab;
	[SerializeField] private GameObject _towerBlockPrefab;
	[SerializeField] private GameObject _startingBlock;
	[SerializeField] private Renderer _renderer;

	[SerializeField] private Slider _healthSlider;

	[SerializeField] private Color _origColor;
	[SerializeField] private int _health = 100;


	//	Vector3 towerBlockSize;
	//	public int towerWidth = 9;

	// Use this for initialization
	void Start ()
	{

		_origColor = _renderer.material.color;
//		towerBlockSize = _towerBlockPrefab.GetComponent<Renderer> ().bounds.size;
//		StartCoroutine(makeTower());
	}

	public void heal(int amount) {
		StartCoroutine (showHealColor ());
		_health += amount;
		updateHealthBar();
	}


	public void takeDamage (int amount)
	{
		StartCoroutine (showDamageColor ());

		_health -= amount;

		if (_health <= 0) {
			die ();
		}
		updateHealthBar ();
	}

	void updateHealthBar ()
	{
		_healthSlider.value = _health;
	}

	IEnumerator showDamageColor ()
	{
		_renderer.material.color = Color.red;
		yield return new WaitForSeconds (.1f);
		_renderer.material.color = _origColor;
	}

	IEnumerator showHealColor ()
	{
		_renderer.material.color = Color.green;
		yield return new WaitForSeconds (.1f);
		_renderer.material.color = _origColor;
	}

	void die ()
	{
		Instantiate (_deathParticlesPrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}



	void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.CompareTag ("Enemy")) {
		}
		
	}



	//	IEnumerator makeTower ()
	//	{
	//		float xIncrease = 0;
	//		float yIncrease = 0;
	//		float zIncrease = 0;
	//		Vector3 addedDistance = new Vector3 (1, yIncrease, zIncrease);
	//		for (int i = 0; i < 1000; i++) {
	//
	//			xIncrease++;
	//
	//
	//			if ((xIncrease * zIncrease) >= (towerWidth * towerWidth)) {
	//				xIncrease = 0;
	//				yIncrease++;
	//				zIncrease = 0;
	//			}
	//
	//			if ((xIncrease >= towerWidth)) {
	//				xIncrease = 0;
	//				zIncrease++;
	//			}
	//			addedDistance = new Vector3 (xIncrease, yIncrease, zIncrease);
	//			//Lay down four blocks
	//			//increse Z by 1
	//			//lawy down four blocks
	//			//increase z by 1;
	//			//if greater than 4
	//
	//			GameObject towerBlock = _objectPoolerScript.GetPooledObject ();
	//			towerBlock.transform.position = _startingBlock.transform.position + addedDistance;
	//			towerBlock.SetActive (true);
	//
	//			yield return null;
	//		}
	////		towerBlock.transform.position =
	//		yield return null;
	//	}


}
