using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;


public class BossSceneScript : MonoBehaviour
{
    public GameObject boss;
    public AudioClip bossSound;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            
            int backgroundMusicID = EazySoundManager.PlayMusic(bossSound, 0.35f, true, false, 1, 1);
        
           Instantiate(boss);
           Destroy(gameObject);
        }
    }
}
