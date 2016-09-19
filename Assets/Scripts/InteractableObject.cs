using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class InteractableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


	public virtual void OnPointerEnter (PointerEventData eventData)
	{
		WeaponsManager.Instance.setReticleToSelector ();
		WeaponsManager.Instance.canShoot = false;
	}

	public virtual void OnPointerExit (PointerEventData eventData)
	{
		WeaponsManager.Instance.setReticleToCrosshair ();
		WeaponsManager.Instance.canShoot = true;

	}


}
