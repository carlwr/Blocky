using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerController
{
    
	[Range(10,200)]public float speed;
	[Range(10,150)]public float jumpForce;
    public float maxWallSlideSpeed = 2;
    [Range(0,25)]public float jumpMultiplier = 10f;
    [Range(0,150)]public float interruptedJumpMultiplier = 80f;
    [Range(0,1)]public float wallSlideMultiplier = 0.5f;

    
	private Rigidbody2D rb2d;
	private bool canJump;
	private Vector2 orientation;

    
    override protected void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D> ();
        
    }

    void FixedUpdate()
	{
        rb2d.AddForce (orientation);
		orientation = new Vector2(0,0);
    }

    void jump(){
        if (state == State.WALL_SLIDE)
        {
            //Jump type 1: Player is holding in button towards wall.
            if (orientation.x == -wallNormal.x)
                rb2d.velocity = new Vector2(0.3f * wallNormal.x, 1) * jumpForce;
            //Jump type 2: player is pressing button in direction opposite to wall.
            else if (orientation.x == wallNormal.x)
                rb2d.velocity = new Vector2(0.8f * wallNormal.x, 1) * jumpForce;
            //Jump type 3: Player is not moving in x direction.
            else
                rb2d.velocity += new Vector2(0, 1) * jumpForce;
            state = State.NORMAL;
        }
        else if ( canJump)
		{
             
			rb2d.velocity += new Vector2(0, 1) * jumpForce;
            state = State.NORMAL;
		}
		
	}

	void interuptJump(){
		rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
	}
	override protected void normalUpdate(){
        
		orientation = new Vector2(0,0);
        //increase gravity for player on way down for feel
        if(rb2d.velocity.y < 0){
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.fixedDeltaTime;
        }else if(rb2d.velocity.y > 0 && !Input.GetKey("w")){
             rb2d.velocity += Vector2.up * Physics2D.gravity.y * (interruptedJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if(isAnyTileGrounded()){
         
            if(state == State.WALL_SLIDE)
            {
                state = State.NORMAL;
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
            if(state == State.WALL_SLIDE  && wallNormal == new Vector2(1,0)){
                rb2d.velocity *= new Vector2(0,wallSlideMultiplier);
            }
            orientation += new Vector2(-1, 0) * speed;
        }
        if (Input.GetKey("d"))
        {
            if(state == State.WALL_SLIDE && wallNormal == new Vector2(-1,0)){
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
			 if(state == State.WALL_SLIDE)
            {
                //If we are sliding and holding in w, we do not want to stop sliding, this prevents that.
                //TODO: make a less brittle implementation of this.
                orientation.Set(0, 0);
            }
        }
        
	}
	override protected void addTileUpdate(){

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
			
            if (Input.GetKey("w") 
            && isEmptyTilePlace(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1,0), null) 
            && aButtonDown <= 0 && dButtonDown <= 0 && sButtonDown <= 0)
			{
                wButtonDown += 0.015f;
                if (wButtonDown > 1)
                {
                    lastAddedTile += new Vector3Int(0, 1, 0);
                    tilemap.SetTile(lastAddedTile, tb);
                    playerTiles.Add(lastAddedTile);
                    state = State.NORMAL;
                    wButtonDown = 0;
                    
                }
                
			}
			else if (Input.GetKey("d") 
            && isEmptyTilePlace(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y,0), null)
            && aButtonDown <= 0 && wButtonDown <= 0 && sButtonDown <= 0)
			{
                dButtonDown += 0.015f;
                if (dButtonDown > 1)
                {
                    lastAddedTile += new Vector3Int(1, 0, 0);
                    tilemap.SetTile(lastAddedTile, tb);
                    playerTiles.Add(lastAddedTile);
                    state = State.NORMAL;
                    dButtonDown = 0;
                }
                    
			}
			else if (Input.GetKey("a") 
            && isEmptyTilePlace(new Vector3Int(lastAddedTile.x -1, lastAddedTile.y,0), null) 
            && wButtonDown <= 0 && dButtonDown <= 0 && sButtonDown <= 0)
			{
                aButtonDown += 0.015f;
                if (aButtonDown > 1)
                {
                    lastAddedTile += new Vector3Int(-1, 0, 0);
                    tilemap.SetTile(lastAddedTile, tb);
                    playerTiles.Add(lastAddedTile);
                    state = State.NORMAL;
                    aButtonDown = 0;
                }
                    
			}
			else if (Input.GetKey("s") 
            && isEmptyTilePlace(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1,0), null) 
            && aButtonDown <= 0 && dButtonDown <= 0 && wButtonDown <= 0)
			{
                sButtonDown += 0.015f;
                if (sButtonDown > 1)
                {
                    lastAddedTile += new Vector3Int(0, -1, 0);
                    tilemap.SetTile(lastAddedTile, tb);
                    playerTiles.Add(lastAddedTile);
                    state = State.NORMAL;
                    sButtonDown = 0;
                }
                    
			}

            if (Input.GetKeyUp("w"))
            {
                wButtonDown = 0;
            }
            if (Input.GetKeyUp("d"))
            {
                dButtonDown = 0;
            }
            if (Input.GetKeyUp("s"))
            {
                sButtonDown = 0;
            }
            if (Input.GetKeyUp("a"))
            {
                aButtonDown = 0;
            }

        }
		
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
            state = State.WALL_SLIDE;
		}
        if(other.contacts[0].normal == new Vector2(-1, 0))
        {
            wallNormal = new Vector2(-1, 0);
            state = State.WALL_SLIDE;
        }
	}
    

   
	
    void OnCollisionExit2D(Collision2D other)
    {
        if(state == State.WALL_SLIDE){
            if(!isAnyTileOnWall()){
                state = State.NORMAL;
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pickups")
        {
            orientation = new Vector3(0, 0, 0);
            state = State.ADD_TILE;
            chooseNextBox = false;
        }
    }


	

    bool isAnyTileGrounded(){
        foreach (Vector3Int tile in playerTiles)
        {
            if(!isEmptyTilePlace(new Vector3Int(tile.x, tile.y -1 , 0), gameObject.tag)){
                return true;
            }
        }
        return false;
    }

    bool isAnyTileOnWall(){
        foreach (Vector3Int tile in playerTiles)
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
}
