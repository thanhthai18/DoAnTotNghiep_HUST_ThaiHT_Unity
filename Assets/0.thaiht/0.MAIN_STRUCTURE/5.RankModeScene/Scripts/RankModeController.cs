using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace thaiht20183826
{
    public class RankModeController : MonoBehaviour
    {
        public static RankModeController instance;
        [SerializeField] private RankModeView rankModeView;
        [SerializeField] PlayerChoose currentPlayerChoose;
        private Coroutine coroutineCheckMatchSuccess;
        [SerializeField] private DataMapScriptableObj dataMapScriptableObj;

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
            rankModeView.btnBack.onClick.AddListener(OnLeaveLobbyButton);
            rankModeView.btnPlay.onClick.AddListener(OnFindMatch);
            GlobalController.ActionOnUpdatedGlobalLeaderboard += OnUpdatedGlobalLeaderboard;
            //NetworkManager.ActionOnConnectedToMaster += HandleOnConnectedToMaster;
            //NetworkManager.ActionOnRoomListUpdate += HandleOnRoomListUpdate;
            NetworkManager.ActionOnJoinedLobby += HandleOnJoinedLobby;
            NetworkManager.ActionOnLeftLobby += HandleOnLeftLobby;
            NetworkManager.ActionOnCreateRoomFailed += HandleOnCreateRoomFailed;
            NetworkManager.ActionOnJoinRoomFailed += HandleOnJoinRoomFailed;
            NetworkManager.ActionOnJoinedRoom += HandleOnJoinedRoom;
            //NetworkManager.ActionOnCreateRoom += HandleOnCreatedRoom;
            NetworkManager.ActionOnLeftRoom += HandleOnLeftRoom;
            //NetworkManager.ActionOnPlayerEnterRoom += HandleOnPlayerEnteredRoom;
            //NetworkManager.ActionOnPLayerLeftRoom += HandleOnPlayerLeftRoom;
            //NetworkManager.ActionOnMasterClientSwitched += HandleOnMasterClientSwitched;
            NetworkManager.ActionOnPlayerPropertiesUpdate += HandleOnPlayerPropertiesUpdate;

        }



        private void OnDisable()
        {
            rankModeView.btnBack.onClick.RemoveAllListeners();
            rankModeView.btnPlay.onClick.RemoveAllListeners();
            GlobalController.ActionOnUpdatedGlobalLeaderboard -= OnUpdatedGlobalLeaderboard;
            //NetworkManager.ActionOnConnectedToMaster -= HandleOnConnectedToMaster;
            //NetworkManager.ActionOnRoomListUpdate -= HandleOnRoomListUpdate;
            NetworkManager.ActionOnJoinedLobby -= HandleOnJoinedLobby;
            NetworkManager.ActionOnLeftLobby -= HandleOnLeftLobby;
            NetworkManager.ActionOnCreateRoomFailed -= HandleOnCreateRoomFailed;
            NetworkManager.ActionOnJoinRoomFailed -= HandleOnJoinRoomFailed;
            NetworkManager.ActionOnJoinedRoom -= HandleOnJoinedRoom;
            //NetworkManager.ActionOnCreateRoom -= HandleOnCreatedRoom;
            NetworkManager.ActionOnLeftRoom -= HandleOnLeftRoom;
            //NetworkManager.ActionOnPlayerEnterRoom -= HandleOnPlayerEnteredRoom;
            //NetworkManager.ActionOnPLayerLeftRoom -= HandleOnPlayerLeftRoom;
            //NetworkManager.ActionOnMasterClientSwitched -= HandleOnMasterClientSwitched;
            NetworkManager.ActionOnPlayerPropertiesUpdate -= HandleOnPlayerPropertiesUpdate;

        }

        public void HandleOnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
            var p = PhotonNetwork.LocalPlayer;
            currentPlayerChoose.GetComponent<PlayerChoose>().Initialize(p.ActorNumber, p.NickName, GlobalController.Instance.GetRankScorePlayer(p.NickName), p);
            PlayFabController.GetLeaderboard();
            object isPlayerReady, characterIndex;
            if (p.CustomProperties.TryGetValue("isPlayerReady", out isPlayerReady))
            {
                currentPlayerChoose.SetPlayerReady((bool)isPlayerReady);
            }
            if (p.CustomProperties.TryGetValue("characterIndex", out characterIndex))
            {
                currentPlayerChoose.SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[(int)characterIndex]);
            }
        }
        public void HandleOnLeftLobby()
        {
            Debug.Log("LeavedLobby");
            if (SceneManagerHelper.ActiveSceneName == SceneGame.RankModeScene)
            {
                SceneManager.LoadScene(SceneGame.SelectModeScene);
            }
        }
        public void OnLeaveLobbyButton()
        {
            Debug.Log("LeavingLobby");
            PhotonNetwork.LocalPlayer.CustomProperties.Clear();
            PhotonNetwork.LeaveLobby();

        }

        public void OnUpdatedGlobalLeaderboard()
        {
            currentPlayerChoose.txtScoreRankDisplay.text = GlobalController.Instance.GetRankScorePlayer(currentPlayerChoose.txtPlayerName.text).ToString();

        }

        public void HandleOnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log("tao pho");
            object isPlayerReady, characterIndex;
            if (changedProps.TryGetValue("isPlayerReady", out isPlayerReady))
            {
                currentPlayerChoose.SetPlayerReady((bool)isPlayerReady);
            }
            if (changedProps.TryGetValue("characterIndex", out characterIndex))
            {
                currentPlayerChoose.SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[(int)characterIndex]);
                Debug.Log("Update properties character");
            }
        }

        public void HandleOnJoinedRoom()
        {
            Debug.Log("Joined Room" + PhotonNetwork.CurrentRoom.Name);
        }
        public void HandleOnJoinRoomFailed()
        {
            PhotonNetwork.CreateRoom(countRoomCreateRank + "_FindRoom_", new RoomOptions { MaxPlayers = 2, IsVisible = false });
        }
        public void HandleOnCreateRoomFailed()
        {
            countRoomCreateRank++;
            PhotonNetwork.JoinRoom(countRoomCreateRank + "_FindRoom_");
        }

        int countRoomCreateRank = 0;
        public void OnFindMatch()
        {
            countRoomCreateRank = 0;
            if (rankModeView.isCounting)
            {
                StopCoroutine(coroutineCheckMatchSuccess);
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                }
            }
            else
            {
                StartCoroutine(CheckFindMatchSuccess());
                PhotonNetwork.JoinRoom(countRoomCreateRank+"_FindRoom_");
            }
        }

        public void HandleOnLeftRoom()
        {
            PhotonNetwork.JoinLobby();
        }
        IEnumerator CheckFindMatchSuccess()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                int randomIndexMap = Random.Range(0, dataMapScriptableObj.listMapInfo.Count);
                
                

                if (PhotonNetwork.IsMasterClient)
                {
                   
                    if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                    {
                        SetRoomMapProperties(randomIndexMap);
                        NetworkManager.Instance.ChangeScene("MainGameScene");
                    }
                }
            }
          
        }

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
    }
}
