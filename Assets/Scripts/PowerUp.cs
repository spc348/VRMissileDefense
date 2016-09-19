using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

public class PowerUp : InteractableObject, IPointerDownHandler
{



	[SerializeField] private Light _light;
	[SerializeField] private GameObject _powerUpParticlesPrefab;

	[SerializeField] private float _minLightSize = 3f;
	[SerializeField] private float _lightSizeMultiplier = 5f;

	public enum PowerUpType
	{
		MORTAR,
		TESLA,
		ROCKET

	}

	public PowerUpType pType;

	void Start ()
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

		GameObject particles = Instantiate (_powerUpParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		switch (pType) {
		case PowerUpType.MORTAR:
			UpgradesManager.Instance.unlockMortar ();
			break;
		case PowerUpType.TESLA:
			UpgradesManager.Instance.unlockTesla ();
			break;
		case PowerUpType.ROCKET:
			UpgradesManager.Instance.unlockRocket ();
			break;
		}
		Destroy (gameObject);

	}

}
