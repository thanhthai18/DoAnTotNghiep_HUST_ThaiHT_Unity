using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomModeController : MonoBehaviourPunCallbacks
{
    public static RoomModeController instance;

    [SerializeField] LobbyRoomView lobbyRoomView;
    [SerializeField] private RoomView roomView;
    [SerializeField] SlotRoomSelect slotRoomPrefab;
    [SerializeField] Dictionary<string, SlotRoomSelect> listRoomEntry;
    [SerializeField] float timeStepUpdateRoom = 1.0f;
    private float nextTimeUpdateRoom;

    private Dictionary<string, RoomInfo> cachedRoomList;
    public PlayerChoose PlayerListEntryPrefab;
    private Dictionary<int, PlayerChoose> playerListChoose;
    [SerializeField] PanelSetupKeyHost panelSetupKeyHost_MapTime;


    private void Awake()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("JoinLobby");
        }
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        listRoomEntry = new Dictionary<string, SlotRoomSelect>();
    }

    private void Start()
    {
        lobbyRoomView.btnCreateRoom.onClick.AddListener(OnClickBtnCreateRoom);
        lobbyRoomView.btnJoinRoom.onClick.AddListener(OnClickBtnJoinRoom);
        roomView.btnBack.onClick.AddListener(OnLeaveRoomButton);
        roomView.btnStartGame.onClick.AddListener(OnStartGameButton);
    }


    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("JoinLobby");
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("dua nhau a");
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        Debug.Log("OnJoinedLobby");
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {

    }


    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
        cachedRoomList.Clear();
        Debug.Log("da tham gia phong");
        CallSetBannerRoomView();
        ViewManager.Show<RoomView>();


        if (playerListChoose == null)
        {
            playerListChoose = new Dictionary<int, PlayerChoose>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            PlayerChoose entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(roomView.playerChooseParent.transform);
            entry.GetComponent<PlayerChoose>().Initialize(p.ActorNumber, p.NickName, MyPlayerValue.rankScore, p);

            object isPlayerReady, characterIndex;
            if (p.CustomProperties.TryGetValue("isPlayerReady", out isPlayerReady))
            {
                entry.GetComponent<PlayerChoose>().SetPlayerReady((bool)isPlayerReady);
            }
            if (p.CustomProperties.TryGetValue("characterIndex", out characterIndex))
            {
                entry.SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[(int)characterIndex]);
                Debug.Log("Caccccc");
            }
            else
            {
                //entry.playerProperties = new Hashtable() { { "characterIndex", 0 } };
                //p.SetCustomProperties(entry.playerProperties);
                //entry.SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[0]);
                //Debug.Log("Lozzzzzzzzzz");
            }

            entry.btnKick.gameObject.SetActive(false);

            playerListChoose.Add(p.ActorNumber, entry);
        }
        //CheckActiveBtnKickPlayer();
        CheckKeyHost();
        roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());

        //Hashtable props = new Hashtable
        //    {
        //        {"PlayerLoadedLevel", false}
        //    };
        //PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Da tao phong: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnLeftRoom()
    {
        ViewManager.Show<LobbyRoomView>();

        PhotonNetwork.LocalPlayer.CustomProperties.Clear();

        foreach (PlayerChoose entry in playerListChoose.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListChoose.Clear();
        playerListChoose = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerChoose entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(roomView.playerChooseParent.transform);
        entry.GetComponent<PlayerChoose>().Initialize(newPlayer.ActorNumber, newPlayer.NickName, MyPlayerValue.rankScore, newPlayer);

        entry.btnKick.gameObject.SetActive(false);
        entry.keyHostIcon.SetActive(false);

        playerListChoose.Add(newPlayer.ActorNumber, entry);

        //CheckActiveBtnKickPlayer();
        CheckKeyHost();
        roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //roomView.OnUpdateLobbyUI();
        Destroy(playerListChoose[otherPlayer.ActorNumber].gameObject);
        playerListChoose.Remove(otherPlayer.ActorNumber);

        roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
        }
        CheckKeyHost();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {

        if (playerListChoose == null)
        {
            playerListChoose = new Dictionary<int, PlayerChoose>();
        }

        PlayerChoose entry;
        if (playerListChoose.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady, characterIndex;
            if (changedProps.TryGetValue("isPlayerReady", out isPlayerReady))
            {
                entry.SetPlayerReady((bool)isPlayerReady);
            }
            if (changedProps.TryGetValue("characterIndex", out characterIndex))
            {
                entry.SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[(int)characterIndex]);
                Debug.Log("Update properties character");
            }
        }

        roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
    }

    #endregion

    private void CheckActiveBtnKickPlayer()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            foreach (PlayerChoose value in playerListChoose.Values)
            {
                value.btnKick.gameObject.SetActive(true);
                value.btnKick.onClickEvent.AddListener(() => { PhotonNetwork.CloseConnection(value.myPlayerPhoton); });
            }
            playerListChoose[PhotonNetwork.LocalPlayer.ActorNumber].btnKick.gameObject.SetActive(false);
        }
    }

    private void CheckKeyHost()
    {
        //foreach (PlayerChoose value in playerListChoose.Values)
        //{
        //    value.keyHostIcon.SetActive(false);
        //}
        foreach (PlayerChoose value in playerListChoose.Values)
        {
            if (value.myPlayerPhoton.IsMasterClient)
            {
                value.keyHostIcon.SetActive(true);
                value.btnReady.gameObject.SetActive(false);
                value.imgReadyIcon.gameObject.SetActive(false);
                panelSetupKeyHost_MapTime.gameObject.SetActive(true);
            }
            else
            {
                value.keyHostIcon.SetActive(false);
                panelSetupKeyHost_MapTime.gameObject.SetActive(false);
            }
        }

        CheckActiveBtnKickPlayer();
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    foreach( var p in PhotonNetwork.PlayerList)
        //    {
        //        if(p != PhotonNetwork.LocalPlayer)
        //        {
        //            playerListChoose[p.ActorNumber].btnKick.gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            playerListChoose[p.ActorNumber].btnKick.gameObject.SetActive(false);
        //        }
        //    }
        //}

    }
    private void ClearRoomListView()
    {
        foreach (SlotRoomSelect entry in listRoomEntry.Values)
        {
            Destroy(entry.gameObject);
        }

        listRoomEntry.Clear();
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }
    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            var slot = Instantiate(slotRoomPrefab, lobbyRoomView.contentSlot.transform);
            slot.transform.localScale = Vector3.one;
            slot.Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            listRoomEntry.Add(info.Name, slot);
        }
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if(p != PhotonNetwork.MasterClient)
            {
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue("isPlayerReady", out isPlayerReady))
                {
                    if (!(bool)isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            
        }

        return true;
    }



    public void LocalPlayerPropertiesUpdated()
    {
        roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
    }


    public void OnClickBtnCreateRoom()
    {
        NetworkManager.instance.CreateRoom(lobbyRoomView.inputRoomName.text);
    }
    public void OnClickBtnJoinRoom()
    {
        PhotonNetwork.JoinRoom(lobbyRoomView.inputRoomName.text);
    }


    //------RoomView------//
    public void CallSetBannerRoomView()
    {
        roomView.SetBannerRoomName(PhotonNetwork.CurrentRoom.Name);
    }


    public void OnLeaveRoomButton()
    {
        PhotonNetwork.LeaveRoom();

    }

    //thai thai thai
    public void SetRoomMapProperties()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Hashtable newProp = new Hashtable() { { "indexMap", (int)PhotonNetwork.LocalPlayer.CustomProperties["indexRoomChoose"] } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(newProp);
        }
    }


    public void OnStartGameButton()
    {
        if (PhotonNetwork.PlayerList.Length >= 1 && PhotonNetwork.PlayerList.Length < 5)
        {
            NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "MainGameScene");
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            //PhotonNetwork.LoadLevel("DemoAsteroids-GameScene");
        }
    }
}

