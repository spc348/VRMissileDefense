using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolerScript : MonoBehaviour
{

	//	public static ObjectPoolerScript current; //This was in the tutorial but I forget why. Why should this be static? Wouldn't you want to have multiple objectPoolers for different objects?
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool willGrow = true;


	[SerializeField] public List<GameObject> pooledObjects;


	// Use this for initialization
	void Awake ()
	{
		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (pooledObject);

			obj.SetActive (false);
			pooledObjects.Add (obj);
		}
	}

	public GameObject GetPooledObject ()
	{

		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].activeInHierarchy) {
				return pooledObjects [i];
			}
		}

		if (willGrow) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			pooledObjects.Add (obj);
			return obj;
		}

		return null;

	}

}