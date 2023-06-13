using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            listPlayersGamePlay = new List<PlayerGamePlay>(new PlayerGamePlay[PhotonNetwork.PlayerList.Length]);
            ChangeState(GamePlayState.IM_IN_GAME);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OnGetKey?.Invoke(KeyCode.Tab);
            }
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
        }
        private void HandleIntro()
        {

        }
        private void HandlePlaying()
        {

        }


        /*-----------------Function-----------------*/

        
        public void SpawnPlayer()
        {
            Debug.Log($"Call InitPlayer + {(int)PhotonNetwork.LocalPlayer.CustomProperties["characterIndex"]}");
            int indexData = (int)PhotonNetwork.LocalPlayer.CustomProperties["characterIndex"];
            var dataSpawn = dataCharacterScriptableObj.listCharacter[indexData];
            playerGamePlayPrefabPath = dataSpawn.characterPrefab.name;
            PlayerGamePlay playerGamePlay = PhotonNetwork.Instantiate(playerGamePlayPrefabPath, Vector2.zero, Quaternion.identity).GetComponent<PlayerGamePlay>();
            playerGamePlay.photonView.RPC("InitPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer, indexData);

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

        /*---------------------------------------*/
    }
}

public enum GamePlayState
{
    IM_IN_GAME,
    INTRO,
    PLAYING,

}