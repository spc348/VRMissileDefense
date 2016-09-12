using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradesManager : Singleton<UpgradesManager> {

	private bool _isShowing;

	public float machineGunStrength = 1;
	public int numTeslaBranches = 1;

	public float rocketStrength = 10f;
	public float rocketLockOnSpeed = 1f;

}
