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

	public Tilemap tm;
	public TileBase tb;
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

	void clearBeforeSwitch(){
		
		orientation = new Vector2(0,0);
	}

	void normalUpdate(){
		orientation = new Vector2(0,0);
		if (Input.GetKey("w"))
        {
			orientation += new Vector2(0,1);
        }
		if (Input.GetKey("s"))
        {	
			orientation += new Vector2(0,-1);
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
		if (Input.GetKey("w"))
        {
			lastAddedTile += new Vector3Int(0,1,0);
			tm.SetTile(lastAddedTile,tb);
			state = State.NORMAL;
        }
		if (Input.GetKey("d"))
        {	
			lastAddedTile += new Vector3Int(1,0,0);
			tm.SetTile(lastAddedTile ,tb);
			state = State.NORMAL;
        }
		if (Input.GetKey("a"))
        {	
			lastAddedTile += new Vector3Int(-1,0,0);
			tm.SetTile(lastAddedTile ,tb);
			
			state = State.NORMAL;
        }
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.collider.tag == "Pickups"){
			state = State.ADD_TILE;
		}
		
	}
}
