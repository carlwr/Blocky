using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public TextMeshProUGUI boxesUnused;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
