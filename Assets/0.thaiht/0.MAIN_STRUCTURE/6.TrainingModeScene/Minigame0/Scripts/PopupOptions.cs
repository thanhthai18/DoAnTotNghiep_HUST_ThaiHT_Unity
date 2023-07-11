using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
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
                SceneManager.LoadScene(SceneGame.TrainingModeScene);
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
