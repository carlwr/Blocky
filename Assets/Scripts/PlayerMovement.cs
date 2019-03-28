using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Hellmade.Sound;

public class PlayerMovement : TilemapController
{
    
	[Range(10,200)]public float speed;
	[Range(0,20000)]public float maxSpeed;
	[Range(10,150)]public float jumpForce;
    public TileBase bodyBase;
    public float maxWallSlideSpeed = 2;
    [Range(0,50)]public float jumpMultiplier = 10f;
    [Range(0,150)]public float interruptedJumpMultiplier = 80f;
    [Range(0,1)]public float wallSlideMultiplier = 0.5f;
    [Range(0,150)]public float runMultiplier = 80f;

    public GameObject LAT; //empty gameobject for the cinemachine to follow

    public AudioClip jumpSoundClip;
    public AudioClip lavaDeathSoundClip;
    public AudioClip blockOptainedSoundClip;
    public AudioClip backgroundMusic;


    private float addTileAddOn = 0.05f;
    private bool chooseNextBox;
    private Vector2 wallNormal;
	private Rigidbody2D rb2d;
	private bool canJump;
	private Vector2 orientation;

    [Range(0,1)]public float WallJumpVectorWhenTowards ;
    [Range(0,1)]public float WallJumpVectorWhenOtherDir;

    
    override protected void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D> ();
        int backgroundMusicID = EazySoundManager.PlayMusic(backgroundMusic, 0.35f, true, false, 1, 1);
        LAT = GameObject.Find("player follow");
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
            //Jump sound played
            int jumpSoundID = EazySoundManager.PlaySound(jumpSoundClip, 0.5f);
            //Jump type 3: Player is not moving in x direction.
            if (orientation.x == 0){
                rb2d.velocity += new Vector2(0, 1) * jumpForce;
            }
            //Jump type 1: Player is holding in button towards wall.
            else if (Mathf.Sign(orientation.x) == Mathf.Sign(-wallNormal.x))
                rb2d.velocity = new Vector2(WallJumpVectorWhenTowards * wallNormal.x, 1) * jumpForce;
            //Jump type 2: player is pressing button in direction opposite to wall.
            else if (Mathf.Sign(orientation.x) == Mathf.Sign(wallNormal.x))
                rb2d.velocity = new Vector2(WallJumpVectorWhenOtherDir * wallNormal.x, 1) * jumpForce;
            
