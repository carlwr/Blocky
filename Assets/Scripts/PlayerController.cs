using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

	public enum State{
		NORMAL,
		ADD_TILE,
        WALL_SLIDE
	}

    public static PlayerController instance;

	public State state;
	private Vector3Int lastAddedTile;
    private List<Vector3Int> playerTiles;
	
    
    private float aButtonDown = 0;
    private float wButtonDown = 0;
    private float dButtonDown = 0;
    private float sButtonDown = 0;
    public int boxesInInventory;

	public TileBase tb;


    public Vector3Int getLastAddedTile(){
        return lastAddedTile;
    }

    public void deleteLastAddedTile()
    {
        int tileCount = playerTiles.Count;
        playerTiles.Remove(lastAddedTile);
        lastAddedTile = playerTiles[tileCount - 2];
    }

    public void setLastAddedTile(Vector3Int newTilePos){
        lastAddedTile = newTilePos;
    }

    public List<Vector3Int> getPlayerTiles(){
        return playerTiles;
    }
    public void AddToPlayerTiles(Vector3Int newTile){
        playerTiles.Add(newTile);
    }


    public void setAButton(float newAButtonDown){
        aButtonDown = newAButtonDown;
    }
    public void setSButton(float newSButtonDown){
        sButtonDown = newSButtonDown;
    }
    
    public void setDButton(float newDButtonDown){
        dButtonDown = newDButtonDown;
    }
    
    public void setWButton(float newWButtonDown){
        wButtonDown = newWButtonDown;
    }
    
    
    
     public float getAButton(){
        return aButtonDown;
    }
    public float getWButton(){
        return wButtonDown;
    }
    public float getSButton(){
        return sButtonDown;
    }
    public float getDButton(){
        return dButtonDown;
    }

    void Awake()
    {
      instance = this;   
    }

    // Use this for initialization
     void Start () {
		state = State.NORMAL;
		playerTiles = new List<Vector3Int>();
        lastAddedTile = new Vector3Int(0,0,0);
        playerTiles.Add(lastAddedTile);
        
	}
	
	

    void Update()
    {
          if(Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

    }

    
}
