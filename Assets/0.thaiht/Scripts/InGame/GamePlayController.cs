using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class GamePlayController : StaticInstance<GamePlayController>
    {
        [SerializeField] GamePlayView gamePlayView;
        public List<PlayerGamePlay> listPlayerGamePlay = new List<PlayerGamePlay>();
        public static event Action<GamePlayState> OnBeforeStateChanged;
        public static event Action<GamePlayState> OnAfterStateChanged;
        public static event Action<KeyCode> OnGetKey;

        public GamePlayState State { get; private set; }
        void Awake()
        {
            base.Awake();
        }
        #region SUBSCRIBE
        private void OnEnable()
        {
            MapController.OnOutAreaMap += RevivalPlayer;
        }
        private void OnDisable()
        {
            MapController.OnOutAreaMap -= RevivalPlayer;
        }
        #endregion

        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OnGetKey?.Invoke(KeyCode.Tab);
            }
        }

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

        public void ChangeState(GamePlayState newState)
        {
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            switch (newState)
            {
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

        private void HandleIntro()
        {

        }
        private void HandlePlaying()
        {

        }


    }
}

public enum GamePlayState
{
    INTRO,
    PLAYING,

}