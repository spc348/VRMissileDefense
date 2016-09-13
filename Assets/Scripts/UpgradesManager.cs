using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradesManager : Singleton<UpgradesManager> {

	[SerializeField] private AudioSource _audSource;
	[SerializeField] private AudioClip _chaChingClip;
	[SerializeField] private Button _mortarButton;
	[SerializeField] private Button _mortarUnlockButton;
	[SerializeField] private Button _teslaButton;
	[SerializeField] private Button _rocketButton;



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




}
