using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb2d;
    public float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector3(speed,0,0);
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.tag == "Player"){
            PlayerController.instance.resetLevel();  
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Plattform"){
            print(other.name);
            rb2d.AddForce(new Vector3(0,jumpForce, 0));
        }

    }
}
