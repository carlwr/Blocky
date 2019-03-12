using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelpTilesController : PlayerController
{
    // Start is called before the first frame update
   PlayerController playerController;

   override protected void Start(){
       
       base.Start();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        
   }

   override protected void Update(){
       state = playerController.state;
       wButtonDown = playerController.getWButton();
       sButtonDown = playerController.getSButton();
       aButtonDown = playerController.getAButton();
       dButtonDown = playerController.getDButton();
       base.Update();
   }

    override protected void normalUpdate(){
        tilemap.ClearAllTiles();
    }
    
    override protected void addTileUpdate(){
        showTilesToChoose();
        updateSelectedTileColor();
    }

    void showTilesToChoose(){
        lastAddedTile = playerController.getLastAddedTile();
		tilemap.ClearAllTiles();
             
		if(isEmptyTilePlace(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1,0), null)){
			tilemap.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), tb);
		}

		if(isEmptyTilePlace(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1,0), null)){
			tilemap.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), tb);
		}

		if(isEmptyTilePlace(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y,0), null)){
			tilemap.SetTile(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y, 0), tb);
		}

		if(isEmptyTilePlace(new Vector3Int(lastAddedTile.x-1, lastAddedTile.y,0), null)){
			tilemap.SetTile(new Vector3Int(lastAddedTile.x - 1, lastAddedTile.y, 0), tb);
		}
	}

    void updateSelectedTileColor(){
        
        tilemap.SetColor(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), new Color(wButtonDown,1-wButtonDown, 1-wButtonDown));
		
        tilemap.SetColor(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), new Color(sButtonDown,1-sButtonDown, 1-sButtonDown));
    
        tilemap.SetColor(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y, 0), new Color(dButtonDown,1-dButtonDown, 1-dButtonDown));
    
        tilemap.SetColor(new Vector3Int(lastAddedTile.x - 1, lastAddedTile.y, 0), new Color(aButtonDown,1-aButtonDown, 1-aButtonDown));
    
    }
}
