using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;

public class BossSceneScript : MonoBehaviour
{
    public GameObject boss;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
        
           Instantiate(boss);
           Destroy(gameObject);
        }
    }
}
