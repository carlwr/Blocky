using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PickupsControlloer : MonoBehaviour {

	Tilemap tilemap;
	// Use this for initialization
	void Start () {
		tilemap = GetComponent<Tilemap>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		Vector2 contactPoint = collisionInfo.contacts[0].point;
		tilemap.SetTile(new Vector3Int((int)contactPoint.x,
						(int)contactPoint.y-1,
						0), null);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		
		
	}
}
