using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cube
{
    public enum CubeType
    {
        HEAD_CUBE,
        NEUTRAL_CUBE
    }
    
    public Vector3Int position;
    public CubeType type;
    public TileBase tb;

    public Cube(Vector3Int pos, CubeType type, TileBase tileBase)
    {
        this.tb = tileBase;
        this.position = pos;
        this.type = type;
    }
}

public class PlayerController : MonoBehaviour {

	public enum State{
		NORMAL,
		ADD_TILE,
        WALL_SLIDE,
        STATIC
	}

    public static PlayerController instance;

	public State state;
	private Cube lastAddedTile;
    private List<Cube> playerTiles;
    
	
    
    private float aButtonDown = 0;
    private float wButtonDown = 0;
    private float dButtonDown = 0;
    private float sButtonDown = 0;
    public int boxesInInventory;

	public TileBase tb;


    public Cube getLastAddedTile(){
        return lastAddedTile;
    }

    public void deleteLastAddedTile()
    {
        
        int tileCount = playerTiles.Count;
        playerTiles.Remove(lastAddedTile);
        lastAddedTile = playerTiles[tileCount - 2];
    }

    public void setLastAddedTile(Cube newTile){
        lastAddedTile = newTile;
    }

    public List<Cube> getPlayerTiles(){
        return playerTiles;
    }
    public void AddToPlayerTiles(Cube newTile){
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

    public void resetLevel()
    {
        //Reset the level to beginning.
        Application.LoadLevel(Application.loadedLevel);
    }

    public void resetLevelToCheckpoint()
    {
        //Reset the level to the last checkpoint.
        GameMaster.instance.resetLevelToCheckpoint();

    }

    void Awake()
    {
      instance = this;   
    }

    // Use this for initialization
     void Start () {
		state = State.NORMAL;
		playerTiles = new List<Cube>();
        lastAddedTile = new Cube(new Vector3Int(0,0,0), Cube.CubeType.NEUTRAL_CUBE, tb);
        playerTiles.Add(lastAddedTile);
        boxesInInventory = GameMaster.instance.playerBoxesCount;
        
	}
	
	

    void Update()
    {
         if(Input.GetButtonDown("ResetButton"))
        {
            resetLevel();
        }
    }

    
}
