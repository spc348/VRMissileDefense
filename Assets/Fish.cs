//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//
//public class Fish : MonoBehaviour
//{
//
//	//	[SerializeField] private SpriteRenderer _spriteRenderer;
//	[SerializeField] private CircleCollider2D _circColl2D;
//	[SerializeField] private Rigidbody2D _rb2D;
//	public NDFishTank fishTank;
//	private RectTransform _fishTankRectTrans;
//	public List<GameObject> fishes;
//	public List<GameObject> walls;
//
//	public float maxSpeed = 3f;
//	float testCount;
//
//
//	// Use this for initialization
//	void Start ()
//	{
//		fishTank = GameObject.Find ("FishTank").GetComponent<NDFishTank> ();
//		_fishTankRectTrans = fishTank.GetComponent<RectTransform> ();
//		fishes = fishTank.fishPool;
//		walls = fishTank.walls;
//	}
//
//	// Update is called once per frame
//	void Update ()
//	{
//		separate (fishes);
//		separate (walls);
//	}
//
//	public void separate (List<GameObject> others)
//	{
//		float desiredSeparation = _circColl2D.radius * 2f;
//		Vector2 sum = new Vector2 ();
//		int count = 0;
//
//		foreach (GameObject other in others) {
//
//			if (other == null) {
//				print ("my other is null: " + gameObject.name);
//			}
//			if (other.activeInHierarchy) {
//				float distance = Vector2.Distance (transform.position, other.transform.position);
//				if ((distance > 0) && (distance < desiredSeparation)) {
//					Vector2 difference = other.transform.position - transform.position;
//					difference.Normalize ();
//					difference = difference / distance;
//					sum += difference;
//					count++;
//				} 
//			}
//		}
//
//		if (count > 0) {
//			sum = sum / count;
//			sum.Normalize ();
//
//			sum = sum * maxSpeed;
//			Vector2 steer = _rb2D.velocity - sum;
//			if (_rb2D.velocity.magnitude < 2f) {
//				_rb2D.AddForce (steer);
//			} 
//		}
//	}
//
//
//	void loopInTank ()
//	{
//		float buffer = 15f;
//		if (transform.localPosition.x > _fishTankRectTrans.rect.width / 2f) {
//			transform.localPosition = new Vector3 (-_fishTankRectTrans.rect.width / 2f + buffer, transform.localPosition.y);
//		}
//
//		if (transform.localPosition.x < -_fishTankRectTrans.rect.width / 2f - buffer) {
//			transform.localPosition = new Vector3 (_fishTankRectTrans.rect.width / 2f, transform.localPosition.y);
//
//		}
//		if (transform.localPosition.y > _fishTankRectTrans.rect.height / 2f) {
//			transform.localPosition = new Vector3 (transform.localPosition.y, -_fishTankRectTrans.rect.height / 2f + buffer);
//		}
//
//		if (transform.localPosition.y < -_fishTankRectTrans.rect.height / 2f) {
//			transform.localPosition = new Vector3 (transform.localPosition.y, _fishTankRectTrans.rect.height / 2f - buffer);
//		}
//
//	}
//
//
//}
