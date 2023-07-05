using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;


namespace thaiht20183826
{
    public class RankModeController : MonoBehaviour
    {
        public static RankModeController instance;
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
        }


        private void OnEnable()
        {
            //lobbyRoomView.btnCreateRoom.onClick.AddListener(OnClickBtnCreateRoom);
            //lobbyRoomView.btnJoinRoom.onClick.AddListener(OnClickBtnJoinRoom);
            //roomView.btnBack.onClick.AddListener(OnLeaveRoomButton);
            //roomView.btnStartGame.onClick.AddListener(OnStartGameButton);
            //GlobalController.ActionOnUpdatedGlobalLeaderboard += OnUpdatedGlobalLeaderboard;
            //NetworkManager.ActionOnConnectedToMaster += HandleOnConnectedToMaster;
            //NetworkManager.ActionOnRoomListUpdate += HandleOnRoomListUpdate;
            //NetworkManager.ActionOnJoinedLobby += HandleOnJoinedLobby;
            //NetworkManager.ActionOnLeftLobby += HandleOnLeftLobby;
            //NetworkManager.ActionOnCreateRoomFailed += HandleOnCreateRoomFailed;
            //NetworkManager.ActionOnJoinRoomFailed += HandleOnJoinRoomFailed;
            //NetworkManager.ActionOnJoinedRoom += HandleOnJoinedRoom;
            //NetworkManager.ActionOnCreateRoom += HandleOnCreatedRoom;
            //NetworkManager.ActionOnLeftRoom += HandleOnLeftRoom;
            //NetworkManager.ActionOnPlayerEnterRoom += HandleOnPlayerEnteredRoom;
            //NetworkManager.ActionOnPLayerLeftRoom += HandleOnPlayerLeftRoom;
            //NetworkManager.ActionOnMasterClientSwitched += HandleOnMasterClientSwitched;
            //NetworkManager.ActionOnPlayerPropertiesUpdate += HandleOnPlayerPropertiesUpdate;

        }



        private void OnDisable()
        {
            //lobbyRoomView.btnCreateRoom.onClick.RemoveAllListeners();
            //lobbyRoomView.btnJoinRoom.onClick.RemoveAllListeners();
            //roomView.btnBack.onClick.RemoveAllListeners();
            //roomView.btnStartGame.onClick.RemoveAllListeners();
            //GlobalController.ActionOnUpdatedGlobalLeaderboard -= OnUpdatedGlobalLeaderboard;
            //NetworkManager.ActionOnConnectedToMaster -= HandleOnConnectedToMaster;
            //NetworkManager.ActionOnRoomListUpdate -= HandleOnRoomListUpdate;
            //NetworkManager.ActionOnJoinedLobby -= HandleOnJoinedLobby;
            //NetworkManager.ActionOnLeftLobby -= HandleOnLeftLobby;
            //NetworkManager.ActionOnCreateRoomFailed -= HandleOnCreateRoomFailed;
            //NetworkManager.ActionOnJoinRoomFailed -= HandleOnJoinRoomFailed;
            //NetworkManager.ActionOnJoinedRoom -= HandleOnJoinedRoom;
            //NetworkManager.ActionOnCreateRoom -= HandleOnCreatedRoom;
            //NetworkManager.ActionOnLeftRoom -= HandleOnLeftRoom;
            //NetworkManager.ActionOnPlayerEnterRoom -= HandleOnPlayerEnteredRoom;
            //NetworkManager.ActionOnPLayerLeftRoom -= HandleOnPlayerLeftRoom;
            //NetworkManager.ActionOnMasterClientSwitched -= HandleOnMasterClientSwitched;
            //NetworkManager.ActionOnPlayerPropertiesUpdate -= HandleOnPlayerPropertiesUpdate;

        }

    }
}
