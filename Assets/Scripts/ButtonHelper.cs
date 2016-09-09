using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public void OnPointerEnter(PointerEventData eventData)
	{

		WeaponsManager.Instance.setReticleToSelector();
		WeaponsManager.Instance.canShoot = false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		WeaponsManager.Instance.setReticleToCrosshair();
		WeaponsManager.Instance.canShoot = true;

	}

}
