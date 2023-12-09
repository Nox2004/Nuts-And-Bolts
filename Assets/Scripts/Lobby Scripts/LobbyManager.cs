using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;

public enum CharacterID : int
{
    None = -1,
    Assistent = 0,
    Doctor = 1
}

public class LobbyManager : MonoBehaviourPunCallbacks
{   
    //UI Objects
    [SerializeField] private GameObject assistent_ui;
    [SerializeField] private TextMeshProUGUI assistent_selection;
    private Image assistent_image;
    [SerializeField] private GameObject doctor_ui;
    [SerializeField] private TextMeshProUGUI doctor_selection;
    private Image doctor_image;

    TransitionType transition_to_game_type = TransitionType.GearRolling;

    public GameObject canvas;

    //Scale of the UI
    private float assistent_scale = 1f;
    private float doctor_scale = 1f;

    public string[] character_selection = new string[2];
    private CharacterID selected = CharacterID.None;
    private CharacterID mouse_over = CharacterID.None; //Used for sounds only
    private ExitGames.Client.Photon.Hashtable playerProperties;

    //Audio Stuff
    private AudioSource audio_source;
    [SerializeField] private AudioClip mouse_over_sound;
    [SerializeField] private AudioClip change_selection_sound;

    // Start
    void Start()
    {
        //Create audio Source
        audio_source = gameObject.AddComponent<AudioSource>();

        UpdatePlayerProperties(CharacterID.None);

        character_selection[(int) CharacterID.Assistent] = "";
        character_selection[(int) CharacterID.Doctor] = "";

        assistent_image = assistent_ui.GetComponent<Image>();
        doctor_image = doctor_ui.GetComponent<Image>();

        //Update character selection with other playes properties
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("character"))
            {
                var new_selected = (CharacterID) player.CustomProperties["character"];

                if (new_selected != CharacterID.None)
                {
                    //selects in managers list
                    character_selection[(int) new_selected] = player.NickName;
                    return;
                }
            }
        }

        //instantiate "LobbySelection" with canvas as its parent
        //var obj = PhotonNetwork.Instantiate("LobbySelection", canvas.transform.position, canvas.transform.rotation);
        //obj.GetComponent<LobbySelection>().manager = this;
    }

    // Update 
    void Update()
    {
        //if both characters are selected
        if (character_selection[(int) CharacterID.Assistent] != "" && character_selection[(int) CharacterID.Doctor] != "")
        {
            //load the scene
            Singleton.Instance.CreateTransition(transition_to_game_type, TransitionMode.OnlineLoadScene, "Game");
            Singleton.Instance.player_character = selected;

            //disable itself
            gameObject.SetActive(false);
        }

        //if press right button
        if (Input.GetMouseButtonDown(1))
        {
            //if is selecting noone, exit the room
            if (selected == CharacterID.None)
            {
                PhotonNetwork.LeaveRoom();
                Singleton.Instance.CreateTransition(TransitionType.GearRolling, TransitionMode.LoadScene, "Menu");
                //disable myself
                gameObject.SetActive(false);
                return;
            }

            audio_source.pitch = 0.65f;
            audio_source.PlayOneShot(change_selection_sound);

            //Unselect
            ChangeCharacter(CharacterID.None);
        }

        if (Input.mousePosition.x > Screen.width / 2)
        {
            //play mouse over sound at a random pitch
            audio_source.pitch = Random.Range(0.9f, 1.1f);
            if (mouse_over == CharacterID.Doctor) audio_source.PlayOneShot(mouse_over_sound);

            //returns true if mouse is clicked
            if (IsSelecting(CharacterID.Assistent)) 
            {
                ChangeCharacter(CharacterID.Assistent);
            }
        }
        else
        {
            audio_source.pitch = Random.Range(0.9f, 1.1f);
            if (mouse_over == CharacterID.Assistent) audio_source.PlayOneShot(mouse_over_sound);

            //returns true if mouse is clicked
            if (IsSelecting(CharacterID.Doctor)) 
            {
                ChangeCharacter(CharacterID.Doctor);
            }
        }

        //Lerp the scale
        assistent_ui.transform.localScale += (new Vector3(1f,1f,1f) * assistent_scale-assistent_ui.transform.localScale)/10f;
        doctor_ui.transform.localScale += (new Vector3(1f,1f,1f) * doctor_scale-doctor_ui.transform.localScale)/10f;

        //Update the UI
        assistent_selection.text = character_selection[(int) CharacterID.Assistent];
        doctor_selection.text = character_selection[(int) CharacterID.Doctor];
    }

    public bool IsSelecting(CharacterID character)
    {
        mouse_over = character;

        if (character == CharacterID.Doctor)
        {
            //Changes their scale and order in the hierarchy
            assistent_scale = 1.1f;
            doctor_scale = 1f;
            assistent_ui.transform.SetSiblingIndex(0);
            doctor_ui.transform.SetSiblingIndex(1);

            if (Input.GetMouseButtonDown(0)) //If the mouse is clicked
            {
                return true;
            }
        }
        else
        {
            //Changes their scale and order in the hierarchy
            assistent_ui.transform.SetSiblingIndex(1);
            doctor_ui.transform.SetSiblingIndex(0);
            doctor_scale = 1.1f;
            assistent_scale = 1f;

            if (Input.GetMouseButtonDown(0)) //If the mouse is clicked
            {
                return true;
            }
        }

        return false;
    }
    
    private void ChangeCharacter(CharacterID character)
    {
        if (character == CharacterID.None)
        {
            selected = CharacterID.None;
            UpdatePlayerProperties(selected);
            return;
        }
        
        //Tried to select someone already selected
        if (character_selection[(int) character] != "") 
        {
            audio_source.pitch = 0.6f; audio_source.PlayOneShot(change_selection_sound); // Play Sound
            return;
        }

        //Changes the character
        selected = character;
        audio_source.pitch = Random.Range(0.8f,1f); audio_source.PlayOneShot(change_selection_sound); // Play Sound

        //add selected character to the player Custom Properties
        UpdatePlayerProperties(selected);
    }

    //update the player properties
    public void UpdatePlayerProperties(CharacterID character)
    {
        //Debug
        Debug.Log("UpdatePlayerProperties: " + character);
        //add selected character to the player Custom Properties
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties.Add("character", character);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    //on player leave
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //if the player has selected a character
        if (otherPlayer.CustomProperties.ContainsKey("character"))
        {
            //if the player has unselected a character
            var new_selected = (CharacterID) otherPlayer.CustomProperties["character"];

            if (new_selected == CharacterID.None) return;

            //selects in managers list
            character_selection[(int) new_selected] = "";
        }
    }

    //on player properties update
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        //if the player is me
        //if (targetPlayer != PhotonNetwork.LocalPlayer)
        //{
            //if the player has selected a character
            if (changedProps.ContainsKey("character"))
            {
                //if the player has unselected a character
                if (character_selection[(int) CharacterID.Assistent] == targetPlayer.NickName) character_selection[(int) CharacterID.Assistent] = "";
                if (character_selection[(int) CharacterID.Doctor] == targetPlayer.NickName) character_selection[(int) CharacterID.Doctor] = "";

                //if the player has selected a character
                var new_selected = (CharacterID) changedProps["character"];

                if (new_selected == CharacterID.None) return;

                //selects in managers list
                character_selection[(int) new_selected] = targetPlayer.NickName;
                
            }
        //}
    }
}
