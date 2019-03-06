using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PickupController : MonoBehaviour
{
    Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Debug.Log("BEEP");
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && collisionInfo.collider.name == "Player")
        {
            foreach(ContactPoint2D hit in collisionInfo.contacts){
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                print(tilemap.WorldToCell(hitPosition));
                
                //tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                //break;
            }
           
        }
        
        
    }
}
