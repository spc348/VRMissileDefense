using UnityEngine;
using System.Collections;

public class UpgradesAndWeaponsPanel : MonoBehaviour
{

	[SerializeField] private GameObject _head;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private InteractablePanel _weaponsPanel;
	[SerializeField] private InteractablePanel _upgradesPanel;

	private float minHeadLoweredTrigger = 40;
	private float maxHeadLoweredTrigger = 90;
	private bool _showingPanel;


	// Use this for initialization
	void Start ()
	{
		checkHeadPosition ();

	}

	void Update ()
	{
		checkHeadPosition ();
	}

	void checkHeadPosition ()
	{
		if (_head.transform.rotation.eulerAngles.x > minHeadLoweredTrigger && _head.transform.rotation.eulerAngles.x < maxHeadLoweredTrigger) {
			if (!_showingPanel) {
				LeanTween.cancel (gameObject);
				LeanTween.value (gameObject, _canvasGroup.alpha, 1f, .25f).setOnUpdate ((float _a) => {
					_canvasGroup.alpha = _a;
				});
				_showingPanel = true;
			}
		} else {
			if (_showingPanel) {
				LeanTween.cancel (gameObject);	
				LeanTween.value (gameObject, _canvasGroup.alpha, 0f, .25f).setOnUpdate ((float _a) => {
					_canvasGroup.alpha = _a;
				});
				_showingPanel = false;
			}
		}
	}

	public void showWeaponsPanel() {
		_upgradesPanel.hide ();
		_weaponsPanel.show ();
	}

	public void showUpgradesPanel() {
		_weaponsPanel.hide ();
		_upgradesPanel.show ();
	}

	}
