using UnityEngine;
using System.Collections;
using Gvr.Internal;


public class WeaponsManager : Singleton<WeaponsManager>
{

	[SerializeField] private AudioSource _audSource;
	private Mortar _mortar;

	[SerializeField] private Camera mainCam;

	[SerializeField] private GameObject _mortarPrefab;
	[SerializeField] private GameObject _spawnPos;
	[SerializeField] private GameObject _reticle;
	[SerializeField] private GameObject _laserEnd;
	[SerializeField] private SpriteRenderer _reticleSpriteRenderer;


	//	[SerializeField] private TextMeshProUGUI _lootText;
	[SerializeField] private Sprite _crosshairMachineGun;
	[SerializeField] private Sprite _crosshairMortar;
	[SerializeField] private Sprite _crosshairTesla;

	public bool canShoot = true;


	public delegate void CancelTeslaEvent ();

	public static event CancelTeslaEvent OnCancelTesla;

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
	private int numTeslaPoint = 49;

	public int lootCount = 0;

	// Use this for initialization
	void Start ()
	{
		SwitchWeapon ("mortar");

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
			_reticleSpriteRenderer.sprite = _crosshairMortar;
			shootWeapon = ShootMortar;
			break;
		case "machineGun":
			_reticleSpriteRenderer.sprite = _crosshairMachineGun;
			shootWeapon = ShootMachineGun;
			break;
		case "tesla":
			_reticleSpriteRenderer.sprite = _crosshairTesla;
			shootWeapon = ShootArcLightening;
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
					enemy.takeDamage (UpgradesManager.Instance.machineGunStrength);

					if (hit.rigidbody != null) {
						hit.rigidbody.AddForce (-hit.normal * 1f, ForceMode.Impulse);
					}
					Instantiate (hitParticles, hit.point, Quaternion.identity);
				}
			}
		}
	}


	public void ShootArcLightening ()
	{

		RaycastHit hit;
		Vector3 rayOrigin = _reticle.transform.position;

		if (Input.GetButton ("Fire1")) {
			_lineRenderer.enabled = true;
//			_nextFireTime = Time.time + _fireRate;

			Vector3 dir = _laserEnd.transform.position - _reticle.transform.position;
			float distance = Vector3.Distance (_reticle.transform.position, _laserEnd.transform.position);
			float step = distance / numTeslaPoint;
			_lineRenderer.SetPosition (0, _reticle.transform.position - new Vector3 (0, 1));
			_lineRenderer.SetPosition (1, _laserEnd.transform.position);

//			_lineRenderer.SetPosition (numTeslaPoint, _laserEnd.transform.position);
//			for (int i = 1; i < numTeslaPoint; i++) {
//				float error1 = Random.Range (-.2f, .2f);
//				float error2 = Random.Range (-.2f, .2f);
//				float error3 = Random.Range (-.1f, .1f);
////
//				_lineRenderer.SetPosition (i, _reticle.transform.position + (dir.normalized * i * step) + new Vector3 (error1, error2, error3));
//				
//			}
			if (Physics.SphereCast (rayOrigin, 2, mainCam.transform.forward, out hit, range)) {
				if (hit.collider.CompareTag ("Enemy")) {
					
					Enemy enemy = hit.collider.gameObject.GetComponent<Enemy> ();
					enemy.doTesla (10, 0);
//					enemy.takeDamage (10);
//
//					if (hit.rigidbody != null) {
//						hit.rigidbody.AddForce (-hit.normal * 1f, ForceMode.Impulse);
//					}


//					Instantiate (hitParticles, hit.point, Quaternion.identity);
				
				}
			} else {
				cancelTesla ();
			}
		} else {
			_lineRenderer.enabled = false;
			cancelTesla ();
		}

	}

	void cancelTesla ()
	{
		if (OnCancelTesla != null) {
			OnCancelTesla ();
		}

	}

	public void ShootRocket ()
	{
		print ("shooting rocket");
	}

	public void increaseLootCount (int lootValue)
	{
		lootCount += lootValue;
//		_lootText.text = "LOOT: " + lootCount.ToString ();
	}

	public void decreaseLootCount (int lootValue)
	{
		lootCount -= lootValue;
//		_lootText.text = "LOOT: " + lootCount.ToString ();
	}
}
