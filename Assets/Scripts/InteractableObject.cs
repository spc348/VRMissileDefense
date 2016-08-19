using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class InteractableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {

	protected GameObject _player;
	protected WeaponsManager _shooter;

	// Use this for initialization
	public virtual void Start () {
		_player = GameObject.Find ("Player");
		_shooter = _player.GetComponent<WeaponsManager> ();
	}
	

	public virtual void OnPointerEnter(PointerEventData eventData)
	{

		_shooter.canShoot = false;
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		_shooter.canShoot = true;

	}
}
