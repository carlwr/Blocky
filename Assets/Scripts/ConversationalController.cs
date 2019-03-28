using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Hellmade.Sound;
public class ConversationalController : MonoBehaviour
{
    public AudioClip[] gibberishNpcTalking;
    public System.Random rand = new System.Random();
    Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
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
            Debug.Log("index:" + index);
            Debug.Log("array length :" + gibberishNpcTalking.Length);

            int gibberishID = EazySoundManager.PlaySound(gibberishNpcTalking[index], 0.5f);
            for (int i = (int)collider.transform.position.x - 3; i < (int)collider.transform.position.x + 3; i++){
                for(int j = (int)collider.transform.position.y - 3; j < (int)collider.transform.position.y + 3; j++){
                    var pPos = tilemap.WorldToCell(new Vector2(i,j));
                    if(tilemap.GetTile(pPos) != null){
                        string text = ((ConversationalAgentTile)tilemap.GetTile(new Vector3Int(i,j,0))).text;
                        UIController.instance.conversation.text = text;
                        UIController.instance.setConversationPosition(new Vector3(i, j + 4, 0));
                    }
                }   
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