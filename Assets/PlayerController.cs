﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

	public enum State{
		NORMAL,
		ADD_TILE
	}

	public State state;
	public float maxJumpForce;
	private Vector3Int lastAddedTile;
	private bool chooseNextBox;
	public bool wallJump;

	public float jumpForce = 0;
	public Tilemap tm;

	public TileBase tb;
	public Tilemap nextTiles;
	public float speed;
	public Vector2 orientation;
	public Rigidbody2D rb2d;
	public bool canJump;

	
    // Use this for initialization
    void Start () {
		tm = GetComponent<Tilemap>();
		rb2d = GetComponent<Rigidbody2D> ();
		state = State.NORMAL;
		lastAddedTile = new Vector3Int(0,0,0);
		wallJump = false;
	}
	
	void FixedUpdate()
	{
        if(Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }


        rb2d.AddForce (orientation * speed);
        switch (state)
        {
            case State.NORMAL:
                normalUpdate();
                break;
            case State.ADD_TILE:
                addTileUpdate();
                break;
        }
    }

	// Update is called once per frame
	void Update () {
		
		
	}


	void showTilesToChoose(){
		
		nextTiles.ClearAllTiles();

		Vector3 tilemapWorld = tm.CellToWorld(lastAddedTile);
		tilemapWorld.x += 0.5f;
		tilemapWorld.y += 0.5f;

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(tilemapWorld.x, tilemapWorld.y + 1),
											 Vector2.zero);
		if(hit.collider == null){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), tb);
		}

		hit = Physics2D.Raycast(new Vector2(tilemapWorld.x, tilemapWorld.y - 1),
											 Vector2.down, 0.1f);
		if(hit.collider == null){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), tb);
		}

		hit = Physics2D.Raycast(new Vector2(tilemapWorld.x+1, tilemapWorld.y),
											 Vector2.right, 0.1f);
		if(hit.collider == null){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y, 0), tb);
		}

		hit = Physics2D.Raycast(new Vector2(tilemapWorld.x-1, tilemapWorld.y),
											 Vector2.left, 0.1f);
		if(hit.collider== null){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x - 1, lastAddedTile.y, 0), tb);
		}
	}

	void jump(){
		if ( canJump)
		{
			orientation += new Vector2(0, 1) * jumpForce;
			jumpForce = 0;
			wallJump = false;
			canJump = false;
		}
		else if(wallJump){
			orientation += new Vector2(0, 1) * 40;
			jumpForce = 0;
			wallJump = false;
			canJump = false;
		}
	}
	void normalUpdate(){

        //Make it not slow motion as soon as the block has been selected and state is normal again.
        if(Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

		orientation = new Vector2(0,0);
		if (Input.GetKey("w"))
        {
          jumpForce += 2f;
		  if(jumpForce > maxJumpForce){
			  jumpForce = maxJumpForce;
		  }
        }

		if(Input.GetKeyUp("w")){
			jump();
		}

		if (Input.GetKey("a"))
        {	
			orientation += new Vector2(-1,0);
        }
		if (Input.GetKey("d"))
        {
			orientation += new Vector2(1,0);
        }
	}
	void addTileUpdate(){

        //When we are adding a tile, make everything slow-motion!
        if(Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

		showTilesToChoose();
		if(!Input.anyKey){
			chooseNextBox = true;
		}
		if(chooseNextBox){
			
			Vector3 tilemapWorld = tm.CellToWorld(lastAddedTile);
			tilemapWorld.x += 0.5f;
			tilemapWorld.y += 0.5f;

			RaycastHit2D hit = Physics2D.Raycast(new Vector2(tilemapWorld.x, tilemapWorld.y + 1),
											 Vector2.zero);
			if (Input.GetKey("w") && 
			hit.collider == null)
			{
				
				nextTiles.ClearAllTiles();
				lastAddedTile += new Vector3Int(0,1,0);
				tm.SetTile(lastAddedTile,tb);
				state = State.NORMAL;
			}
			else if (Input.GetKey("d") && 
			Physics2D.Raycast(new Vector2(tilemapWorld.x+1, tilemapWorld.y),
											 Vector2.right, 0.1f).collider	 == null)
			{	
				nextTiles.ClearAllTiles();
				lastAddedTile += new Vector3Int(1,0,0);
				tm.SetTile(lastAddedTile ,tb);
				state = State.NORMAL;
			}
			else if (Input.GetKey("a") && 
			Physics2D.Raycast(new Vector2(tilemapWorld.x-1, tilemapWorld.y),
											 Vector2.left, 0.1f).collider == null)
			{	
				nextTiles.ClearAllTiles();
				lastAddedTile += new Vector3Int(-1,0,0);
				tm.SetTile(lastAddedTile ,tb);
				
				state = State.NORMAL;
			}
			else if (Input.GetKey("s") && 
			Physics2D.Raycast(new Vector2(tilemapWorld.x, tilemapWorld.y - 1),
											 Vector2.down, 0.1f).collider == null)
			{	
				nextTiles.ClearAllTiles();
				lastAddedTile += new Vector3Int(0,-1,0);
				tm.SetTile(lastAddedTile ,tb);
				
				state = State.NORMAL;
			}
			else{
				jumpForce = 0;
			}
		}
		
	}

	
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.contacts[0].normal == new Vector2(1,0)||other.contacts[0].normal == new Vector2(-1,0))
		{
			wallJump = true;
		}
	}

	void OnCollisionStay2D(Collision2D other)
	{
		if(other.contacts[0].normal == new Vector2(0,1)){
			canJump = true;
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
}

