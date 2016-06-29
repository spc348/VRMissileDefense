using UnityEngine;
using System.Collections;
using TMPro;

public class DooberSplash : MonoBehaviour
{

	[SerializeField] private TextMeshPro _dooberText;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (fadeAndDestroy ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	IEnumerator fadeAndDestroy ()
	{
		yield return new WaitForSeconds (.5f);
		LeanTween.value (gameObject, _dooberText.color, Color.clear, .5f).setOnUpdate ((Color _c) => {
			_dooberText.color = _c;	
		});
		yield return new WaitForSeconds (.5f);
		Destroy (gameObject);
	}

	public void setText(int amount) {
		string prefix = "";
		_dooberText.text = amount.ToString();
	}
}
