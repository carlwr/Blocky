using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class RocketScript : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;
    public string nextScene;
    public float acceleration;
    private float speed = 0;
    private bool activated;



    void Start()
    {
        activated = false;
        player.SetActive(false);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            activated = true;
            cam.GetComponent<CinemachineVirtualCamera>().Priority = 11;
            player.SetActive(true);
            other.transform.position = transform.position;
            other.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if(activated){
            liftOff();

        }
    }

    void liftOff(){
        speed += acceleration;
        Vector3 pos = gameObject.transform.position;
        gameObject.transform.position =  pos + new Vector3(0,speed,0) * Time.deltaTime;
    }
    
}