            PlayerController.instance.state = PlayerController.State.NORMAL;
        }
        else if ( canJump)
		{
            //Jump sound played
            int jumpSoundID = EazySoundManager.PlaySound(jumpSoundClip, 0.5f);
            rb2d.velocity += new Vector2(0, 1) * jumpForce;
            PlayerController.instance.state = PlayerController.State.NORMAL;            

        }
		
	}

    IEnumerator removeDebugText(){
        yield return new WaitForSeconds(2);
        UIController.instance.jumpType.text = "";
    } 

	void normalUpdate(){
        
        
		orientation = new Vector2(0,0);
        //increase gravity for player on way down for feel
        if(rb2d.velocity.y < 0){
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.fixedDeltaTime;
        }else if(rb2d.velocity.y > 0 && !Input.GetButton("Jump")){
             rb2d.velocity += Vector2.up * Physics2D.gravity.y * (interruptedJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if(rb2d.velocity.x < 0 && !Input.GetButton("Left")){
            rb2d.velocity += Vector2.right * runMultiplier * Time.fixedDeltaTime;
        }else if(rb2d.velocity.x > 0 && !Input.GetButton("Right")){
             rb2d.velocity += Vector2.left * runMultiplier * Time.fixedDeltaTime;
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


            //Put audios to right speed
            Audio backgroundMusicAudio = EazySoundManager.GetAudio(backgroundMusic);
            backgroundMusicAudio.Pitch = 1f;
            //Audio jumpAudio = EazySoundManager.GetAudio(jumpSoundClip);
            //jumpAudio.Pitch = 1f;
            //Audio lavaDeathAudio = EazySoundManager.GetAudio(lavaDeathSoundClip);
            //lavaDeathAudio.Pitch = 1f;

            
}

        

        
        if (Input.GetButton("Left"))
        {
            if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE  && wallNormal == new Vector2(1,0)){
                rb2d.velocity *= new Vector2(0,wallSlideMultiplier);
            }

            orientation += new Vector2(-1, 0) * speed;
        }
        if (Input.GetButton("Right"))
        {
            if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE && wallNormal == new Vector2(-1,0)){
                rb2d.velocity *= new Vector2(0,wallSlideMultiplier);
            }
            orientation += new Vector2(1, 0) * speed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            
            jump();
            
        }
        
        if (Input.GetButton("Jump"))
        {
			 if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE)
            {
                //If we are sliding and holding in w, we do not want to stop sliding, this prevents that.
                //TODO: make a less brittle implementation of this.
                orientation.Set(0, 0);
            }
        }

        if(Input.GetButtonDown("BuildingButton")){
            PlayerController.instance.state = PlayerController.State.ADD_TILE;
        }

        
        Vector3 v = rb2d.velocity;
        if(v.x < -maxSpeed){
            v.x = -maxSpeed;
        }
        else if(v.x > maxSpeed){
            v.x = maxSpeed;
        }
        rb2d.velocity = v;
    

        
	}
	void addTileUpdate(){


        //When we are adding a tile, make everything slow-motion!
        if(Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Audio backgroundMusicAudio = EazySoundManager.GetAudio(backgroundMusic);
            backgroundMusicAudio.Pitch = 0.5f;
           // Audio jumpAudio = EazySoundManager.GetAudio(jumpSoundClip);
            //jumpAudio.Pitch = 0.5f;
            //Audio lavaDeathAudio = EazySoundManager.GetAudio(lavaDeathSoundClip);
            //lavaDeathAudio.Pitch = 0.5f;
        }

        if(Input.GetButtonUp("BuildingButton")){
                PlayerController.instance.state = PlayerController.State.NORMAL;
        }


		if(!Input.GetButton("Up") && !Input.GetButton("Left") && !Input.GetButton("Down") && !Input.GetButton("Right")){
			chooseNextBox = true;
		}
		if(chooseNextBox){
			Vector3Int lastAddedTile = PlayerController.instance.getLastAddedTile().position;
            float aButtonDown = PlayerController.instance.getAButton();
            float wButtonDown = PlayerController.instance.getWButton();
            float dButtonDown = PlayerController.instance.getDButton();
            float sButtonDown = PlayerController.instance.getSButton();
            if(Input.GetButtonUp("DeleteBlockButton") && PlayerController.instance.getPlayerTiles().Count > 1)
            {
                //If q is pressed, delete the block that was placed last.
                tilemap.SetTile(PlayerController.instance.getLastAddedTile().position, null);
                PlayerController.instance.deleteLastAddedTile();
                PlayerController.instance.boxesInInventory++;
                UIController.instance.increaseBoxesUnused();
                
               LAT.transform.position = gameObject.transform.position + PlayerController.instance.getLastAddedTile().position;
           
            }
            else if(PlayerController.instance.boxesInInventory > 0){
                if (Input.GetButton("Up")
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
                else if (Input.GetButton("Right") 
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
                else if (Input.GetButton("Left") 
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
                else if (Input.GetButton("Down") 
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

                if (Input.GetButtonUp("Up"))
                {
                    PlayerController.instance.setWButton(0);
                }
                if (Input.GetButtonUp("Right"))
                {
                    PlayerController.instance.setDButton(0);
                }
                if (Input.GetButtonUp("Down"))
                {
                    PlayerController.instance.setSButton(0);
                }
                if (Input.GetButtonUp("Left"))
                {
                    PlayerController.instance.setAButton(0);
                }

            }
            
        }

		
	}

    bool canDeleteTile(Vector3Int tilePos){
        if( PlayerController.instance.getPlayerTiles().Count > 1){
            Vector3Int secondToLastPlayerPos = PlayerController.instance.getPlayerTiles()[PlayerController.instance.getPlayerTiles().Count-2].position;
            if(tilePos == secondToLastPlayerPos){
                return true;
            }
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
        if( PlayerController.instance.state != PlayerController.State.ADD_TILE){

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
	}
    

   
	
    void OnCollisionExit2D(Collision2D other)
    {
        
        if( PlayerController.instance.state != PlayerController.State.ADD_TILE){

            if(PlayerController.instance.state == PlayerController.State.WALL_SLIDE){
                if(!isAnyTileOnWall()){
                    PlayerController.instance.state = PlayerController.State.NORMAL;
                }
            }
        }        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pickups")
        {
            Debug.Log("Pickup Gained!");
            //blockOptained sound played
            int lavaDeathSoundID = EazySoundManager.PlaySound(blockOptainedSoundClip, 0.5f);
            PlayerController.instance.boxesInInventory ++;
            UIController.instance.increaseBoxesUnused();
            orientation = new Vector3(0, 0, 0);
            //PlayerController.instance.state = PlayerController.State.ADD_TILE;
            chooseNextBox = false;
            
        }

        if (other.tag == "Obstacle")
        {
            Debug.Log("Obstacle Touched");
            //lava sound played
            int lavaDeathSoundID = EazySoundManager.PlaySound(lavaDeathSoundClip, 0.5f);
            PlayerController.instance.resetLevel();
        }
    }


	

    bool isAnyTileGrounded(){
        foreach (Cube tile in PlayerController.instance.getPlayerTiles())
        {
            if(!isEmptyTilePlace(new Vector3Int(tile.position.x, tile.position.y -1 , 0), gameObject.tag)){
                return true;
            }
        }
        return false;
    }

    bool isAnyTileOnWall(){
        foreach (Cube tile in PlayerController.instance.getPlayerTiles())
        {
            if(!isEmptyTilePlace(new Vector3Int(tile.position.x-1, tile.position.y , 0), gameObject.tag)){
                return true;
            }
            
            else if(!isEmptyTilePlace(new Vector3Int(tile.position.x+1, tile.position.y , 0), gameObject.tag)){
                return true;
            }
        }
        return false;
    }

    private void newTile(Vector3Int newTileOffset){
        Vector3Int newTilePos = PlayerController.instance.getLastAddedTile().position + newTileOffset;
        Cube cubeToAdd = new Cube(newTilePos, Cube.CubeType.NEUTRAL_CUBE, bodyBase);
        PlayerController.instance.setLastAddedTile(cubeToAdd);
        tilemap.SetTile(newTilePos, bodyBase);
        
        LAT.transform.position = gameObject.transform.position + newTilePos;
        
        PlayerController.instance.AddToPlayerTiles(cubeToAdd);
    }

}
