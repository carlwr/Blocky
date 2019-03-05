using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

	private enum State{
		NORMAL,
		ADD_TILE
	}

	private State state;
	private Vector3Int lastAddedTile;
	private bool chooseNextBox;
	public bool jump;

	public float jumpForce;
	public Tilemap tm;
	public TileBase tb;
	public Tilemap nextTiles;
	public float speed;
	public Vector2 orientation;
	public Rigidbody2D rb2d; 
	// Use this for initialization
	void Start () {
		tm = GetComponent<Tilemap>();
		rb2d = GetComponent<Rigidbody2D> ();
		state = State.NORMAL;
		lastAddedTile = new Vector3Int(0,0,0);
	}
	
	void FixedUpdate()
	{
		rb2d.AddForce (orientation * speed);
	}

	// Update is called once per frame
	void Update () {
		
		switch(state){
			case State.NORMAL:
				normalUpdate();
				break;
			case State.ADD_TILE:
				addTileUpdate();
				break;
		}
	}

	void showTilesToChoose(){
		if(tm.GetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1,0)) == null){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), tb);
		}
		
		if(tm.GetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1,0)) == null){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), tb);
		}
		
		if(tm.GetTile(new Vector3Int(lastAddedTile.x +1, lastAddedTile.y,0)) == null){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y, 0), tb);
		}
		
		if(tm.GetTile(new Vector3Int(lastAddedTile.x -1, lastAddedTile.y,0)) == null){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x - 1, lastAddedTile.y, 0), tb);
		}
	}

	void removeTilesToShow(){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), null);
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), null);
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x+1, lastAddedTile.y, 0), null);
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x-1, lastAddedTile.y, 0), null);
			
	}

	void normalUpdate(){
		orientation = new Vector2(0,0);
		if (Input.GetKey("w") && jump)
        {
				orientation += new Vector2(0,1) * jumpForce;
				jump = false;
			
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
		showTilesToChoose();
		if(!Input.anyKey){
			chooseNextBox = true;
		}
		if(chooseNextBox){
			if (Input.GetKey("w") && 
			tm.GetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1,0)) == null)
			{
				removeTilesToShow();
				lastAddedTile += new Vector3Int(0,1,0);
				tm.SetTile(lastAddedTile,tb);
				state = State.NORMAL;
			}
			else if (Input.GetKey("d") && 
			tm.GetTile(new Vector3Int(lastAddedTile.x + 1, lastAddedTile.y ,0)) == null)
			{	
				removeTilesToShow();
				lastAddedTile += new Vector3Int(1,0,0);
				tm.SetTile(lastAddedTile ,tb);
				state = State.NORMAL;
			}
			else if (Input.GetKey("a") && 
			tm.GetTile(new Vector3Int(lastAddedTile.x -1, lastAddedTile.y,0)) == null)
			{	
				removeTilesToShow();
				lastAddedTile += new Vector3Int(-1,0,0);
				tm.SetTile(lastAddedTile ,tb);
				
				state = State.NORMAL;
			}
			else if (Input.GetKey("s") && 
			tm.GetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y-1,0)) == null)
			{	
				removeTilesToShow();
				lastAddedTile += new Vector3Int(0,-1,0);
				tm.SetTile(lastAddedTile ,tb);
				
				state = State.NORMAL;
			}
		}
		
	}

	void OnCollisionStay2D(Collision2D collisionInfo)
	{
		if(collisionInfo.collider.name == "Plattform"){
			
				
			jump = true;
			
		}
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		
		if(other.collider.tag == "Pickups"){
			orientation = new Vector3(0,0,0);
			state = State.ADD_TILE;
			chooseNextBox = false;
		}
		
	}
}
