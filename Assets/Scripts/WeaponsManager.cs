using UnityEngine;
using System.Collections;
using Gvr.Internal;
using TMPro;


public class WeaponsManager : Singleton<WeaponsManager>
{

	[SerializeField] private AudioSource _audSource;
	private Mortar _mortar;

	[SerializeField] private Camera mainCam;

	[SerializeField] private GameObject _mortarPrefab;
	[SerializeField] private GameObject _spawnPos;
	[SerializeField] private GameObject _reticle;
	[SerializeField] private SpriteRenderer _reticleSpriteRenderer;

	[SerializeField] private TextMeshProUGUI _lootText;
	[SerializeField] private Sprite crosshairMachineGun;
	[SerializeField] private Sprite crosshairMortar;
	public bool canShoot = true;
	delegate void ShootWeapon ();
	ShootWeapon shootWeapon;

	//Mortar
	private bool _mortarPrimed = false;

	//MachineGun
		[SerializeField] private LineRenderer _lineRenderer;
	[SerializeField] private AudioClip _pistolFireClip;
	public float _fireRate = .25f;
	private float _nextFireTime;
	public ParticleSystem smokeParticles;
	public GameObject hitParticles;
	private float range = Mathf.Infinity;

	public int lootCount = 0;

	// Use this for initialization
	void Start ()
	{
		SwitchWeapon ("machineGun");

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
			_reticleSpriteRenderer.sprite = crosshairMortar;
			shootWeapon = ShootMortar;
			break;
		case "machineGun":
			_reticleSpriteRenderer.sprite = crosshairMachineGun;
			shootWeapon = ShootMachineGun;
			break;
		default:
			break;
		}
	}


	public void ShootMortar ()
	{
		if (GvrViewer.Instance.Triggered) {
			if (!_mortarPrimed) {
				GameObject mortarGO = Instantiate (_mortarPrefab, _spawnPos.transform.position, Quaternion.identity) as GameObject;
				_mortar = mortarGO.GetComponent<Mortar> ();

				Vector3 shootDir = (_reticle.transform.position - _spawnPos.transform.position).normalized;
				_mortar.rb.AddForce (shootDir * 50f, ForceMode.Impulse);
				_mortarPrimed = true;
			} else {
				_mortar.explode ();
				_mortarPrimed = false;
			}
		}
	}

	public void ShootMachineGun ()
	{

		RaycastHit hit;
		Vector3 rayOrigin = _reticle.transform.position;

		if (Input.GetButton ("Fire1") && Time.time > _nextFireTime) {
			_nextFireTime = Time.time + _fireRate;
			_audSource.PlayOneShot (_pistolFireClip);

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
		}

	}


	public void ShootArcLightening ()
	{

		RaycastHit hit;
		Vector3 rayOrigin = _reticle.transform.position;

		if (Input.GetButton ("Fire1") && Time.time > _nextFireTime) {
			_nextFireTime = Time.time + _fireRate;
			_audSource.PlayOneShot (_pistolFireClip);

			if (Physics.SphereCast (rayOrigin, 2, mainCam.transform.forward, out hit, range)) {
				Enemy enemy = hit.collider.gameObject.GetComponent<Enemy> ();
				if (enemy != null) {
					enemy.takeDamage (10);

					if (hit.rigidbody != null) {
						hit.rigidbody.AddForce (-hit.normal * 1f, ForceMode.Impulse);
					}

									_lineRenderer.SetPosition(0, _reticle.transform.position);
									_lineRenderer.SetPosition(1, hit.point);
					Instantiate (hitParticles, hit.point, Quaternion.identity);
				}
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
