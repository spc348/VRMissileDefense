using UnityEngine;
using System.Collections;

public class Firework : MonoBehaviour
{
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private Renderer _renderer;
	public Rigidbody rb;
	[SerializeField] private GameObject _fireworkParticlesPrefab;

	[SerializeField] private Color[] _colors;
	[SerializeField] private Color _color;

	[SerializeField] private float _timeToWait = 3f;

	public float explosionRadius = 5f;
	public float explosionPower = 10.0F;
	// Use this for initialization
	void Start ()
	{
		_color = _colors [Random.Range (0, _colors.Length)];
		_renderer.material.color = _color;
//		_timeToWait = Random.Range(.1f, 1f);
//		StartCoroutine(delayedExplode());
	}

	IEnumerator delayedExplode ()
	{
		yield return new WaitForSeconds (_timeToWait);
		explode ();
	}

	public void explode ()
	{

		Vector3 explosionPos = transform.position;
//		print ("radius: " + explosionRadius);

		Collider[] colliders = Physics.OverlapSphere (explosionPos, explosionRadius);

		for (int i = 0; i < colliders.Length; i++) {
			if (colliders [i].gameObject.CompareTag ("Enemy")) {
				colliders [i].GetComponent<Enemy> ().Die ();
			}
		}

		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null)
				rb.AddExplosionForce (explosionPower, explosionPos, explosionRadius, 3.0F, ForceMode.Impulse);

		}

		_renderer.enabled = false;
		GameObject fireworkParticles = Instantiate (_fireworkParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		fireworkParticles.GetComponent<ParticleSystem> ().startColor = _color;
		_audioSource.Play ();
		Destroy (gameObject, 2f);
	}
}