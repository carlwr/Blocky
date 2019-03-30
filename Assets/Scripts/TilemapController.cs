using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    protected Tilemap tilemap;

    
    // Start is called before the first frame update
    virtual protected void Start()
    {
		tilemap = GetComponent<Tilemap>();
    }


    protected bool isEmptyTilePlace(Vector3Int tilePlace, string collisionExceptionTag = null){
        Vector3 tilemapWorld = tilemap.CellToWorld(tilePlace);
        tilemapWorld.x += 0.5f;
        tilemapWorld.y += 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(tilemapWorld.x, tilemapWorld.y),
											 Vector2.zero);
                                             
        Debug.DrawRay(new Vector2(tilemapWorld.x, tilemapWorld.y), Vector2.zero , Color.green,0.1f);
        
        
        if(hit.collider == null || hit.collider.tag != "Plattform" && hit.collider.tag != "Player"){
            return true;
        }
        return false;
    }

    protected bool isEmptyTilePlaceForJump(Vector3Int tilePlace, string collisionExceptionTag = null){
        Vector3 tilemapWorld = tilemap.CellToWorld(tilePlace);
        tilemapWorld.x += 0.5f;
        tilemapWorld.y += 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(tilemapWorld.x, tilemapWorld.y),
											 Vector2.zero);
                                             
        Debug.DrawRay(new Vector2(tilemapWorld.x, tilemapWorld.y), Vector2.zero , Color.green,0.1f);
        
        
        if(hit.collider == null || hit.collider.tag != "Plattform"){
            return true;
        }
        return false;
    }

}
