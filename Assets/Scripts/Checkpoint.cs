﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Checkpoint : MonoBehaviour
{
    public Sprite checkpointUnused;
    public Sprite checkpointUsed;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            GetComponent<SpriteRenderer>().sprite = checkpointUsed;
            GameMaster.instance.lastCheckpointPos = gameObject.transform.position;
            GameMaster.instance.playerBoxesCount = PlayerController.instance.boxesInInventory 
                                                +  PlayerController.instance.getPlayerTiles().Count-1;
            
            BoundsInt bounds = GameObject.FindGameObjectWithTag("Pickups").GetComponent<Tilemap>().cellBounds;
            TileBase[] allTiles = GameObject.FindGameObjectWithTag("Pickups").GetComponent<Tilemap>().GetTilesBlock(GameObject.FindGameObjectWithTag("Pickups").GetComponent<Tilemap>().cellBounds);

            for (int x = 0; x < bounds.size.x; x++) {
                for (int y = 0; y < bounds.size.y; y++) {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null) {
                        Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    } else {
                    // Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                    }
                }
            }  
            GameMaster.instance.pickups = GameObject.FindGameObjectWithTag("Pickups").GetComponent<Tilemap>().GetTilesBlock(GameObject.FindGameObjectWithTag("Pickups").GetComponent<Tilemap>().cellBounds);

            GameMaster.instance.pickupsBounds = GameObject.FindGameObjectWithTag("Pickups").GetComponent<Tilemap>().cellBounds;
        }
    }
}
