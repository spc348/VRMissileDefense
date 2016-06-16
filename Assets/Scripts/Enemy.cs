using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
	
	[SerializeField] private AudioSource audSource;
	[SerializeField] private AudioClip deathBoomClip;
	[SerializeField] private Collider _collider;

//	public EnemyManager EManager() { GetType {}} 
	[SerializeField] private EnemyManager _enemyManager;

	public GameObject Target {
		get { return _target; }
		set {
			_target = value;
			_targetSet = true;
		}
	}

	[SerializeField] private GameObject _target;
	[SerializeField] private bool _targetSet = false;
	[SerializeField] private Renderer _renderer;
	[SerializeField] private ParticleSystem deathExplosion;
	[SerializeField] private Slider _healthSlider;

	[SerializeField] private float _rotationSpeed = 10f;
	[SerializeField] public float moveSpeed = 4f;

	public bool stunned = false;

	public float stunnedCountdown = 5f;
	private int health = 10;




	// Use this for initialization
	void Start ()
	{
//		_target = GameObject.Find ("Player");	
//		updateHealthBar();
	}

	// Update is called once per frame
	void Update ()
	{
		if (_targetSet) {
			if (!stunned) {
				MoveTowardTarget ();
			} else {
				stunnedCountdown -= Time.deltaTime;
				if (stunnedCountdown <= 0) {
					stunnedCountdown = 5f;
					stunned = false;
				}
			}
		}


	}

	public void Die ()
	{
		StartCoroutine(delayedDie());
	}

	IEnumerator delayedDie ()
	{
		yield return new WaitForSeconds (.5f);
		_renderer.enabled = false;
		_collider.enabled = false;

		Instantiate (deathExplosion, transform.position, Quaternion.identity);
		gameObject.SetActive(false);
//				audSource.PlayOneShot (deathBoomClip);
//				Destroy (gameObject, deathBoomClip.length);
	}

	public void MoveTowardTarget ()
	{
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (_target.transform.position - transform.position), _rotationSpeed * Time.fixedDeltaTime);
		RaycastHit hit;
		float offsetAlign = 0f;
		if (Physics.Raycast (transform.position, Vector3.down, out hit)) {
			//			transform.position = new Vector3(transform.position.x, transform.position.y + hit.point.y + offsetAlign, transform.position.z);
		}
		transform.position += transform.forward * moveSpeed * Time.deltaTime;
		Debug.DrawLine (transform.position, hit.point, Color.cyan);
	}


	public void updateHealthBar ()
	{
		_healthSlider.value = health;

	}

	public void takeDamage ()
	{
		health--;
		updateHealthBar ();
	}


	void OnCollisionEnter (Collision coll)
	{
		
	}
}
