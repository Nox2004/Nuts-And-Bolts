using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    //Awake
    void Awake()
    {
        //If there is no instance, set it to this
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject); //Sets this to not be destroyed when reloading scene
    }

    public void GoToLevel(string scene_name)
    {
        Debug.Log("Loading level: " + scene_name);
        PhotonNetwork.LoadLevel(scene_name);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
