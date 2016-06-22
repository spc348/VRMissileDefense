using UnityEngine;
using System.Collections;

public class AutoDestroyParticles : AutoDestroy
{

	[SerializeField] private ParticleSystem _particleSystem;

	public override void Start ()
	{
		_timeToWait = _particleSystem.startLifetime;
		base.Start ();
	}

}
