using UnityEngine;
using System.Collections;
using Gvr.Internal;
using TMPro;
using SonicBloom.Koreo;


public class Shooter : MonoBehaviour
{

	[SerializeField] private AudioSource _audSource;
	private Mortar _mortar;

	[SerializeField] private Camera mainCam;

	[SerializeField] private GameObject _mortarPrefab;
	[SerializeField] private GameObject _spawnPos;
	[SerializeField] private GameObject _reticle;
	[SerializeField] private TextMeshProUGUI _lootText;

	public bool canShoot = true;

	delegate void ShootWeapon ();

	ShootWeapon shootWeapon;

	//Mortar
	private bool _mortarPrimed = false;


	//Pistol
	[SerializeField] private GameObject _pistolFlare;
	//	[SerializeField] private LineRenderer _lineRenderer;
	[SerializeField] private AudioClip _pistolFireClip;
	public float _fireRate = .25f;
	private float _nextFireTime;
	public ParticleSystem smokeParticles;
	public GameObject hitParticles;
	public float range = 100050;
	public float kEventCount = 0;

	public int lootCount = 0;

	// Use this for initialization
	void Start ()
	{
		SwitchWeapon ("pistol");

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (canShoot) {
			shootWeapon ();
		}
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

//			if (canShoot) {
			if (!_mortarPrimed) {
				GameObject fireworkGO = Instantiate (_mortarPrefab, _spawnPos.transform.position, Quaternion.identity) as GameObject;
				_mortar = fireworkGO.GetComponent<Mortar> ();

				Vector3 shootDir = (_reticle.transform.position - _spawnPos.transform.position).normalized;
				_mortar.rb.AddForce (shootDir * 50f, ForceMode.Impulse);
				_mortarPrimed = true;
			} else {
				_mortar.explode ();
				_mortarPrimed = false;
			}
//			}
		}


	}

	public void ShootPistol ()
	{

		RaycastHit hit;
//		Vector3 rayOrigin = mainCam.ViewportToWorldPoint (new Vector3 (.5f, .5f, 0));
		Vector3 rayOrigin = _reticle.transform.position;

		if (Input.GetButtonDown ("Fire1") && Time.time > _nextFireTime) {

			_nextFireTime = Time.time + _fireRate;

			if (Physics.SphereCast (rayOrigin, 2, mainCam.transform.forward, out hit, range)) {
				Enemy enemy = hit.collider.gameObject.GetComponent<Enemy> ();
				if (enemy != null) {
					enemy.takeDamage (10);

					if (hit.rigidbody != null) {
						hit.rigidbody.AddForce (-hit.normal * 1f, ForceMode.Impulse);
					}

//				_lineRenderer.SetPosition(0, _reticle.transform.position);
//				_lineRenderer.SetPosition(1, hit.point);
					Instantiate (hitParticles, hit.point, Quaternion.identity);
				}
			}
			StartCoroutine (PistolShotEffectCoroutine ());
		}

	}

	public void ShootRocket ()
	{

		print ("shooting rocket");
	}

	IEnumerator PistolShotEffectCoroutine ()
	{
		_pistolFlare.gameObject.SetActive (true);
		yield return new WaitForSeconds (.075f);
		_audSource.PlayOneShot (_pistolFireClip);
		_pistolFlare.gameObject.SetActive (false);
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
