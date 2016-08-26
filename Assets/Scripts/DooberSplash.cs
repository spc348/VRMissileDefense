using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DooberSplash : MonoBehaviour
{

	[SerializeField] private TextMeshPro _dooberText;

	// Use this for initialization
	void Start ()
	{
		GameObject player = GameObject.Find ("Player");
		transform.LookAt (2*transform.position - player.transform.position);
//		transform.rotation = Quaternion.Euler (transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
		StartCoroutine (fadeAndDestroy ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	IEnumerator fadeAndDestroy ()
	{

		yield return new WaitForSeconds (.5f);

//		LeanTween.value (gameObject, _dooberText.color, Color.clear, .5f).setOnUpdate ((Color _c) => {
//			_dooberText.color = _c;	
//		});
		yield return new WaitForSeconds (.5f);
		Destroy (gameObject);
	}

	public void setText (float amount)
	{
		string prefix = "";
		_dooberText.text = amount.ToString ();
	}
}
