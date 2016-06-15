using UnityEngine;
using System.Collections;
using Gvr.Internal;

public class Shooter : MonoBehaviour {

	[SerializeField] private GameObject _fireworkPrefab;
	[SerializeField] private GameObject _spawnPos;
	[SerializeField] private GameObject _reticle;
	private Firework _firework;
	private bool _fireworkPrimed = false;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (GvrViewer.Instance.Triggered) {
			if (!_fireworkPrimed) {
				ShootFirework();
				_fireworkPrimed = true;
			} else {
				_firework.explode();
				_fireworkPrimed = false;
			}
		}
	}

	public void ShootFirework() {

		GameObject fireworkGO = Instantiate(_fireworkPrefab, _spawnPos.transform.position, Quaternion.identity) as GameObject;
		_firework = fireworkGO.GetComponent<Firework>();

		Vector3 shootDir = (_reticle.transform.position - _spawnPos.transform.position).normalized;
		_firework.rb.AddForce(shootDir * 50f, ForceMode.Impulse);
//		_fireworkPrefab.
	}

}
