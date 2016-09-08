using UnityEngine;
using System.Collections;

public class LockOnReticle : MonoBehaviour
{

	//this is your object that you want to have the UI element hovering over

	public GameObject target;
	[SerializeField] private RectTransform _canvasRect;
	[SerializeField] private RectTransform _lockOnReticleRect;
	[SerializeField] private Camera _camera;

	//then you calculate the position of the UI element
	//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
	void Update ()
	{

//		Vector2 targetPos = WorldObject.transform.position;
//		Vector2 viewportPos = Camera.main.WorldToViewportPoint (targetPos);
//		UI_Element.anchorMin = viewportPos;
//		UI_Element.anchorMax = viewportPos;
//
		Vector2 ViewportPosition = _camera.WorldToViewportPoint (target.transform.position);
		Vector2 WorldObject_ScreenPosition = new Vector2 (
			                                     ((ViewportPosition.x * _canvasRect.sizeDelta.x * 2f) - (_canvasRect.sizeDelta.x )),
			                                     ((ViewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)));

//		WorldObject_ScreenPosition.x = -WorldObject_ScreenPosition.x;
		//now you can set the position of the ui element
		_lockOnReticleRect.anchoredPosition = WorldObject_ScreenPosition;

		float dot = Vector3.Dot ((target.transform.position - _canvasRect.transform.position).normalized, Camera.main.transform.forward);
		if (dot <= 0) {
			_lockOnReticleRect.gameObject.SetActive (false);
		} else {
			_lockOnReticleRect.gameObject.SetActive (true);
				
		}// don't draw ui target marker
	
	}

}
