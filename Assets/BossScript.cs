using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;
using Hellmade.Sound;

public class BossScript : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb2d;
    public float jumpForce;
    public Tilemap fire;
    public TileBase fireTile;
    public bool startScene;
    public GameObject cam;
    public string text;

    public AudioClip talk;
    public AudioClip bossSound;
    private bool stuck;
    // Start is called before the first frame update
    void Start()
    {
        startScene = true;
        
        int backgroundMusicID = EazySoundManager.PlayMusic(bossSound, 0.35f, true, false, 1, 1);
        EazySoundManager.PlaySound(talk, 0.5f);
        rb2d = GetComponent<Rigidbody2D>();
        fire = GameObject.Find("Obstacles").GetComponent<Tilemap>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(startScene){
            
            cam.GetComponent<CinemachineVirtualCamera>().Priority = 11;
            UIController.instance.conversation.text = text;
            UIController.instance.setConversationPosition(new Vector3(transform.position.x+5, transform.position.y + 2, 0));
       
            StartCoroutine(conversation());
            
        }
        else{
        cam.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        if(stuck){
            rb2d.velocity = new Vector3(speed,jumpForce, 0);
        }
        else{
            rb2d.velocity = new Vector3(speed,0,0);
        }
        setOnFire();

        }
    }

    IEnumerator conversation(){
        
        yield return new WaitForSeconds(5);
        UIController.instance.conversation.text = "";
        startScene = false;

    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.tag == "Player"){
            PlayerController.instance.resetLevel();  
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.name == "Plattform"){
           // print(other.name);
           stuck = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.name == "Plattform"){
           // print(other.name);
           stuck = false;
        }
        
    }
    

    void setOnFire(){
        for(int i = (int)(transform.position.y)-20 ; i < (int)(transform.position.y) + 20; i ++){
            RaycastHit2D hit = Physics2D.Raycast(new Vector2((int)transform.position.x, i),
											 Vector2.zero);

            if(hit.collider != null && hit.collider.tag == "Plattform"){
                print(hit.collider.tag);
                fire.SetTile(new Vector3Int((int)transform.position.x, i, 0), fireTile);
            }
        }
    }
}
