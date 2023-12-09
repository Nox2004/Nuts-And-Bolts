using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LevelManager : MonoBehaviourPunCallbacks
{
    //Depending on the character, the level will be different - Disable the other character's level
    //Has functions to pass items from one player to the other using player custom properties
    public bool ended = false;

    [SerializeField] private GameObject assistent_room_parent;
    [SerializeField] private GameObject doctor_room_parent;
    [SerializeField] private Vector3 assistent_room_instantiate;

    [SerializeField] private DoctorInAssistentRoom doctor_object_in_assistent_room;

    private SendItemEvent item_sender;
    private ReceiveItemEvent item_receiver;

    private Player other_player;
    private Player my_player;

    // Start is called before the first frame update
    void Start()
    {
        //Get item Sender and Receiver components
        item_sender = GetComponent<SendItemEvent>();
        item_receiver = GetComponent<ReceiveItemEvent>();

        item_receiver.levelManager = this;
        item_receiver.character = Singleton.Instance.player_character;

        //get my player
        my_player = PhotonNetwork.LocalPlayer;

        //get the other player
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player != my_player)
            {
                other_player = player;
                break;
            }
        }
        switch (Singleton.Instance.player_character)
        {
            case CharacterID.Assistent:
                doctor_room_parent.SetActive(false);
                break;
            case CharacterID.Doctor:
                assistent_room_parent.SetActive(false);
                break;
        }

        if (Singleton.Instance.player_character==CharacterID.Doctor) SetDoctorItems(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SendItem(SendItemEventData data)
    {
        //if i am assistent and the other player has an item
        if (Singleton.Instance.player_character == CharacterID.Assistent)
        {
            if ((bool)other_player.CustomProperties["has_item"])
            {
                Debug.Log("Assistent - Other player has an item"); return false;
            }
            Debug.Log("Assistent - I am passing a Item to the Doctor, he has an item now"); SetDoctorItems(true);
        }
        if (Singleton.Instance.player_character == CharacterID.Doctor)
        {
            Debug.Log("Doctor - I have no more items"); SetDoctorItems(false);
        }

        item_sender.SendItem(data);
        /*
        //add selected character to the player Custom Properties
        playerProperties.Add("last_item", obj.name);
        Debug.Log("Sending item: "+obj.name);
        my_player.SetCustomProperties(playerProperties);
        */
        return true;
    }

    //Instantiate Items in assistents room
    public GameObject ReceiveAssistent(string obj_name, ItemProperties prop)
    {
        GameObject obj = (GameObject)Resources.Load("Assistent/"+obj_name, typeof(GameObject));

        var _obj = Instantiate(obj, assistent_room_parent.transform);

        doctor_object_in_assistent_room.handing_item = true;
        doctor_object_in_assistent_room.held_item = _obj;
        _obj.transform.position = doctor_object_in_assistent_room.instantiate_pos;

        _obj.GetComponent<AssistentItem>().properties = prop;
        _obj.GetComponent<AssistentItem>().received = true;

        return _obj;
    }

    //Instantiate Items in doctors room
    public GameObject ReceiveDoctor(string obj_name, ItemProperties prop)
    {
        GameObject obj = (GameObject)Resources.Load("Doctor/"+obj_name, typeof(GameObject));

        //instantiate the object
        var _obj = Instantiate(obj, doctor_room_parent.transform);
        _obj.GetComponent<DoctorItem>().properties = prop;
        _obj.GetComponent<DoctorItem>().received = true;

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties.Add("has_item", true);
        my_player.SetCustomProperties(playerProperties);

        return _obj;
    }
    
    //Set the Doctor has_item property
    public void SetDoctorItems(bool has_item)
    {
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties.Add("has_item", has_item);

        if (Singleton.Instance.player_character == CharacterID.Doctor) my_player.SetCustomProperties(playerProperties);
        else other_player.SetCustomProperties(playerProperties);
    }

    //Trigger this to win the game
    public void WinTheLevel()
    {
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties.Add("win", true);
        my_player.SetCustomProperties(playerProperties);
    }

    //Return to menu if player leaves
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (ended) return;

        //finish connection
        PhotonNetwork.LeaveRoom();
        Singleton.Instance.CreateTransition(TransitionType.GearRolling, TransitionMode.LoadScene, "DemoEndDisconected");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("win"))
        {
            //finish connection
            if(PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
            if (!ended) Singleton.Instance.CreateTransition(TransitionType.GearRolling, TransitionMode.LoadScene, "DemoEndWin");
            ended = true;
        }

        //if the player is not me
        /*
        if (targetPlayer != my_player)
        {
            if (!changedProps.ContainsKey("last_item")) return;
            
            string received_item_name = (string) changedProps["last_item"];
            
            switch (Singleton.Instance.player_character)
            {
                case CharacterID.Assistent:
                {
                    GameObject received_item = (GameObject)Resources.Load("Assistent/"+received_item_name, typeof(GameObject));

                    ReceiveAssistent(received_item);
                }
                break;
                case CharacterID.Doctor:
                {
                    GameObject received_item = (GameObject)Resources.Load("Doctor/"+received_item_name, typeof(GameObject));

                    ReceiveDoctor(received_item);
                }
                break;
            }
        }
        */
    }
}
