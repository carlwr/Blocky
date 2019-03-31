using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossScript : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb2d;
    public float jumpForce;
    public Tilemap fire;
    public TileBase fireTile;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector3(speed,0,0);
        setOnFire();
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.tag == "Player"){
            PlayerController.instance.resetLevel();  
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Plattform"){
           // print(other.name);
            rb2d.velocity = new Vector3(0,rb2d.velocity.y,0);
            rb2d.AddForce(new Vector3(0,jumpForce, 0));
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
