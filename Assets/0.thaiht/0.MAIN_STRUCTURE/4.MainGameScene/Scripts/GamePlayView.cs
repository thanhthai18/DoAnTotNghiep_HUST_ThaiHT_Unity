using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class GamePlayView : View
    {
        public TabPlayerInfo tabPlayerInfo;
        public TextMeshProUGUI txtTimeCounting;
        public LeaderBoardEndGame leaderBoardEndGameView;
        public Button btnOptions;
        public UIView viewOptions;
        public Button btnQuitGame;
        public Button btnSound, btnMusic;
        public UIButton btnClose;

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
            btnOptions.onClick.AddListener(OpenViewOptions);
            btnQuitGame.onClick.AddListener(QuitGame);
            btnClose.onClickEvent.AddListener(CloseViewOptions);
            viewOptions.InstantHide();
        }
        public void OpenViewOptions()
        {
            viewOptions.Show();
        }
        public void CloseViewOptions()
        {
            viewOptions.Hide();
        }
        public void QuitGame()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.Disconnect();
            this.Wait(0.5f, () =>
            {
                SceneManager.LoadScene(SceneGame.SelectModeScene);
            });
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
            txtTimeCounting.text = "Time: " + time + "s";
        }

        public void ShowLeaderBoardEndGame(int[] listScore)
        {
            leaderBoardEndGameView.SpawnElement(tabPlayerInfo.listHolderPlayerIconInTab, listScore);


            leaderBoardEndGameView.GetComponent<UIView>().Show();
        }

        private void OnDestroy()
        {
            GamePlayController.OnGetKey -= GamePlayController_OnGetKey;
        }
    }
}
