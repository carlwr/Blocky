using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : TilemapController {

	public enum State{
		NORMAL,
		ADD_TILE,
        WALL_SLIDE
	}

	public State state;
	protected Vector3Int lastAddedTile;
    protected List<Vector3Int> playerTiles;
	protected bool chooseNextBox;
    
    protected Vector2 wallNormal;
    protected float aButtonDown = 0;
    protected float wButtonDown = 0;
    protected float dButtonDown = 0;
    protected float sButtonDown = 0;

	public TileBase tb;


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
    public Vector3Int getLastAddedTile(){
        return lastAddedTile;
    }

	
    // Use this for initialization
    override protected void Start () {
		base.Start();
		state = State.NORMAL;
		playerTiles = new List<Vector3Int>();
        lastAddedTile = new Vector3Int(0,0,0);
        playerTiles.Add(lastAddedTile);
        
	}
	
	

    virtual protected void Update()
    {
          if(Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }


        switch (state)
        {
            case State.WALL_SLIDE:
                
            case State.NORMAL:
                normalUpdate();
                break;
            case State.ADD_TILE:
                addTileUpdate();
                break;
        }
    }

    virtual protected void normalUpdate(){

    }

    virtual protected void addTileUpdate(){

    }
}

