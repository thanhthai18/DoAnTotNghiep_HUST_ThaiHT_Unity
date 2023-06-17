using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class GamePlayView : View
    {
        public TabPlayerInfo tabPlayerInfo;
        public TextMeshProUGUI txtTimeCounting;
        public UIView leaderBoardEndGameView;
    
    
        void Awake()
        {
            
        }
        #region SUBSCRIBE
        private void OnEnable()
        {
        
        }
        private void OnDisable()
        {
        
        }
        #endregion
    
        void Start()
        {
        }

        public override void Initialize()
        {
            GamePlayController.OnGetKey += GamePlayController_OnGetKey;
        }

        private void GamePlayController_OnGetKey(KeyCode obj)
        {
            switch (obj)
            {
                case KeyCode.Tab:
                    tabPlayerInfo.OnTab();
                    break;
                default:
                    break;
            }
        }
        public void SetTextTimeCount(int time)
        {
            txtTimeCounting.text = "Time: " + time +"s";
        }

        public void ShowLeaderBoardEndGame()
        {
            leaderBoardEndGameView.Show();
        }

        private void OnDestroy()
        {
            GamePlayController.OnGetKey -= GamePlayController_OnGetKey;
        }
    }
}
