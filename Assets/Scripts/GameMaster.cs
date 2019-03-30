using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public GameObject playerReference;
    public GameObject pickupsReference;
    private TileBase[] emptyPlayer;

    public Vector3 lastCheckpointPos = new Vector3(0, 0, 0);
    public int playerBoxesCount = 0;
    public TileBase[] pickups;// = new Tilemap();
    public BoundsInt pickupsBounds;

    public void resetLevelToCheckpoint()
    {
        playerReference.transform.position = lastCheckpointPos;
        playerReference.GetComponent<PlayerController>().boxesInInventory = playerBoxesCount;
        playerReference.GetComponent<Tilemap>().SetTilesBlock(GameObject.FindGameObjectWithTag("Player").GetComponent<Tilemap>().cellBounds, emptyPlayer);
        pickupsReference.GetComponent<Tilemap>().SetTilesBlock(pickupsBounds, pickups);
    }

    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        }else{
            Destroy(gameObject);
        }

        emptyPlayer = playerReference.GetComponent<Tilemap>().GetTilesBlock(GameObject.FindGameObjectWithTag("Player").GetComponent<Tilemap>().cellBounds); ;
        
    }
}
