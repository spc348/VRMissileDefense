using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Target : MonoBehaviour
{

	[SerializeField] private Renderer _renderer1;
	[SerializeField] private Renderer _renderer2;

	[SerializeField] private Slider _healthSlider;

	[SerializeField] private int _maxHealth = 100;
	[SerializeField] private int _health = 100;
	[SerializeField] private Color _hurtColor;
	[SerializeField] private Color _origColor;

	[SerializeField] private GameObject _deathParticlesPrefab;

	public delegate void GameOverEvent();
	public static event GameOverEvent OnGameOver;

	void Start ()
	{
		_origColor = _renderer1.material.color;
		updateHealthBar();
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
		_renderer1.material.color = _hurtColor;		
		_renderer2.material.color = _hurtColor;

		yield return new WaitForSeconds (.1f);
		_renderer1.material.color = _origColor;
		_renderer2.material.color = _origColor;

	}

	IEnumerator showHealColor ()
	{
		_renderer1.material.color = Color.green;
		_renderer2.material.color = Color.green;

		yield return new WaitForSeconds (.1f);
		_renderer1.material.color = _origColor;
		_renderer2.material.color = _origColor;

	}

	void die ()
	{
		GameObject explosion = Instantiate (_deathParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		OnGameOver();
		Destroy (gameObject);

	}
}
