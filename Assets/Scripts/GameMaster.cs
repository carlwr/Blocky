using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    
    public Vector3 lastCheckpointPos = new Vector3(0, 0, 0);
    public int playerBoxesCount = 0;
    public TileBase[] pickups;// = new Tilemap();
    public BoundsInt pickupsBounds;

    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        }else{
            Destroy(gameObject);
        }
        
    }
}
