using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField] private Shooter _shooter;
	[SerializeField] private Tower _tower;
	[SerializeField] private int _itemCost = 10;
	private int _amountToHeal = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void purchaseItem() {

		if (_shooter.lootCount >= _itemCost) {
			_shooter.decreaseLootCount(_itemCost);
			_tower.heal(_amountToHeal);
		} else {
			print ("not enough loot");
		}
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
