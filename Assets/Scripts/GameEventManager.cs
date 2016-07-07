﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameEventManager : MonoBehaviour {
	//Game = Normal Distribution Explorer
	private Dictionary <string, UnityEvent> eventDictionary;
	private static GameEventManager eventManager;
	public static GameEventManager instance 
	{
		get 
		{
			if (!eventManager) 
			{
				eventManager = FindObjectOfType(typeof (GameEventManager)) as GameEventManager;
				if (!eventManager) {
					Debug.LogError("There needs to be on active GameEventManager script on a GameObject in your scene.");
				}  
				else 
				{
					eventManager.Init();
				}
			}
			return eventManager;
		}
	}

	void Init() 
	{
		if (eventDictionary == null) {
			eventDictionary = new Dictionary<string, UnityEvent>();
		}
	}

	public static void StartListening (string eventName, UnityAction listener) 
	{
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.AddListener (listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add (eventName, thisEvent);
		}
	}

	public static void StopListening (string eventName, UnityAction listener) 
	{
		if (eventManager == null) return;
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) 
		{
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent (string eventName)
	{
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) 
		{
			thisEvent.Invoke();
		}
	}

}
