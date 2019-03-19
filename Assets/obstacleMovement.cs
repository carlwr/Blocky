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
    // Start is called before the first frame update
    void Start()
    {
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
                orientation += new Vector2(0, -1) * speed;
            }
            else
            {
                orientation += new Vector2(0, 1) * speed;
            }
            
        }
        else
        {            
            
            if (obstacleTurnsAround)
            {
                orientation += new Vector2(1, 0) * speed;
            }
            else
            {
                orientation += new Vector2(-1, 0) * speed;
            }
        }
         
    }
}
