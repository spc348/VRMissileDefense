using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour
{

	[SerializeField] private List<GameObject> _trees = new List<GameObject> ();
	[SerializeField] private GameObject _cloudPrefab1;
	//	[SerializeField] private List<GameObject> _clouds = new List<GameObject> ();
	// Use this for initialization
	void Start ()
	{
//		placeTrees ();
//		placeClouds ();
	
	}

	void placeTrees ()
	{
	
		
	}

	void placeClouds ()
	{
		int numClouds = 50;
		for (int j = 0; j < numClouds; j++) {
			Vector3 cloudSpawnPos = new Vector3 (Random.Range (-1000000, 1000000), Random.Range (11000, 25000), Random.Range (-100000, 100000));
			GameObject cloud = Instantiate (_cloudPrefab1, cloudSpawnPos, Quaternion.identity) as GameObject;
		}
	}


}
