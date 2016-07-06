using UnityEngine;
using System.Collections;
using Gvr.Internal;
using TMPro;

public class Shooter : MonoBehaviour
{

	private Firework _mortar;

	[SerializeField] private GameObject _fireworkPrefab;
	[SerializeField] private GameObject _spawnPos;
	[SerializeField] private GameObject _reticle;
	[SerializeField] private TextMeshProUGUI _lootText;

	public bool canShoot = true;

	delegate void ShootWeapon ();

	ShootWeapon shootWeapon;

	private bool _mortarPrimed = false;
	public int lootCount = 0;



	// Use this for initialization
	void Start ()
	{
		SwitchWeapon("mortar");
	}
	
	// Update is called once per frame
	void Update ()
	{
		shootWeapon();
	}

	public void SwitchWeapon (string weaponName)
	{
		switch (weaponName) {
		case "mortar":
			shootWeapon = ShootMortar;
			break;
		case "pistol":
			shootWeapon = ShootPistol;
			break;
		default:
			break;
		}
	}

	public void ShootMortar ()
	{

		if (GvrViewer.Instance.Triggered) {

			if (canShoot) {
				if (!_mortarPrimed) {
					GameObject fireworkGO = Instantiate (_fireworkPrefab, _spawnPos.transform.position, Quaternion.identity) as GameObject;
					_mortar = fireworkGO.GetComponent<Firework> ();

					Vector3 shootDir = (_reticle.transform.position - _spawnPos.transform.position).normalized;
					_mortar.rb.AddForce (shootDir * 50f, ForceMode.Impulse);
					_mortarPrimed = true;
				} else {
					_mortar.explode ();
					_mortarPrimed = false;
				}
			}
		}


	}

	public void ShootPistol ()
	{

		if (GvrViewer.Instance.Triggered) {
			if (canShoot) {
				print ("shooting pistol");
			}
		}

	}

	public void ShootRocket ()
	{

		print ("shooting rocket");
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
