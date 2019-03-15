using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PlayerMovement : TilemapController
{
    
	[Range(10,200)]public float speed;
	[Range(10,150)]public float jumpForce;
    public TileBase tileBase;
    public float maxWallSlideSpeed = 2;
    [Range(0,50)]public float jumpMultiplier = 10f;
    [Range(0,150)]public float interruptedJumpMultiplier = 80f;
    [Range(0,1)]public float wallSlideMultiplier = 0.5f;


    private float addTileAddOn = 0.05f;
    private bool chooseNextBox;
    private Vector2 wallNormal;
	private Rigidbody2D rb2d;
	private bool canJump;
	private Vector2 orientation;

    
    override protected void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D> ();
        
    }

    void Update()
    {
        switch (PlayerController.instance.state)
        {
            case PlayerController.State.WALL_SLIDE:
                
            case PlayerController.State.NORMAL:
                normalUpdate();
                break;
            case PlayerController.State.ADD_TILE:
                addTileUpdate();
                break;
        }
    }

    void FixedUpdate()
	{
        rb2d.AddForce (orientation);
		orientation = new Vector2(0,0);
    }

    void jump(){
        if (PlayerController.instance.state == PlayerController.State.WALL_SLIDE)
        {
            
            //Jump type 3: Player is not moving in x direction.
            if (orientation.x == 0)
                rb2d.velocity += new Vector2(0, 1) * jumpForce;
            //Jump type 1: Player is holding in button towards wall.
            else if (Mathf.Sign(orientation.x) == Mathf.Sign(-wallNormal.x))
                rb2d.velocity = new Vector2(0.3f * wallNormal.x, 1) * jumpForce;
            //Jump type 2: player is pressing button in direction opposite to wall.
            else if (Mathf.Sign(orientation.x) == Mathf.Sign(wallNormal.x))
                rb2d.velocity = new Vector2(0.8f * wallNormal.x, 1) * jumpForce;
            
            PlayerController.instance.state = PlayerController.State.NORMAL;
        }
        else if ( canJump)
		{
			rb2d.velocity += new Vector2(0, 1) * jumpForce;
            PlayerController.instance.state = PlayerController.State.NORMAL;
		}
		
	}

	void interuptJump(){
		rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
	}
	void normalUpdate(){
        
        
		orientation = new Vector2(0,0);
        //increase gravity for player on way down for feel
        if(rb2d.velocity.y < 0){
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.fixedDeltaTime;
        }else if(rb2d.velocity.y > 0 && !Input.GetKey("w")){
             rb2d.velocity += Vector2.up * Physics2D.gravity.y * (interruptedJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if(isAnyTileGrounded()){
         
            if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE)
            {
                PlayerController.instance.state = PlayerController.State.NORMAL;
            }
            canJump = true;
        }
        else{
            canJump = false;
        }
        //Make it not slow motion as soon as the block has been selected and state is normal again.
        if(Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        

        
        if (Input.GetKey("a"))
        {
            if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE  && wallNormal == new Vector2(1,0)){
                rb2d.velocity *= new Vector2(0,wallSlideMultiplier);
            }
            orientation += new Vector2(-1, 0) * speed;
        }
        if (Input.GetKey("d"))
        {
            if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE && wallNormal == new Vector2(-1,0)){
                rb2d.velocity *= new Vector2(0,wallSlideMultiplier);
            }
            orientation += new Vector2(1, 0) * speed;
        }

        if (Input.GetKeyDown("w"))
        {
            jump();
        }
		if (Input.GetKey("w"))
        {
			 if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE)
            {
                //If we are sliding and holding in w, we do not want to stop sliding, this prevents that.
                //TODO: make a less brittle implementation of this.
                orientation.Set(0, 0);
            }
        }

        if(Input.GetKeyUp("e")){
            PlayerController.instance.state = PlayerController.State.ADD_TILE;
        }
        
	}
	void addTileUpdate(){

        //When we are adding a tile, make everything slow-motion!
        if(Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

		if(!Input.anyKey){
			chooseNextBox = true;
		}
		if(chooseNextBox){
			Vector3Int lastAddedTile = PlayerController.instance.getLastAddedTile();
            float aButtonDown = PlayerController.instance.getAButton();
            float wButtonDown = PlayerController.instance.getWButton();
            float dButtonDown = PlayerController.instance.getDButton();
            float sButtonDown = PlayerController.instance.getSButton();
            if(PlayerController.instance.boxesInInventory > 0){
                if (Input.GetKey("w") 
                && isEmptyTilePlace(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1,0), null) 
                && aButtonDown <= 0 && dButtonDown <= 0 && sButtonDown <= 0)
                {
                    PlayerController.instance.setWButton(wButtonDown + addTileAddOn);
                    if (PlayerController.instance.getWButton() > 1)
                    {
                        newTile(new Vector3Int(0, 1, 0));
                        PlayerController.instance.boxesInInventory --;
                        UIController.instance.decreaseBoxesUnused();
                        //PlayerController.instance.state = PlayerController.State.NORMAL;
                        PlayerController.instance.setWButton(0);
                        
                    }
                    
                }
                else if (Input.GetKey("d") 
                && isEmptyTilePlace(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y,0), null)
                && aButtonDown <= 0 && wButtonDown <= 0 && sButtonDown <= 0)
                {
                    PlayerController.instance.setDButton(dButtonDown + addTileAddOn);
                    if (PlayerController.instance.getDButton() > 1)
                    {
                        newTile(new Vector3Int(1, 0, 0));
                        PlayerController.instance.boxesInInventory --;
                        UIController.instance.decreaseBoxesUnused();
                        //PlayerController.instance.state = PlayerController.State.NORMAL;
                        PlayerController.instance.setDButton(0);
                    }
                        
                }
                else if (Input.GetKey("a") 
                && isEmptyTilePlace(new Vector3Int(lastAddedTile.x -1, lastAddedTile.y,0), null) 
                && wButtonDown <= 0 && dButtonDown <= 0 && sButtonDown <= 0)
                {
                    PlayerController.instance.setAButton(aButtonDown + addTileAddOn);
                    if (PlayerController.instance.getAButton() > 1)
                    {
                        newTile(new Vector3Int(-1, 0, 0));
                        PlayerController.instance.boxesInInventory --;
                        UIController.instance.decreaseBoxesUnused();
                        //PlayerController.instance.state = PlayerController.State.NORMAL;
                        PlayerController.instance.setAButton(0);
                    }
                        
                }
                else if (Input.GetKey("s") 
                && isEmptyTilePlace(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1,0), null) 
                && aButtonDown <= 0 && dButtonDown <= 0 && wButtonDown <= 0)
                {
                    PlayerController.instance.setSButton(sButtonDown + addTileAddOn);
                    if (PlayerController.instance.getSButton() > 1)
                    {
                        newTile(new Vector3Int(0, -1, 0));
                        PlayerController.instance.boxesInInventory --;
                        UIController.instance.decreaseBoxesUnused();
                        //PlayerController.instance.state = PlayerController.State.NORMAL;
                        PlayerController.instance.setSButton(0);
                    }
                        
                }

                if (Input.GetKeyUp("w"))
                {
                    PlayerController.instance.setWButton(0);
                }
                if (Input.GetKeyUp("d"))
                {
                    PlayerController.instance.setDButton(0);
                }
                if (Input.GetKeyUp("s"))
                {
                    PlayerController.instance.setSButton(0);
                }
                if (Input.GetKeyUp("a"))
                {
                    PlayerController.instance.setAButton(0);
                }

            }
            if(Input.GetKeyUp("e")){
                PlayerController.instance.state = PlayerController.State.NORMAL;
            }

        }

		
	}

    bool canDeleteTile(Vector3Int tilePos){
        Vector3Int secondToLastPlayerPos = PlayerController.instance.getPlayerTiles()[PlayerController.instance.getPlayerTiles().Count-2];
        if(tilePos == secondToLastPlayerPos){
            return true;
        }
        return false;
    }

    void wallSlideUpdate()
    {
        

        if(rb2d.velocity.y < -maxWallSlideSpeed)
        {
            
            rb2d.velocity.Set(rb2d.velocity.x, -maxWallSlideSpeed);
        }
        //Check for when no longer touching wall
    }

	
	void OnCollisionEnter2D(Collision2D other)
	{
        

		if(other.contacts[0].normal == new Vector2(1, 0))
		{
            wallNormal = new Vector2(1,0);
            PlayerController.instance.state = PlayerController.State.WALL_SLIDE;
		}
        if(other.contacts[0].normal == new Vector2(-1, 0))
        {
            wallNormal = new Vector2(-1, 0);
            PlayerController.instance.state = PlayerController.State.WALL_SLIDE;
        }
	}
    

   
	
    void OnCollisionExit2D(Collision2D other)
    {
        if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE){
            if(!isAnyTileOnWall()){
                PlayerController.instance.state = PlayerController.State.NORMAL;
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pickups")
        {
            PlayerController.instance.boxesInInventory ++;
            UIController.instance.increaseBoxesUnused();
            orientation = new Vector3(0, 0, 0);
            //PlayerController.instance.state = PlayerController.State.ADD_TILE;
            chooseNextBox = false;
        }
    }


	

    bool isAnyTileGrounded(){
        foreach (Vector3Int tile in PlayerController.instance.getPlayerTiles())
        {
            if(!isEmptyTilePlace(new Vector3Int(tile.x, tile.y -1 , 0), gameObject.tag)){
                return true;
            }
        }
        return false;
    }

    bool isAnyTileOnWall(){
        foreach (Vector3Int tile in PlayerController.instance.getPlayerTiles())
        {
            if(!isEmptyTilePlace(new Vector3Int(tile.x-1, tile.y , 0), gameObject.tag)){
                return true;
            }
            
            else if(!isEmptyTilePlace(new Vector3Int(tile.x+1, tile.y , 0), gameObject.tag)){
                return true;
            }
        }
        return false;
    }

    private void newTile(Vector3Int newTile){
        Vector3Int tileToAdd = PlayerController.instance.getLastAddedTile() + newTile;
        PlayerController.instance.setLastAddedTile(tileToAdd);
        tilemap.SetTile(tileToAdd, tileBase);
        PlayerController.instance.AddToPlayerTiles(tileToAdd);
    }
}
