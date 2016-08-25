using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradesManager : Singleton<UpgradesManager> {

	[SerializeField] private GameObject _head;
	[SerializeField] private CanvasGroup _canvasGroup;
	
	private float minHeadLoweredTrigger = 50;
	private float maxHeadLoweredTrigger = 90;
	public int numTeslaBranches = 1;



	void Update() {
//		print (_head.transform.rotation.eulerAngles.x);
		if (_head.transform.rotation.eulerAngles.x > minHeadLoweredTrigger && _head.transform.rotation.eulerAngles.x < maxHeadLoweredTrigger) {
			_canvasGroup.alpha = 1;
		} else {
			_canvasGroup.alpha = 0;
		}
	}
}
