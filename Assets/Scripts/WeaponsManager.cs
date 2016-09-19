using UnityEngine;
using System.Collections;
using Gvr.Internal;
using UnityEngine.UI;


public class WeaponsManager : Singleton<WeaponsManager>
{
	[SerializeField] private AudioSource _audSource;
	[SerializeField] private AudioClip _switchWeaponClip;

	private Mortar _mortar;
	[SerializeField] private ObjectPoolerScript _rocketPooler;
	[SerializeField] private LockOnManager _lockOnManagerLeft;
	[SerializeField] private LockOnManager _lockOnManagerRight;

	[SerializeField] private Camera mainCam;

	[SerializeField] private GameObject _mortarPrefab;
	[SerializeField] private GameObject _spawnPos;
	[SerializeField] private GameObject _reticle;
	[SerializeField] private GameObject _selectorReticle;
	[SerializeField] private GameObject _laserEnd;
	[SerializeField] private GameObject _rocketLaunchPos;
	private GameObject _rocketTarget;

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
	[SerializeField] private AudioClip _machineGunFireClip;
	[SerializeField] private AudioClip _rocketFireClip;
	[SerializeField] private AudioClip _mortarLaunchClip;

	private bool _isFirstWeaponEquip = true;
	public float _fireRate = .25f;
	private float _nextFireTime;
	public ParticleSystem smokeParticles;
	public GameObject hitParticles;
	private float range = Mathf.Infinity;
	private int numTeslaPoint = 49;
	public bool isLockedOn = false;

	public int mortarAmmo;
	public int TeslaAmmo;
	public int rocketAmmo;
	private int maxMortarAmmo = 5;
	private int maxTeslaAmmo = 50;
	private int maxRocketAmmo = 2;

	public int lootCount = 0;

	// Use this for initialization
	void Start ()
	{
		SwitchWeapon ("machineGun");
		_isFirstWeaponEquip = false;
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
		if (!_isFirstWeaponEquip) {
			_audSource.PlayOneShot (_switchWeaponClip);
		}

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
			shootWeapon = ShootTesla;
			break;
		case "rocket":
			_reticleSpriteRenderer.sprite = _crosshairTesla;
			shootWeapon = ShootRocket;
			break;
		default:
			break;
		}
	}


	public void ShootMortar ()
	{
		if (GvrViewer.Instance.Triggered) {
			if (!_mortarPrimed) {
				_audSource.PlayOneShot (_mortarLaunchClip);
				GameObject mortarGO = Instantiate (_mortarPrefab, _spawnPos.transform.position, Quaternion.identity) as GameObject;
				_mortar = mortarGO.GetComponent<Mortar> ();

				Vector3 shootDir = (_laserEnd.transform.position - _reticle.transform.position).normalized;
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
			_audSource.PlayOneShot (_machineGunFireClip);

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


	public void ShootTesla ()
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
		RaycastHit hit;
		Vector3 rayOrigin = _reticle.transform.position;
		if (Input.GetButton ("Fire1")) {
			if (Physics.SphereCast (rayOrigin, 2, mainCam.transform.forward, out hit, range)) {
				if (hit.collider.CompareTag ("Enemy")) {
					_rocketTarget = hit.collider.gameObject;
					lockOnTarget (_rocketTarget);
				}
			}	
		} else {
			if (isLockedOn) {
				launchRocket (_rocketTarget);
				isLockedOn = false;
			}
			cancelLockOn ();
		}
	}

	void launchRocket (GameObject target)
	{
		GameObject rocket = _rocketPooler.GetPooledObject ();

		rocket.transform.position = _rocketLaunchPos.transform.position;
		rocket.GetComponent<Rocket> ().target = target;
		rocket.SetActive (true);
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

	public void setReticleToSelector ()
	{
		_reticle.SetActive (false);
		_selectorReticle.SetActive (true);
	}

	public void setReticleToCrosshair ()
	{
		_reticle.SetActive (true);
		_selectorReticle.SetActive (false);
	}

	void lockOnTarget (GameObject target)
	{
		_lockOnManagerLeft.startLockOnProcess (target);
		_lockOnManagerRight.startLockOnProcess (target);
	}

	void cancelLockOn ()
	{
		_lockOnManagerLeft.endLockOnProcess ();
		_lockOnManagerRight.endLockOnProcess ();
	}


	public void delayedTurnOnShoot() {
		StartCoroutine (delayedTurnOnShootCoroutine ());
	}
	IEnumerator delayedTurnOnShootCoroutine ()
	{
		yield return new WaitForSeconds (.1f);
		WeaponsManager.Instance.setReticleToCrosshair ();
		WeaponsManager.Instance.canShoot = true;
	}
}
