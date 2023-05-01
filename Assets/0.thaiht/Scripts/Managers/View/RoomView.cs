using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RoomView : View
{
    [SerializeField] public Button btnBack;
    [SerializeField] public Button btnStartGame;
    [SerializeField] TextMeshProUGUI txtBannerRoomName;

    public override void Initialize()
    {
        LobbyRoomController.eventUpdateLobbyRoom += OnUpdateLobbyUI;
        Debug.Log("lobby");
    }

    public void SetBannerRoomName(string roomName)
    {
        txtBannerRoomName.text = roomName;
    }

    public void OnUpdateLobbyUI()
    {
        //txtPlayerList.text = "";

        //foreach (Player player in PhotonNetwork.PlayerList)
        //{
        //    txtPlayerList.text += player.NickName + "\n";
        //}

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    btnStartGame.interactable = true;
        //}
        //else
        //{
        //    btnStartGame.interactable = false;
        //}
    }

    private void OnDestroy()
    {
        LobbyRoomController.eventUpdateLobbyRoom -= OnUpdateLobbyUI;
    }
}
