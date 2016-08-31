﻿using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public float Health {
		get { return _health; }
	}

	[SerializeField] protected float _origMaxHealth;
	protected float _health;
	protected int _strength;
			

}
