using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{


	[SerializeField] protected float _timeToWait = 2f;

	// Use this for initialization
	public virtual void Start ()
	{
		StartCoroutine (delayedDestroy (_timeToWait));
	}


	protected virtual IEnumerator delayedDestroy (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
