using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradesManager : Singleton<UpgradesManager> {

	[SerializeField] private ObjectPoolerScript _powerUpPooler;
	[SerializeField] private AudioSource _audSource;
	[SerializeField] private AudioClip _chaChingClip;
	[SerializeField] private Button _mortarButton;
	[SerializeField] private Button _mortarUnlockButton;
	[SerializeField] private Button _teslaButton;
	[SerializeField] private Button _teslaUnlockButton;
	[SerializeField] private Button _rocketButton;
	[SerializeField] private Button _rocketUnlockButton;

	private bool _isShowing;

	public int lootCount = 0;

	public float machineGunStrength = 1;
	public int numTeslaBranches = 1;

	public float rocketStrength = 10f;
	public float rocketLockOnSpeed = 1f;

	private int _mortarUnlockCost = 10;
	private int _teslaUnlockCost = 20;
	private int _rocketUnlockCost = 20;

	public void unlockMortar() {
		_audSource.PlayOneShot (_chaChingClip);
		_mortarButton.interactable = true;
		_mortarUnlockButton.gameObject.SetActive (false);
	}

	public void unlockTesla() {
		_audSource.PlayOneShot (_chaChingClip);
		_teslaButton.interactable = true;
		_teslaUnlockButton.gameObject.SetActive (false);
	}

	public void unlockRocket() {
		_audSource.PlayOneShot (_chaChingClip);
		_rocketButton.interactable = true;
		_rocketUnlockButton.gameObject.SetActive (false);
	}

	public void dropPowerUp(Vector3 pos) {
		GameObject powerUp = _powerUpPooler.GetPooledObject ();
		powerUp.transform.position = pos;
		powerUp.SetActive (true);
	}

}
