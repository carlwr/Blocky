using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Hellmade.Sound;
public class ConversationalController : MonoBehaviour
{
    public string text;
    public AudioClip[] gibberishNpcTalking;
    
    static public int[] gibberish;
    public System.Random rand = new System.Random();

    public float npcTalkingCooldown = 4;
    private float time;
    public bool speak = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= time + npcTalkingCooldown)
        {
            speak = true;
        }
    }

    public void initiateConversation(){

    }

    public void stopConversation(){
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            // Generate a random index less than the size of the array.  
            
            var pPos = transform.position;
            UIController.instance.conversation.text = text;
            UIController.instance.setConversationPosition(new Vector3(transform.position.x, transform.position.y + 4, 0));
            if (speak)
            {
                int index = rand.Next(gibberishNpcTalking.Length);
                int gibberishID = EazySoundManager.PlaySound(gibberishNpcTalking[index], 0.5f);
                time = Time.time;
                speak = false;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Player"){
            UIController.instance.conversation.text = "";

        }
    }

}