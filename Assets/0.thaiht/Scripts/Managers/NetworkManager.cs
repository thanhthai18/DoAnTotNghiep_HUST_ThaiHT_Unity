using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    public Dictionary<string, RoomInfo> cachedRoomList;
    public bool IsConnected => PhotonNetwork.IsConnected;
    public bool InLobby => PhotonNetwork.InLobby;
    public bool InRoom => PhotonNetwork.InRoom;
    public Player[] PlayerList => PhotonNetwork.PlayerList;
    public Player LocalPlayer => PhotonNetwork.LocalPlayer;
    public Room CurrentRoom => PhotonNetwork.CurrentRoom;
    public bool IsMasterClient => PhotonNetwork.IsMasterClient;
    public string NickName
    {
        get => PhotonNetwork.NickName;
        set => PhotonNetwork.NickName = value;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //private void Start()
    //{
    //    PhotonNetwork.ConnectUsingSettings();
    //}

    public void ConnectMasterServerToOpenMode(ModeGame enumModeGame)
    {
        PhotonNetwork.NickName = MyPlayerValue.playerName;
        GlobalValue.currentModeGame = enumModeGame;
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GlobalController.Instance.Wait(0.5f, () =>
            {
                LoaderSystem.Loading(false);

                if (GlobalValue.currentModeGame == ModeGame.RoomMode)
                {
                    PhotonNetwork.LoadLevel(SceneGame.RoomModeScene);
                }
                else if (GlobalValue.currentModeGame == ModeGame.RankMode)
                {
                    PhotonNetwork.LoadLevel(SceneGame.RankModeScene);
                }
            });
            
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Da ket noi toi master server");
        PhotonNetwork.EnableCloseConnection = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Da mat ket noi toi mater server");
    }




    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions() { MaxPlayers = 4 , BroadcastPropsChangeToAll = true});
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}