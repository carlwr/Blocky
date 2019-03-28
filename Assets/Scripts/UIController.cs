using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    RectTransform UI_Element;
    RectTransform CanvasRect;
    public Camera camera;

    public Vector3 conversationPosition;
    public TextMeshProUGUI boxesUnused;

    public TextMeshProUGUI conversation;

    
    public TextMeshProUGUI jumpType;
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        
        CanvasRect=canvas.GetComponent<RectTransform>();
        conversation.text = "LOL";

    }

    // Update is called once per frame
    void Update()
    {
        updateConversationPosition();
    }

    void updateConversationPosition(){
        Vector2 ViewportPosition=camera.WorldToScreenPoint(conversationPosition);
        
        Vector2 WorldObject_ScreenPosition=new Vector2(
            ((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
            ((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));
 //now you can set the position of the ui element
        conversation.transform.position=ViewportPosition;
    }

    public void setConversationPosition(Vector3 objectPos){
        conversationPosition = objectPos;
      
    }

    public void decreaseBoxesUnused(){
        int boxesUnusedInt = int.Parse(boxesUnused.text);
        boxesUnusedInt --;
        boxesUnused.text = System.Convert.ToString(boxesUnusedInt);
    }

    public void increaseBoxesUnused(){
        int boxesUnusedInt = int.Parse(boxesUnused.text);
        boxesUnusedInt ++;
        boxesUnused.text = System.Convert.ToString(boxesUnusedInt);
    }
}
