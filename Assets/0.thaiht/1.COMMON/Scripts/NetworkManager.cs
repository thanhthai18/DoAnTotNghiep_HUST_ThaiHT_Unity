using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Reflection;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;
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

    private Action OnConnectCompleted;
    public static event Action ActionOnConnectedToMaster;
    public static event Action<List<RoomInfo>> ActionOnRoomListUpdate;
    public static event Action ActionOnJoinedLobby;
    public static event Action ActionOnLeftLobby;
    public static event Action ActionOnCreateRoomFailed;
    public static event Action ActionOnJoinRoomFailed;
    public static event Action ActionOnJoinRandomFailed;
    public static event Action ActionOnJoinedRoom;
    public static event Action ActionOnLeftRoom;
    public static event Action<Player> ActionOnMasterClientSwitched;
    public static event Action<Player, ExitGames.Client.Photon.Hashtable> ActionOnPlayerPropertiesUpdate;
    public static event Action ActionOnPlayerListChanged;
    public static event Action ActionOnDisconnected;
    public static event Action ActionOnCreateRoom;
    public static event Action<Player> ActionOnPlayerEnterRoom;
    public static event Action<Player> ActionOnPLayerLeftRoom;
    public static event Action<ExitGames.Client.Photon.Hashtable> ActionOnRoomUpdateProperties;


    //    #region Singleton
    //    protected NetworkManager() { }

    //    private static NetworkManager f_instance;

    //    /// <summary> Returns a reference to the UIPopupManager in the scene. If one does not exist, it gets created. </summary>
    //    public static NetworkManager Instance
    //    {
    //        get
    //        {
    //            if (f_instance != null) return f_instance;
    //            //if (ApplicationIsQuitting) return null;
    //            f_instance = FindObjectOfType<NetworkManager>();
    //            if (f_instance == null) DontDestroyOnLoad(AddToScene().gameObject);
    //            return f_instance;
    //        }
    //    }
    //    private static NetworkManager AddToScene(bool selectGameObjectAfterCreation = false) { return AddToScene<NetworkManager>($"{MethodBase.GetCurrentMethod().DeclaringType}", true, selectGameObjectAfterCreation); }
    //    public static T AddToScene<T>(string gameObjectName, bool isSingleton, bool selectGameObjectAfterCreation = false) where T : MonoBehaviour
    //    {
    //        var component = FindObjectOfType<T>();
    //        if (component != null && isSingleton)
    //        {
    //            Debug.Log("Cannot add another " + typeof(T).Name + " to this Scene because you don't need more than one.");
    //#if UNITY_EDITOR
    //            UnityEditor.Selection.activeObject = component;
    //#endif
    //            return component;
    //        }

    //        component = new GameObject(gameObjectName, typeof(T)).GetComponent<T>();

    //#if UNITY_EDITOR
    //        UnityEditor.Undo.RegisterCreatedObjectUndo(component.gameObject, "Created " + gameObjectName);
    //        if (selectGameObjectAfterCreation) UnityEditor.Selection.activeObject = component.gameObject;
    //#endif
    //        return component;
    //    }
    //    private void Awake()
    //    {
    //        if (f_instance != null && f_instance != this)
    //        {
    //            Destroy(gameObject);
    //            return;
    //        }
    //        f_instance = this;
    //        DontDestroyOnLoad(gameObject);


    //    }
    //    public void InitInstance() { }
    //    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

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

    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Da ket noi toi master server");
        ActionOnConnectedToMaster?.Invoke();
        PhotonNetwork.EnableCloseConnection = true;
        GlobalController.Instance.HidePopupDisconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Da mat ket noi toi mater server");
        ActionOnDisconnected?.Invoke();
    }
    public override void OnJoinedLobby()
    {
        ActionOnJoinedLobby?.Invoke();
    }
    public override void OnLeftLobby()
    {
        ActionOnLeftLobby?.Invoke();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ActionOnRoomListUpdate?.Invoke(roomList);
    }
    public override void OnCreatedRoom()
    {
        ActionOnCreateRoom?.Invoke();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ActionOnCreateRoomFailed?.Invoke();
    }
    public override void OnJoinedRoom()
    {
        ActionOnJoinedRoom?.Invoke();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ActionOnJoinRoomFailed?.Invoke();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ActionOnJoinRandomFailed?.Invoke();
    }
    public override void OnLeftRoom()
    {
        ActionOnLeftRoom?.Invoke();
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        ActionOnMasterClientSwitched?.Invoke(newMasterClient);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        ActionOnPlayerPropertiesUpdate?.Invoke(targetPlayer, changedProps);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ActionOnPlayerEnterRoom?.Invoke(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ActionOnPLayerLeftRoom?.Invoke(otherPlayer);
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        ActionOnRoomUpdateProperties?.Invoke(propertiesThatChanged);
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Disconnected)
        {
            Debug.Log("Đã mất kết nối với Photon Server");
            LoaderSystem.Loading(false);
            GlobalController.Instance.ShowPopupDisconnect();
        }
    }

    

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true, CleanupCacheOnLeave = false }); ;
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