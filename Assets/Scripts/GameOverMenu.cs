using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
	
	[SerializeField] private GameObject WeaponsManager;
	[SerializeField] private GameObject UpgradesManager;
	[SerializeField] private GameObject EnemyManager;
	[SerializeField] private GameObject TargetManager;

	public void loadStartScene ()
	{
//		Destroy (WeaponsManager);
//		Destroy (UpgradesManager);
//		Destroy (EnemyManager);
//		Destroy (TargetManager);
		SceneManager.LoadScene ("StartScene");
	}
}
