using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class GamePlayController : StaticInstance<GamePlayController>
    {
        public List<PlayerGamePlay> listPlayerGamePlay = new List<PlayerGamePlay>();


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


    }
}
