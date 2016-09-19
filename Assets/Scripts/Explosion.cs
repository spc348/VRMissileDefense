using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	[SerializeField] private AudioSource _audSource;
	[SerializeField] private AudioClip _boomClip;
	[SerializeField] private Light _light;
	// Use this for initialization
	void Start ()
	{
		_audSource.PlayOneShot (_boomClip);
		StartCoroutine (flash ());
	}

	IEnumerator flash ()
	{

		float flashTime = .1f;
		LeanTween.value (_light.gameObject, _light.range, 50f, flashTime).setOnUpdate ((float _r) => {
			_light.range = _r;
		});

		yield return new WaitForSeconds (flashTime);

		LeanTween.value (_light.gameObject, _light.range, 0f, flashTime).setOnUpdate ((float _r) => {
			_light.range = _r;
		});
	}
}
