using UnityEngine;
using System.Collections;
using Gvr.Internal;
using TMPro;

public class Shooter : MonoBehaviour
{

	private Firework _firework;

	[SerializeField] private GameObject _fireworkPrefab;
	[SerializeField] private GameObject _spawnPos;
	[SerializeField] private GameObject _reticle;
	[SerializeField] private TextMeshProUGUI _lootText;

	public bool canShoot = true;


	private bool _fireworkPrimed = false;
	public int lootCount = 0;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GvrViewer.Instance.Triggered) {

			if (canShoot) {
				if (!_fireworkPrimed) {
					ShootFirework ();
					_fireworkPrimed = true;
				} else {
					_firework.explode ();
					_fireworkPrimed = false;
				}
			}
		}
	}

	public void ShootFirework ()
	{

		GameObject fireworkGO = Instantiate (_fireworkPrefab, _spawnPos.transform.position, Quaternion.identity) as GameObject;
		_firework = fireworkGO.GetComponent<Firework> ();

		Vector3 shootDir = (_reticle.transform.position - _spawnPos.transform.position).normalized;
		_firework.rb.AddForce (shootDir * 50f, ForceMode.Impulse);
	}

	public void increaseLootCount (int lootValue)
	{

		lootCount += lootValue;
		_lootText.text = "LOOT: " + lootCount.ToString ();

	}

	public void decreaseLootCount (int lootValue)
	{

		lootCount -= lootValue;
		_lootText.text = "LOOT: " + lootCount.ToString ();

	}
}
