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

	void OnCollisionEnter2D(Collision2D collision)
	{
		Vector3 hitPosition = Vector3.zero;
        
        if (tilemap != null && collision.collider.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Collision");
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
        }
	}

}
