﻿using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace thaiht20183826
{
    public class GamePlayController : MonoBehaviourPunCallbacks
    {
        public static GamePlayController instance;

        [SerializeField] public GamePlayView gamePlayView;
        public static event Action<GamePlayState> OnBeforeStateChanged;
        public static event Action<GamePlayState> OnAfterStateChanged;
        public static event Action<KeyCode> OnGetKey;

        [Header("Player")]
        public List<PlayerGamePlay> listPlayersGamePlay = new List<PlayerGamePlay>();
        public int countPlayerInGame;
        public PlayerGamePlay playerGamePlayPrefab;
        public string playerGamePlayPrefabPath;

        [Header("For State")]
        public int timeToEnd;
        public bool isEndGame;

        [Header("Data")]
        public DataCharacter dataCharacterScriptableObj;

        [Header("General")]
        public float gamePlayTime = 30; // Thời gian trò chơi (đơn vị: giây)
        private float currentTime;
        [SerializeField] private bool isCounting = false;


        public GamePlayState State { get; private set; }
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #region SUBSCRIBE
        private void OnEnable()
        {
            base.OnEnable();
            PlayerGamePlay.OnPlayerOutAreaMap += PlayerGamePlay_OnPlayerOutAreaMap;
            PlayerGamePlay.OnPlayerDaHoiSinh += PlayerGamePlay_OnPlayerDaHoiSinh;
            PlayerGamePlay.OnPlayerLostByHeart += PlayerGamePlay_OnPlayerLostByHeart;
        }


        private void OnDisable()
        {
            base.OnDisable();
            PlayerGamePlay.OnPlayerOutAreaMap -= PlayerGamePlay_OnPlayerOutAreaMap;
            PlayerGamePlay.OnPlayerDaHoiSinh -= PlayerGamePlay_OnPlayerDaHoiSinh;
            PlayerGamePlay.OnPlayerLostByHeart -= PlayerGamePlay_OnPlayerLostByHeart;
        }
        #endregion

        void Start()
        {
            gamePlayTime = 10;
            listPlayersGamePlay = new List<PlayerGamePlay>(new PlayerGamePlay[PhotonNetwork.PlayerList.Length]);
            ChangeState(GamePlayState.IM_IN_GAME);
        }


        /*-----------------State-----------------*/
        public void ChangeState(GamePlayState newState)
        {
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            switch (newState)
            {
                case GamePlayState.IM_IN_GAME:
                    photonView.RPC(nameof(HandleImInGame), RpcTarget.AllBuffered);
                    break;
                case GamePlayState.INTRO:
                    HandleIntro();
                    break;
                case GamePlayState.PLAYING:
                    HandlePlaying();
                    break;
                case GamePlayState.ENDGAME:
                    HandleEndGame();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnAfterStateChanged?.Invoke(newState);

            Debug.Log($"New state: {newState}");
        }

        [PunRPC]
        private void HandleImInGame()
        {
            countPlayerInGame++;
            if (countPlayerInGame == PhotonNetwork.PlayerList.Length)
            {
                SpawnPlayer();
            }
            ChangeState(GamePlayState.INTRO);
        }
        private void HandleIntro()
        {
            ChangeState(GamePlayState.PLAYING);
        }
        private void HandlePlaying()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCountdown(); // Chỉ khởi động đếm ngược từ Master Client
            }
        }
        private void HandleEndGame()
        {
            int[] arrScore = Helpers.GetAddScoreRank(gamePlayView.tabPlayerInfo.listHolderPlayerIconInTab.Count).ToArray();
            photonView.RPC(nameof(GameEndRPC), RpcTarget.All, arrScore);
        }

        /*-----------------Function-----------------*/

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OnGetKey?.Invoke(KeyCode.Tab);
            }


            if (isCounting)
            {
                currentTime -= Time.deltaTime;

                // Gửi thời gian cập nhật cho tất cả các người chơi trong phòng
                photonView.RPC(nameof(UpdateTimeRPC), RpcTarget.All, currentTime);

                // Kiểm tra nếu thời gian kết thúc
                if (currentTime <= 0f)
                {
                    if (!isEndGame)
                    {
                        isEndGame = true;
                        ChangeState(GamePlayState.ENDGAME);
                    }

                }
            }

        }
        public void SpawnPlayer()
        {
            Debug.Log($"Call InitPlayer + {(int)PhotonNetwork.LocalPlayer.CustomProperties["characterIndex"]}");
            int indexData = (int)PhotonNetwork.LocalPlayer.CustomProperties["characterIndex"];
            var dataSpawn = dataCharacterScriptableObj.listCharacter[indexData];
            playerGamePlayPrefabPath = dataSpawn.characterPrefab.name;
            PlayerGamePlay playerGamePlay = PhotonNetwork.Instantiate(playerGamePlayPrefabPath, Vector2.zero, Quaternion.identity).GetComponent<PlayerGamePlay>();
            playerGamePlay.photonView.RPC(nameof(playerGamePlay.InitPlayer), RpcTarget.All, PhotonNetwork.LocalPlayer, indexData);

        }


        public PlayerGamePlay GetPlayer(int playerId)
        {
            return listPlayersGamePlay.First(s => s.id_Photon == playerId);
        }

        private void PlayerGamePlay_OnPlayerOutAreaMap(PlayerGamePlay playerGamePlay)
        {
            //Truyen diem
            //var listTmp = gamePlayView.tabPlayerInfo.listHolderPlayerIconInTab[]
        }
        private void PlayerGamePlay_OnPlayerDaHoiSinh(PlayerGamePlay obj)
        {
            //throw new NotImplementedException();
        }


        private void PlayerGamePlay_OnPlayerLostByHeart()
        {
            Debug.Log("End Game Rank");
        }



        public void StartCountdown()
        {
            currentTime = gamePlayTime;
            isCounting = true;
        }

        [PunRPC]
        private void UpdateTimeRPC(float time)
        {
            currentTime = time;
            gamePlayView.SetTextTimeCount((int)currentTime);
        }


        [PunRPC]
        private void GameEndRPC(int[] arrScore)
        {
            // Thực hiện các hành động sau khi kết thúc trò chơi (ví dụ: hiển thị kết quả cuối cùng, trở về menu, vv.)
            isCounting = false;
            listPlayersGamePlay.ForEach(s => s.isCanControl = false);
            gamePlayView.ShowLeaderBoardEndGame(arrScore);
            GlobalValue.masterClientID = PhotonNetwork.MasterClient.ActorNumber;
            this.Wait(5, () =>
            {
                LoaderSystem.Loading(true);
                GlobalController.Instance.ReloadPreviousRoom();
            });

        }




        //public void OnReloadedSceneWhenReJoin(Scene a, LoadSceneMode b)
        //{
        //    //ViewManager.Show<RoomView>();
        //    Debug.Log("loz me m");
        //    PhotonNetwork.CreateRoom(GlobalValue.previousRoom.Name, new RoomOptions { MaxPlayers = GlobalValue.previousRoom.MaxPlayers }, TypedLobby.Default);
        //    SceneManager.sceneLoaded -= OnReloadedSceneWhenReJoin;
        //}

        /*---------------------------------------*/
    }
}

public enum GamePlayState
{
    IM_IN_GAME,
    INTRO,
    PLAYING,
    ENDGAME,
}