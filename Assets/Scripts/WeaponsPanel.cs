using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponsPanel : MonoBehaviour
{

	[SerializeField] CanvasGroup _canvasGroup;
	private bool _isShowing = false;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void toggleWeaponsPanel ()
	{
		print ("toggling");
		if (!_isShowing) {
			show ();
			print ("showing");
		} else {
			hide ();
			print ("hding");
		}
	}

	public void show ()
	{
		_isShowing = true;
		LeanTween.scale (gameObject, Vector3.one, .1f);
		LeanTween.value (gameObject, _canvasGroup.alpha, 1f, .2f).setOnUpdate ((float _a) => {
			_canvasGroup.alpha = _a;
		}).setEase(LeanTweenType.easeOutExpo); 
	}

	public void hide ()
	{
		_isShowing = false;
		LeanTween.scale (gameObject, Vector3.zero, .1f);
		LeanTween.value (gameObject, _canvasGroup.alpha, 0f, .2f).setOnUpdate ((float _a) => {
			_canvasGroup.alpha = _a;
		}); 
	}
}


