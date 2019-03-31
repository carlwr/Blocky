using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 orientation;
    public bool upAndDown = false;
    [Range(0, 50)] public float speed = 3;
    [Range(0, 50)] public float rangeInSeconds = 3;
    private float time;
    private bool obstacleTurnsAround;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        time = Time.time;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2d.velocity = orientation;
        orientation = new Vector2(0, 0);
    }

    void Update()
    {
        if (Time.time >= time + rangeInSeconds)
        {
            obstacleTurnsAround = !obstacleTurnsAround;
            time = Time.time;
        }
        if (upAndDown)
        {
            if (obstacleTurnsAround)
            { 
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y+180, transform.rotation.z,0);
               
                orientation += new Vector2(0, -1) * speed;
            }
            else
            {
                
                transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z,0);
                orientation += new Vector2(0, 1) * speed;
            }
            
        }
        else
        {            
            
            if (obstacleTurnsAround)
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y+180, transform.rotation.z,0);
                orientation += new Vector2(1, 0) * speed;
            }
            else
            {
                
                transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z,0);
                orientation += new Vector2(-1, 0) * speed;
            }
        }
         
    }
}
