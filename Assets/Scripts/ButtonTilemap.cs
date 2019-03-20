using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class ButtonTilemap : TilemapController
{
    TilemapCollider2D tileMapCollider;
    TileBase[] tileBases;
    public TileBase bridgeOpen;
    public TileBase bridgeClosed;
    public TileBase buttonPressed;
    public TileBase buttonNotPressed;
    

    void Start()
    {
        base.Start();
        tileMapCollider = gameObject.GetComponent<TilemapCollider2D>();
        BoundsInt bounds = tilemap.cellBounds;
        tileBases = tilemap.GetTilesBlock(bounds);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (tilemap != null && collider.gameObject.tag == "Player")
        {
            Vector2 hitPosition = GetComponent<Collider2D>().ClosestPoint(collider.transform.position);
            Vector3Int hitPositionInTile = tilemap.WorldToCell(hitPosition);
            TileBase tb = tilemap.GetTile(hitPositionInTile);
            if(tb != null && tb.name == "button"){
                
                tileMapCollider.isTrigger = false;
                BoundsInt bounds = tilemap.cellBounds;
                List<Vector3Int> bridgeTiles = new List<Vector3Int>();
                for(int x = bounds.xMin; x < bounds.xMax; x++){
                    for(int y = bounds.yMin; y < bounds.yMax; y++){
                        if(tilemap.GetTile(new Vector3Int(x,y,0)) != null && tilemap.GetTile(new Vector3Int(x,y,0)).name != "button"){
                            bridgeTiles.Add(new Vector3Int(x,y,0));
                            tilemap.SetTile(new Vector3Int(x,y,0), bridgeOpen);
                        }
                    }   
                }
               tilemap.SetTile(hitPositionInTile, buttonPressed);
                StartCoroutine(ButtonCooldown(bridgeTiles, hitPositionInTile));
                    
            }
        }
        
    }

    IEnumerator ButtonCooldown(List<Vector3Int> bridgeTiles, Vector3Int hitPosition)
    {
        yield return new WaitForSeconds(5);
        tilemap.SetTile(hitPosition, buttonNotPressed);
        
        for (int i = 0; i < bridgeTiles.Count; i++)
        {
               tilemap.SetTile(bridgeTiles[i], bridgeClosed);
        }
        tileMapCollider.isTrigger = true;
    }
}
