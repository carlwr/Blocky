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
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void initiateConversation(){

    }

    public void stopConversation(){
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player"){
            // Generate a random index less than the size of the array.  
            int index = rand.Next(gibberishNpcTalking.Length);
            //Jump sound played
            int gibberishID = EazySoundManager.PlaySound(gibberishNpcTalking[index], 0.5f);
            var pPos = transform.position;
            UIController.instance.conversation.text = text;
            UIController.instance.setConversationPosition(new Vector3(transform.position.x, transform.position.y + 4, 0));
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Player"){
            UIController.instance.conversation.text = "";

        }
    }

}