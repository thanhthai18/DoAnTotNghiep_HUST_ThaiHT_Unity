using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class PopupOptions : MonoBehaviour
    {
        private UIView selfView;
        public Button btnQuitGame;
        public Button btnSound;
        public Button btnMusic;
        public UIButton btnContinue;

        private void Awake()
        {
            selfView = GetComponent<UIView>();
        }
        private void Start()
        {
            btnQuitGame.onClick.AddListener(QuitGame);
            btnContinue.onClickEvent.AddListener(Hide);
        }

        public void QuitGame()
        {
            LoaderSystem.Loading(true);
            this.Wait(0.5f, () =>
            {
                LoaderSystem.Loading(false);
                AudioController.Instance.PlayBackgroundMusicCommon();
                if (GlobalValue.currentModeGame == ModeGame.TrainingMode)
                {
                    SceneManager.LoadScene(SceneGame.TrainingModeScene);
                }
                else
                {
                    if (PhotonNetwork.InRoom)
                    {
                        PhotonNetwork.LeaveRoom();
                    }
                    if (PhotonNetwork.InLobby)
                    {
                        PhotonNetwork.LeaveLobby();
                    }
                    SceneManager.LoadScene(SceneGame.SelectModeScene);
                }
            });
        }
        public void Show()
        {
            selfView.Show();
        }
        public void Hide()
        {
            selfView.Hide();
        }

    }
}
