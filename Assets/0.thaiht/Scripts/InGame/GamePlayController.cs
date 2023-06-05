using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class GamePlayController : MonoBehaviourPunCallbacks
    {
        public static GamePlayController instance;

        [SerializeField] GamePlayView gamePlayView;
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
            MapController.OnOutAreaMap += RevivalPlayer;
        }
        private void OnDisable()
        {
            base.OnDisable();
            MapController.OnOutAreaMap -= RevivalPlayer;
        }
        #endregion

        void Start()
        {
            listPlayersGamePlay = new List<PlayerGamePlay>(PhotonNetwork.PlayerList.Length);
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
        public void RevivalPlayer(PlayerGamePlay playerGamePlayer)
        {
            playerGamePlayer.isCanControl = false;
            var beginScale = playerGamePlayer.transform.localScale;
            playerGamePlayer.transform.DOScale(0, 0.1f).OnComplete(() =>
            {
                playerGamePlayer.transform.position = Vector3.zero;
                playerGamePlayer.transform.DOScale(beginScale, 0.1f).OnComplete(() => playerGamePlayer.isCanControl = true);
            });
        }

        
        public void SpawnPlayer()
        {
            Debug.Log($"Call InitPlayer + {GlobalValue.indexCharacterTransfer}");
            PlayerGamePlay playerGamePlay = PhotonNetwork.Instantiate(playerGamePlayPrefabPath, Vector2.zero, Quaternion.identity).GetComponent<PlayerGamePlay>();
            playerGamePlay.photonView.RPC("InitPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer, dataCharacterScriptableObj.listCharacter[GlobalValue.indexCharacterTransfer]);
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