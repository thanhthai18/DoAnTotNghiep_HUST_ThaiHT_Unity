using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using thaiht20183826;

public class RoomModeController : MonoBehaviour
{
    public static RoomModeController instance;

    [SerializeField] LobbyRoomView lobbyRoomView;
    [SerializeField] private RoomView roomView;
    [SerializeField] SlotRoomSelect slotRoomPrefab;
    [SerializeField] Dictionary<string, SlotRoomSelect> listRoomEntry;

    private Dictionary<string, RoomInfo> cachedRoomList;
    public PlayerChoose PlayerListEntryPrefab;
    private Dictionary<int, PlayerChoose> playerListChoose;
    [SerializeField] PanelSetupKeyHost panelSetupKeyHost_MapTime;


    private void Awake()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("JoiningLobby");
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
    }

    private void OnEnable()
    {
        lobbyRoomView.btnCreateRoom.onClick.AddListener(OnClickBtnCreateRoom);
        lobbyRoomView.btnJoinRoom.onClick.AddListener(OnClickBtnJoinRoom);
        roomView.btnBack.onClick.AddListener(OnLeaveRoomButton);
        roomView.btnStartGame.onClick.AddListener(OnStartGameButton);
        GlobalController.ActionOnUpdatedGlobalLeaderboard += OnUpdatedGlobalLeaderboard;
        NetworkManager.ActionOnConnectedToMaster += HandleOnConnectedToMaster;
        NetworkManager.ActionOnRoomListUpdate += HandleOnRoomListUpdate;
        NetworkManager.ActionOnJoinedLobby += HandleOnJoinedLobby;
        NetworkManager.ActionOnLeftLobby += HandleOnLeftLobby;
        NetworkManager.ActionOnCreateRoomFailed += HandleOnCreateRoomFailed;
        NetworkManager.ActionOnJoinRoomFailed += HandleOnJoinRoomFailed;
        NetworkManager.ActionOnJoinedRoom += HandleOnJoinedRoom;
        NetworkManager.ActionOnCreateRoom += HandleOnCreatedRoom;
        NetworkManager.ActionOnLeftRoom += HandleOnLeftRoom;
        NetworkManager.ActionOnPlayerEnterRoom += HandleOnPlayerEnteredRoom;
        NetworkManager.ActionOnPLayerLeftRoom += HandleOnPlayerLeftRoom;
        NetworkManager.ActionOnMasterClientSwitched += HandleOnMasterClientSwitched;
        NetworkManager.ActionOnPlayerPropertiesUpdate += HandleOnPlayerPropertiesUpdate;
        
    }

 

    private void OnDisable()
    {
        lobbyRoomView.btnCreateRoom.onClick.RemoveAllListeners();
        lobbyRoomView.btnJoinRoom.onClick.RemoveAllListeners();
        roomView.btnBack.onClick.RemoveAllListeners();
        roomView.btnStartGame.onClick.RemoveAllListeners();
        GlobalController.ActionOnUpdatedGlobalLeaderboard -= OnUpdatedGlobalLeaderboard;
        NetworkManager.ActionOnConnectedToMaster -= HandleOnConnectedToMaster;
        NetworkManager.ActionOnRoomListUpdate -= HandleOnRoomListUpdate;
        NetworkManager.ActionOnJoinedLobby -= HandleOnJoinedLobby;
        NetworkManager.ActionOnLeftLobby -= HandleOnLeftLobby;
        NetworkManager.ActionOnCreateRoomFailed -= HandleOnCreateRoomFailed;
        NetworkManager.ActionOnJoinRoomFailed -= HandleOnJoinRoomFailed;
        NetworkManager.ActionOnJoinedRoom -= HandleOnJoinedRoom;
        NetworkManager.ActionOnCreateRoom -= HandleOnCreatedRoom;
        NetworkManager.ActionOnLeftRoom -= HandleOnLeftRoom;
        NetworkManager.ActionOnPlayerEnterRoom -= HandleOnPlayerEnteredRoom;
        NetworkManager.ActionOnPLayerLeftRoom -= HandleOnPlayerLeftRoom;
        NetworkManager.ActionOnMasterClientSwitched -= HandleOnMasterClientSwitched;
        NetworkManager.ActionOnPlayerPropertiesUpdate -= HandleOnPlayerPropertiesUpdate;
   
    }

    private void OnUpdatedGlobalLeaderboard()
    {
        foreach (var value in playerListChoose.Values)
        {
            value.txtScoreRankDisplay.text = GlobalController.Instance.GetRankScorePlayer(value.txtPlayerName.text).ToString();
        }
    }

    #region PUN CALLBACKS

    public void HandleOnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("JoinLobby");
        }
    }

    public void HandleOnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("dua nhau a");
        Debug.Log(roomList.Count);
        roomList.ForEach(s => Debug.Log(s.Name));
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public void HandleOnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        Debug.Log("OnJoinedLobby");
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public void HandleOnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }
    private void HandleOnCreateRoomFailed()
    {
        throw new NotImplementedException();
    }
    private void HandleOnJoinRoomFailed()
    {
        throw new NotImplementedException();
    }


    public void HandleOnJoinedRoom()
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

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var p = PhotonNetwork.PlayerList[i];
            if(PhotonNetwork.LocalPlayer == p)
            {
                GlobalValue.indexPosSpawnPlayerGamePlay = i;
            }
            PlayerChoose entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(roomView.playerChooseParent.transform);
            entry.GetComponent<PlayerChoose>().Initialize(p.ActorNumber, p.NickName, GlobalController.Instance.GetRankScorePlayer(p.NickName), p);

            object isPlayerReady, characterIndex;
            if (p.CustomProperties.TryGetValue("isPlayerReady", out isPlayerReady))
            {
                entry.GetComponent<PlayerChoose>().SetPlayerReady((bool)isPlayerReady);
            }
            if (p.CustomProperties.TryGetValue("characterIndex", out characterIndex))
            {
                entry.SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[(int)characterIndex]);
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
        PlayFabController.GetLeaderboard();

        CheckKeyHost();
        roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
        SetRoomMapProperties(0);
        SetRoomTimePlayProperties(int.Parse(panelSetupKeyHost_MapTime.txtTimeChoose.text.Replace("s","")));
        //Hashtable props = new Hashtable
        //    {
        //        {"PlayerLoadedLevel", false}
        //    };
    }
    public void HandleOnCreatedRoom()
    {
        Debug.Log("Da tao phong: " + PhotonNetwork.CurrentRoom.Name);
    }


    public void HandleOnLeftRoom()
    {
        Debug.Log("LeavedRoom");
        if (SceneManagerHelper.ActiveSceneName == SceneGame.RoomModeScene)
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
    }

    public void HandleOnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerChoose entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(roomView.playerChooseParent.transform);
        entry.GetComponent<PlayerChoose>().Initialize(newPlayer.ActorNumber, newPlayer.NickName, GlobalController.Instance.GetRankScorePlayer(newPlayer.NickName), newPlayer);

        entry.btnKick.gameObject.SetActive(false);
        entry.keyHostIcon.SetActive(false);

        playerListChoose.Add(newPlayer.ActorNumber, entry);

        //CheckActiveBtnKickPlayer();
        CheckKeyHost();
        roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
    }

    public void HandleOnPlayerLeftRoom(Player otherPlayer)
    {
        //roomView.OnUpdateLobbyUI();
        Destroy(playerListChoose[otherPlayer.ActorNumber].gameObject);
        playerListChoose.Remove(otherPlayer.ActorNumber);
        roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
    }

    public void HandleOnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            roomView.btnStartGame.gameObject.SetActive(CheckPlayersReady());
        }
        CheckKeyHost();
    }

    public void HandleOnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
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
            }
            else
            {
                value.keyHostIcon.SetActive(false);
            }
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            panelSetupKeyHost_MapTime.ActiveButtonArrows(true);
        }
        else
        {
            panelSetupKeyHost_MapTime.ActiveButtonArrows(false);
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
    public void UpdateRoomListView()
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
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p != PhotonNetwork.MasterClient)
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
        NetworkManager.Instance.CreateRoom(lobbyRoomView.inputRoomName.text);
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
        Debug.Log("LeavingRoom");
        PhotonNetwork.LeaveRoom();

    }



    //thai thai thai
    public void SetRoomMapProperties(int index)
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["indexMap"] == null)
            {
                Hashtable newProp = new Hashtable() { { "indexMap", index } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(newProp);
            }
            else
            {
                PhotonNetwork.CurrentRoom.CustomProperties["indexMap"] = index;
            }
        }
      
    }
    public void SetRoomTimePlayProperties(int time)
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["timePlay"] == null)
            {
                Hashtable newProp = new Hashtable() { { "timePlay", time } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(newProp);
            }
            else
            {
                PhotonNetwork.CurrentRoom.CustomProperties["timePlay"] = time;
            }
        }
    }


    public void OnStartGameButton()
    {
        if (PhotonNetwork.PlayerList.Length >= 1 && PhotonNetwork.PlayerList.Length < 5)
        {
            //NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "MainGameScene");
            NetworkManager.Instance.ChangeScene("MainGameScene");
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            //PhotonNetwork.LoadLevel("DemoAsteroids-GameScene");
        }
    }
    private void OnDestroy()
    {
        Destroy(instance);
    }
}

