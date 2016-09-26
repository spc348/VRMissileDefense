﻿using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

public class PowerUp : InteractableObject, IPointerDownHandler
{



	[SerializeField] private AudioSource _audSource;
	[SerializeField] private AudioClip _chaChingClip;
	[SerializeField] private Light _light;
	[SerializeField] private GameObject _powerUpParticlesPrefab;
	[SerializeField] private float _minLightSize = 10f;
	[SerializeField] private float _lightSizeMultiplier = 5f;

	public enum PowerUpType
	{
		MORTAR,
		TESLA,
		ROCKET

	}

	public PowerUpType pType;

	void OnEnable ()
	{
		pType = (PowerUpType)(Random.Range (0, 3));
	}


	void Update ()
	{
		pulseLight ();
	}

	public void pulseLight ()
	{
		_light.range = _minLightSize + Mathf.Cos (Time.time * _lightSizeMultiplier);
	}

	public virtual void OnPointerDown (PointerEventData eventData)
	{

		_audSource.PlayOneShot (_chaChingClip);
		GameObject particles = Instantiate (_powerUpParticlesPrefab, transform.position, Quaternion.Euler (new Vector3 (-90, 0, 0))) as GameObject;
		switch (pType) {
		case PowerUpType.MORTAR:
			print ("increasing mortar");
			WeaponsManager.Instance.increaseMortarAmmo ();
//			UpgradesManager.Instance.unlockMortar ();
			break;
		case PowerUpType.TESLA:
			print ("increasing teslsa");
			WeaponsManager.Instance.increaseTeslaAmmo ();
//			UpgradesManager.Instance.unlockTesla ();
			break;
		case PowerUpType.ROCKET:
			print ("increaseing rocket");
			WeaponsManager.Instance.increaseRocketAmmo ();
//			UpgradesManager.Instance.unlockRocket ();
			break;
		}

		WeaponsManager.Instance.delayedTurnOnShoot ();
		gameObject.SetActive (false);
	}

	IEnumerator delayedDeactivate() {
//		yield return new WaitForSeconds(
	}
}
