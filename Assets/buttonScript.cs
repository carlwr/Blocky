using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Hellmade.Sound;

public class buttonScript : MonoBehaviour
{
    public GameObject tilemapObject;
    Tilemap tilemap;
    public Sprite buttonPressed;
    public Sprite buttonNotPressed;
    public TileBase bridgeOpen;
    public TileBase bridgeClosed;
    public int cooldown;
    TilemapCollider2D tileMapCollider;
    public AudioClip buttonSound;
    
    void Start()
    {
        tilemap = tilemapObject.GetComponent<Tilemap>();
        tileMapCollider = tilemapObject.GetComponent<TilemapCollider2D>();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {   
            int backgroundMusicID = EazySoundManager.PlayMusic(buttonSound, 0.35f, false, false, 1, 1);

            GetComponent<SpriteRenderer>().sprite = buttonPressed;

            tilemapObject.tag = "Plattform";
            tileMapCollider.isTrigger = false;
            BoundsInt bounds = tilemap.cellBounds;
            List<Vector3Int> bridgeTiles = new List<Vector3Int>();
            for(int x = bounds.xMin; x < bounds.xMax; x++){
                for(int y = bounds.yMin; y < bounds.yMax; y++){
                    if(tilemap.GetTile(new Vector3Int(x,y,0)) != null){
                        bridgeTiles.Add(new Vector3Int(x,y,0));
                        tilemap.SetTile(new Vector3Int(x,y,0), bridgeOpen);
                    }
                }   
            }
            StartCoroutine(ButtonCooldown(bridgeTiles));
                
        }
        
    }

    IEnumerator ButtonCooldown(List<Vector3Int> bridgeTiles)
    {
        yield return new WaitForSeconds(cooldown);
        
        GetComponent<SpriteRenderer>().sprite = buttonNotPressed;
        for (int i = 0; i < bridgeTiles.Count; i++)
        {
               tilemap.SetTile(bridgeTiles[i], bridgeClosed);
        }
        tileMapCollider.isTrigger = true;
        
        tilemapObject.tag = "Untagged";
    }
}
