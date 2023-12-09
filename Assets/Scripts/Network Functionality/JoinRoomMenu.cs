using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class JoinRoomMenu : MonoBehaviourPunCallbacks
{
    //input fields
    [SerializeField] private TMP_InputField room_name;
    [SerializeField] private TMP_InputField player_name;

    [SerializeField] private TransitionType transition;

    public override void OnJoinedRoom()
    {
        Debug.Log("Entrou Na Sala, criando transicao");

        Singleton.Instance.CreateTransition(TransitionType.GearRolling, TransitionMode.OnlineLoadScene, "Lobby");
    }

    public void JoinButton()
    {
        PhotonNetwork.NickName = player_name.text;
        PhotonNetwork.JoinOrCreateRoom(room_name.text, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default, null);
    }

    public void ExitButton()
    {
        Debug.Log("Saindo do jogo...");

        Singleton.Instance.CreateTransition(TransitionType.GearRolling, TransitionMode.ExitGame);
    }
}
