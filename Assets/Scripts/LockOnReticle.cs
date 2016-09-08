using UnityEngine;
using System.Collections;

public class LockOnReticle : MonoBehaviour
{

	//this is your object that you want to have the UI element hovering over
	public GameObject WorldObject;

	public RectTransform CanvasRect;
	//this is the ui element
	public RectTransform UI_Element;
	public Camera camera;

	void Start ()
	{
		//first you need the RectTransform component of your canvas
	
	}

	//then you calculate the position of the UI element
	//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
	void Update ()
	{

//		Vector2 targetPos = WorldObject.transform.position;
//		Vector2 viewportPos = Camera.main.WorldToViewportPoint (targetPos);
//		UI_Element.anchorMin = viewportPos;
//		UI_Element.anchorMax = viewportPos;
//
		Vector2 ViewportPosition = camera.WorldToViewportPoint (WorldObject.transform.position);
		Vector2 WorldObject_ScreenPosition = new Vector2 (
			                                     ((ViewportPosition.x * CanvasRect.sizeDelta.x * 2f) - (CanvasRect.sizeDelta.x )),
			                                     ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

//		WorldObject_ScreenPosition.x = -WorldObject_ScreenPosition.x;
		//now you can set the position of the ui element
		print ("pos: " + WorldObject_ScreenPosition);
		UI_Element.anchoredPosition = WorldObject_ScreenPosition;

		float dot = Vector3.Dot ((WorldObject.transform.position - CanvasRect.transform.position).normalized, Camera.main.transform.forward);
		if (dot <= 0) {
			UI_Element.gameObject.SetActive (false);
		} else {
			UI_Element.gameObject.SetActive (true);
				
		}// don't draw ui target marker
	
	}
	//	public RectTransform canvasRectT;
	//	public RectTransform _reticle;
	//	public Transform objectToFollow;
	////	public Camera _cam;
	//	Vector2 screenPoint;
	//
	//	void Update ()
	//	{
	//		screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main,objectToFollow.position);
	//		screenPoint.y = Screen.height - screenPoint.y;
	//
	//		_reticle.transform.position = screenPoint;
	//
	////		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, objectToFollow.position);
	////		_image.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
	//	}

}
