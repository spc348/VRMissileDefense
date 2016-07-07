using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class InteractableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {

	protected GameObject _player;
	protected Shooter _shooter;

	// Use this for initialization
	public virtual void Start () {
		_player = GameObject.Find ("Player");
		_shooter = _player.GetComponent<Shooter> ();
	}
	

	public void OnPointerEnter(PointerEventData eventData)
	{

		_shooter.canShoot = false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_shooter.canShoot = true;

	}
}
