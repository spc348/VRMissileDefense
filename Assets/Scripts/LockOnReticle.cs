using UnityEngine;
using System.Collections;

public class LockOnReticle : MonoBehaviour
{

	public RectTransform canvasRectT;
	public RectTransform _image;
	public Transform objectToFollow;

	void Update()
	{
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, objectToFollow.position);
		_image.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
	}

}
