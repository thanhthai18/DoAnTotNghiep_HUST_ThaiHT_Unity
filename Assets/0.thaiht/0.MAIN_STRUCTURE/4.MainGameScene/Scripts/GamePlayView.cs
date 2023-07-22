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
        [SerializeField] public TabPlayerInfo tabPlayerInfo;
        [SerializeField] public TextMeshProUGUI txtTimeCounting;
        [SerializeField] public LeaderBoardEndGame leaderBoardEndGameView;
        [SerializeField] public Button btnOptions;
        //public Button btnQuitGame;
        [SerializeField] public PopupOptions popupOptions;
        [SerializeField] public Button btnSound, btnMusic;
        [SerializeField] public UIButton btnClose;
        [Header("Item Effect")]
        [SerializeField] GameObject panelCurrentEffectItem;
        [SerializeField] ItemEffectApplyingUI itemEffectApplyingUIPrefab;




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
            //btnQuitGame.onClick.AddListener(QuitGame);
        }
        public void OpenViewOptions()
        {
            popupOptions.Show();
        }
        //public void QuitGame()
        //{
        //    PhotonNetwork.LeaveRoom();
        //    PhotonNetwork.LeaveLobby();
        //    //PhotonNetwork.Disconnect();
        //    this.Wait(0.5f, () =>
        //    {
        //        SceneManager.LoadScene(SceneGame.SelectModeScene);
        //    });
        //}

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

        public ItemEffectApplyingUI SpawnItemEffectApplyingUI(Sprite spriteItem)
        {
            Debug.Log("show item effect UI");
            var tmp = Instantiate(itemEffectApplyingUIPrefab, panelCurrentEffectItem.transform);
            tmp.Init(spriteItem);
            return tmp;
        }

        private void OnDestroy()
        {
            GamePlayController.OnGetKey -= GamePlayController_OnGetKey;
        }
    }
}
