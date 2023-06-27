using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    public static GlobalController Instance;
    public Scene currentScene;
    public DataCharacter scriptableDataCharacter;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Debug.Log("Khoi tao GlobalController");
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        currentScene = arg0;
    }

    public void ReloadPreviousRoom()
    {
        Debug.Log("reload");
        SceneManager.sceneLoaded += DelayReJoinRoom;

        if (PhotonNetwork.IsMasterClient)
        {
            NetworkManager.Instance.ChangeScene(SceneGame.RoomModeScene);
        }

        //PhotonNetwork.LeaveRoom();
        //NetworkManager.ActionOnConnectedToMaster += MasterClientReJoining;

    }



    void DelayReJoinRoom(Scene arg0, LoadSceneMode arg1)
    {
        try
        {
            this.Wait(0.1f, () =>
            {
                if (arg0.name == SceneGame.RoomModeScene)
                {
                    if (RoomModeController.instance != null)
                    {
                        //PhotonNetwork.GetCustomRoomList(TypedLobby.Default, null);
                        RoomModeController.instance.HandleOnJoinedRoom();
                    }
                }
                LoaderSystem.Loading(false);
                SceneManager.sceneLoaded -= DelayReJoinRoom;
            });

        }
        catch (System.Exception e)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    //private void Update()
    //{
    //    if (PhotonNetwork.IsConnected)
    //    {
    //        Debug.Log("Count Room " + PhotonNetwork.CountOfRooms);
    //    }
    //}

    #region Old Rejoin
    //public void MasterClientReJoining()
    //{
    //    Debug.Log("rejoin");
    //    Debug.Log($"{PhotonNetwork.LocalPlayer.ActorNumber}, {GlobalValue.masterClientID}");

    //    NetworkManager.Instance.ChangeScene(SceneGame.RoomModeScene);
    //    NetworkManager.ActionOnJoinedLobby += OnMasterClientRejoinLobby;



    //}

    //public void OnMasterClientRejoinLobby()
    //{
    //    PhotonNetwork.CreateRoom(GlobalValue.previousRoom.Name, new RoomOptions { MaxPlayers = GlobalValue.previousRoom.MaxPlayers }, TypedLobby.Default);
    //    NetworkManager.ActionOnJoinedRoom += MasterClientReJoined;
    //    NetworkManager.ActionOnConnectedToMaster -= MasterClientReJoining;
    //    NetworkManager.ActionOnJoinedLobby -= OnMasterClientRejoinLobby;
    //}
    //public void MasterClientReJoined()
    //{
    //    LoaderSystem.Loading(false);
    //    NetworkManager.Instance.photonView.RPC(nameof(MemberClientRejoining), RpcTarget.Others); // Null?
    //    NetworkManager.ActionOnJoinedRoom -= MasterClientReJoined;
    //}

    //[PunRPC]
    //private void MemberClientRejoining()
    //{
    //    NetworkManager.Instance.ChangeScene(SceneGame.RoomModeScene);
    //    PhotonNetwork.LeaveRoom();

    //    NetworkManager.ActionOnJoinedLobby += CallMemberJoinRoom;
    //    //NetworkManager.Instance.photonView.RPC(nameof(JoinRoom), RpcTarget.Others, GlobalValue.previousRoom.Name);
    //}

    //private void CallMemberJoinRoom()
    //{
    //    JoinRoom(GlobalValue.previousRoom.Name);
    //    NetworkManager.ActionOnJoinedLobby -= CallMemberJoinRoom;
    //}
    //private void JoinRoom(string roomName)
    //{
    //    // Các người chơi khác sẽ tham gia lại vào phòng chơi với tên phòng chơi mới
    //    PhotonNetwork.JoinRoom(roomName);
    //}
    #endregion
}
