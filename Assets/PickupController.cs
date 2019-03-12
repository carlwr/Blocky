using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PickupController : TilemapController
{

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && collisionInfo.collider.gameObject.GetComponent<PlayerController>() != null)
        {
            foreach(ContactPoint2D hit in collisionInfo.contacts){
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                print(tilemap.WorldToCell(hitPosition));
                
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                //break;
            }
           
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (tilemap != null && collider.gameObject.GetComponent<PlayerController>() != null)
        {
            Vector2 hitPosition = GetComponent<Collider2D>().ClosestPoint(collider.transform.position);
            print(tilemap.GetTile(tilemap.WorldToCell(hitPosition)));
            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }
    }
}
