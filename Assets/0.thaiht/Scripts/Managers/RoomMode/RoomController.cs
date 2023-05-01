using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomController : MonoBehaviourPunCallbacks
{
    public static RoomController instance;
    [SerializeField] private RoomView roomView;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        roomView.btnBack.onClick.AddListener(OnLeaveLobbyButton);
        roomView.btnStartGame.onClick.AddListener(OnStartGameButton);
    }

    public void CallSetBannerRoomView()
    {
        roomView.SetBannerRoomName(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomView.OnUpdateLobbyUI();
    }

    public void OnLeaveLobbyButton()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            LobbyRoomController.instance.CallRemoveRoomSelect(PhotonNetwork.CurrentRoom.Name);
        }
        PhotonNetwork.LeaveRoom();
        ViewManager.Show<LobbyRoomView>();

    }

    public void OnStartGameButton()
    {
        if (PhotonNetwork.PlayerList.Length >= 1 && PhotonNetwork.PlayerList.Length < 5)
        {
            NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "MainGameScene");

        }
    }
}
