using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

	public enum State{
		NORMAL,
		ADD_TILE
	}

	public State state;
	private Vector3Int lastAddedTile;
	private bool chooseNextBox;
	private bool jump;

	public float jumpForce;
	public Tilemap tm;

	public TileBase tb;
	public Tilemap nextTiles;
	public float speed;
	public Vector2 orientation;
	public Rigidbody2D rb2d;
    private float canJump = 0f;
    // Use this for initialization
    void Start () {
		tm = GetComponent<Tilemap>();
		rb2d = GetComponent<Rigidbody2D> ();
		state = State.NORMAL;
		lastAddedTile = new Vector3Int(0,0,0);
		jump = false;
	}
	
	void FixedUpdate()
	{

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
		
		nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), null);
		nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), null);
		nextTiles.SetTile(new Vector3Int(lastAddedTile.x+1, lastAddedTile.y, 0), null);
		nextTiles.SetTile(new Vector3Int(lastAddedTile.x-1, lastAddedTile.y , 0), null);

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

	void removeTilesToShow(){
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y + 1, 0), null);
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x, lastAddedTile.y - 1, 0), null);
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x+1, lastAddedTile.y, 0), null);
			nextTiles.SetTile(new Vector3Int(lastAddedTile.x-1, lastAddedTile.y, 0), null);
			
	}

	void normalUpdate(){
		orientation = new Vector2(0,0);
		if (Input.GetKeyDown("w"))
        {
            if (Time.time > canJump && rb2d.velocity.y >= 0 || jump)
            {
                orientation += new Vector2(0, 1) * jumpForce;
                canJump = Time.time + 0.4f;
				jump = false;
            }
            
			


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
			
			Vector3 tilemapWorld = tm.CellToWorld(lastAddedTile);
			tilemapWorld.x += 0.5f;
			tilemapWorld.y += 0.5f;

			RaycastHit2D hit = Physics2D.Raycast(new Vector2(tilemapWorld.x, tilemapWorld.y + 1),
											 Vector2.zero);
			if (Input.GetKey("w") && 
			hit.collider == null)
			{
				removeTilesToShow();
				lastAddedTile += new Vector3Int(0,1,0);
				tm.SetTile(lastAddedTile,tb);
				state = State.NORMAL;
			}
			else if (Input.GetKey("d") && 
			Physics2D.Raycast(new Vector2(tilemapWorld.x+1, tilemapWorld.y),
											 Vector2.right, 0.1f).collider	 == null)
			{	
				removeTilesToShow();
				lastAddedTile += new Vector3Int(1,0,0);
				tm.SetTile(lastAddedTile ,tb);
				state = State.NORMAL;
			}
			else if (Input.GetKey("a") && 
			Physics2D.Raycast(new Vector2(tilemapWorld.x-1, tilemapWorld.y),
											 Vector2.left, 0.1f).collider == null)
			{	
				removeTilesToShow();
				lastAddedTile += new Vector3Int(-1,0,0);
				tm.SetTile(lastAddedTile ,tb);
				
				state = State.NORMAL;
			}
			else if (Input.GetKey("s") && 
			Physics2D.Raycast(new Vector2(tilemapWorld.x, tilemapWorld.y - 1),
											 Vector2.down, 0.1f).collider == null)
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
		}
			
		
	}

	void OnCollisionEnter2D(Collision2D other)
	{


        if(other.contacts[0].normal == new Vector2(1,0)||other.contacts[0].normal == new Vector2(-1,0))
		{
			jump = true;
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

