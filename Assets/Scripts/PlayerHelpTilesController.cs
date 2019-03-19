using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHelpTilesController : TilemapController
{
    // Start is called before the first frame update

   public TileBase tileBase;

   override protected void Start(){
       
       base.Start();
        
   }

    void Update(){
        
       if(PlayerController.instance.state == PlayerController.State.ADD_TILE){
           showTilesToChoose();
            updateSelectedTileColor();
       }
       else{
           tilemap.ClearAllTiles();
       }
   }

   

    void showTilesToChoose(){
		tilemap.ClearAllTiles();
        
        Vector3Int lastAddedTile = PlayerController.instance.getLastAddedTile().position;
             
		if(isEmptyTilePlace(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1,0), null)){
			tilemap.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), tileBase);
		}

		if(isEmptyTilePlace(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1,0), null)){
			tilemap.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), tileBase);
		}

		if(isEmptyTilePlace(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y,0), null)){
			tilemap.SetTile(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y, 0), tileBase);
		}

		if(isEmptyTilePlace(new Vector3Int(lastAddedTile.x-1, lastAddedTile.y,0), null)){
			tilemap.SetTile(new Vector3Int(lastAddedTile.x - 1, lastAddedTile.y, 0), tileBase);
		}
	}

    void updateSelectedTileColor(){
        
        Vector3Int lastAddedTile = PlayerController.instance.getLastAddedTile().position;
        if(PlayerController.instance.boxesInInventory < 1){
            
            tilemap.SetColor(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), new Color(0.5f,0.5f, 0.5f, 0.3f));
            
            tilemap.SetColor(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), new Color(0.5f,0.5f, 0.5f, 0.3f));
        
            tilemap.SetColor(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y, 0), new Color(0.5f,0.5f, 0.5f, 0.3f));
        
            tilemap.SetColor(new Vector3Int(lastAddedTile.x - 1, lastAddedTile.y, 0), new Color(0.5f,0.5f, 0.5f, 0.3f));
        
            }
        else{
            float wButtonDown = PlayerController.instance.getWButton();
            float sButtonDown = PlayerController.instance.getSButton();
            float aButtonDown = PlayerController.instance.getAButton();
            float dButtonDown = PlayerController.instance.getDButton();
            
                
            tilemap.SetColor(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), new Color(wButtonDown,1-wButtonDown, 1-wButtonDown));
            
            tilemap.SetColor(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), new Color(sButtonDown,1-sButtonDown, 1-sButtonDown));
        
            tilemap.SetColor(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y, 0), new Color(dButtonDown,1-dButtonDown, 1-dButtonDown));
        
            tilemap.SetColor(new Vector3Int(lastAddedTile.x - 1, lastAddedTile.y, 0), new Color(aButtonDown,1-aButtonDown, 1-aButtonDown));
        
            }
        }
    

}
