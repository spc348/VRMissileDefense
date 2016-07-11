using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tower : InteractableObject
{
	[SerializeField] private ObjectPoolerScript _objectPoolerScript;

	[SerializeField] private GameObject _deathParticlesPrefab;
	[SerializeField] private GameObject _towerBlockPrefab;
	[SerializeField] private GameObject _startingBlock;
	[SerializeField] private GameObject _towerMovePos;
	[SerializeField] private Renderer _renderer;

	[SerializeField] private Slider _healthSlider;

	private bool _playerIsAtThisTower = false;
	private bool _playerIsMoving;
	[SerializeField] private Color _origColor;
	private Color _origColorLowAlpha;
	[SerializeField] private Color _highlightedColor;
	[SerializeField] private Color _hurtColor;
	[SerializeField] private int _health = 100;

	public TargetManager targetManager;

	void OnEnable() {
		GameEventManager.StartListening ("ResetAllTowers",reset);
	}

	void OnDisable() {
		GameEventManager.StopListening ("ResetAllTowers",reset);
	}
		
	// Use this for initialization
	public override void Start ()
	{

		base.Start ();
		_origColor = _renderer.material.color;
		_origColorLowAlpha = new Color(_origColor.r, _origColor.g, _origColor.b, .2f);

//		towerBlockSize = _towerBlockPrefab.GetComponent<Renderer> ().bounds.size;
//		StartCoroutine(makeTower());
	}

	public void heal (int amount)
	{
		StartCoroutine (showHealColor ());
		_health += amount;
		updateHealthBar ();
	}


	public void takeDamage (int amount)
	{
		StartCoroutine (showDamageColor ());

		_health -= amount;

		if (_health <= 0) {
			die ();
		}
		updateHealthBar ();
	}

	void updateHealthBar ()
	{
		_healthSlider.value = _health;
	}

	IEnumerator showDamageColor ()
	{
		_renderer.material.color = _hurtColor;
		yield return new WaitForSeconds (.1f);
		_renderer.material.color = _origColor;
	}

	IEnumerator showHealColor ()
	{
		_renderer.material.color = Color.green;
		yield return new WaitForSeconds (.1f);
		_renderer.material.color = _origColor;
	}

	void die ()
	{
		Instantiate (_deathParticlesPrefab, transform.position, Quaternion.identity);
		targetManager.targetGOs.Remove (gameObject);
		Destroy (gameObject);
	}

	public void SetGazedAt (bool gazedAt)
	{
		if (!_playerIsAtThisTower) {
			_renderer.material.color = gazedAt ? _highlightedColor : _origColor;
		}
	}

	public void flyPlayerToPos ()
	{
		if (!_playerIsMoving && !_playerIsAtThisTower) {
			StartCoroutine (flyPlayerToPosCoroutine ());
		}
	}

	IEnumerator flyPlayerToPosCoroutine ()
	{

		GameEventManager.TriggerEvent ("ResetAllTowers");
		_playerIsMoving = true;
		LeanTween.move (_player, _towerMovePos.transform.position, .2f).setEase (LeanTweenType.easeOutExpo);
		while (LeanTween.isTweening (_player)) {
			yield return null;
		}
		LeanTween.color (gameObject, _origColorLowAlpha, 1f);
		_playerIsMoving = false;
		_playerIsAtThisTower = true;

	}

	public void reset() {
		_playerIsAtThisTower = false;
		_renderer.material.color = _origColor;
	}


	//	IEnumerator makeTower ()
	//	{
	//		float xIncrease = 0;
	//		float yIncrease = 0;
	//		float zIncrease = 0;
	//		Vector3 addedDistance = new Vector3 (1, yIncrease, zIncrease);
	//		for (int i = 0; i < 1000; i++) {
	//
	//			xIncrease++;
	//
	//
	//			if ((xIncrease * zIncrease) >= (towerWidth * towerWidth)) {
	//				xIncrease = 0;
	//				yIncrease++;
	//				zIncrease = 0;
	//			}
	//
	//			if ((xIncrease >= towerWidth)) {
	//				xIncrease = 0;
	//				zIncrease++;
	//			}
	//			addedDistance = new Vector3 (xIncrease, yIncrease, zIncrease);
	//			//Lay down four blocks
	//			//increse Z by 1
	//			//lawy down four blocks
	//			//increase z by 1;
	//			//if greater than 4
	//
	//			GameObject towerBlock = _objectPoolerScript.GetPooledObject ();
	//			towerBlock.transform.position = _startingBlock.transform.position + addedDistance;
	//			towerBlock.SetActive (true);
	//
	//			yield return null;
	//		}
	////		towerBlock.transform.position =
	//		yield return null;
	//	}


}
