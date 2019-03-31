using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class hatScript : MonoBehaviour
{

    public GameObject drPhilPlayer;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        drPhilPlayer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            cam.GetComponent<CinemachineVirtualCamera>().Priority = 11;
            drPhilPlayer.SetActive(true);
            other.gameObject.SetActive(false);
 
        }
    }
}
