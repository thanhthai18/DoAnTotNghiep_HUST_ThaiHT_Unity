using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using thaiht20183826;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace thaiht20183826
{
    public class GlobalController : MonoBehaviour
    {
        public static GlobalController Instance;
        public Scene currentScene;
        public DataCharacter scriptableDataCharacter;
        public static event Action ActionOnUpdatedGlobalLeaderboard;
        [SerializeField] PopupDisconnect popupDisconnect;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            Debug.Log("Khoi tao GlobalController");
            Application.targetFrameRate = GlobalValue.DEFAULT_FPS;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            PlayFabController.ActionOnLoadSuccess += SetGlobalValueLeaderboard;

        }



        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            currentScene = arg0;
        }
        void SetGlobalValueLeaderboard(List<PlayFab.ClientModels.PlayerLeaderboardEntry> obj)
        {
            GlobalValue.listPlayerLeaderBoard = obj;
            ActionOnUpdatedGlobalLeaderboard?.Invoke();
        }

        public void ReloadPreviousRoom()
        {
            Debug.Log("reload");
            SceneManager.sceneLoaded += DelayReJoinRoom;
            //if (PhotonNetwork.IsMasterClient)
            {
                //NetworkManager.Instance.ChangeScene(SceneGame.RoomModeScene);
                SceneManager.LoadScene(SceneGame.RoomModeScene);
            }




            //PhotonNetwork.LeaveRoom();
            //NetworkManager.ActionOnConnectedToMaster += MasterClientReJoining;

        }



        void DelayReJoinRoom(Scene arg0, LoadSceneMode arg1)
        {
            try
            {
                //this.Wait(0.5f, () =>
                //{
                if (arg0.name == SceneGame.RoomModeScene)
                {
                    //if (RoomModeController.instance != null)
                    //{
                    PhotonNetwork.CurrentRoom.IsOpen = true;
                    PhotonNetwork.CurrentRoom.IsVisible = true;

                    Invoke(nameof(DelayCallJoin), 0.5f);
                    //}
                }
                LoaderSystem.Loading(false);
                SceneManager.sceneLoaded -= DelayReJoinRoom;

                //});

            }
            catch (System.Exception e)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        void DelayCallJoin()
        {
            AudioController.Instance.PlayBackgroundMusicCommon();
            PlayFabController.GetLeaderboard();
            RoomModeController.instance.HandleOnJoinedRoom();

        }

        public int GetRankScorePlayer(string namePlayer)
        {
            foreach (var value in GlobalValue.listPlayerLeaderBoard)
            {
                if (value.DisplayName == namePlayer)
                {
                    return value.StatValue;
                }
            }
            return 0;
        }

        public void ShowPopupDisconnect()
        {
            popupDisconnect.Show();
        }
        public void HidePopupDisconnect()
        {
            popupDisconnect.Hide();
        }

        public static List<int> GetAddScoreRank(int maxPlayer)
        {
            List<int> listScore = new List<int>();
            switch (maxPlayer)
            {
                case 2:
                    {
                        listScore.Add(Random.Range((int)(0.7f * GlobalValue.ELO_RANK), 1 * GlobalValue.ELO_RANK));
                        listScore.Add(-1 * Random.Range((int)(0.25f * GlobalValue.ELO_RANK), (int)(0.6f * GlobalValue.ELO_RANK)));
                        break;
                    }
                case 3:
                    {
                        listScore.Add(Random.Range((int)(0.7f * GlobalValue.ELO_RANK), 1 * GlobalValue.ELO_RANK));
                        listScore.Add(Random.Range((int)(0.2f * GlobalValue.ELO_RANK), (int)(0.5f * GlobalValue.ELO_RANK)));
                        listScore.Add(-1 * Random.Range((int)(0.25f * GlobalValue.ELO_RANK), (int)(0.6f * GlobalValue.ELO_RANK)));
                        break;
                    }
                case 4:
                    {
                        listScore.Add(Random.Range((int)(0.7f * GlobalValue.ELO_RANK), 1 * GlobalValue.ELO_RANK));
                        listScore.Add(Random.Range((int)(0.3f * GlobalValue.ELO_RANK), (int)(0.7f * GlobalValue.ELO_RANK)));
                        listScore.Add(Random.Range(0, 4));
                        listScore.Add(-1 * Random.Range((int)(0.25f * GlobalValue.ELO_RANK), (int)(0.6f * GlobalValue.ELO_RANK)));
                        break;
                    }
                default:
                    break;
            }
            return listScore;
        }

        private void OnDestroy()
        {
            PlayFabController.ActionOnLoadSuccess -= SetGlobalValueLeaderboard;
        }
    }
}

