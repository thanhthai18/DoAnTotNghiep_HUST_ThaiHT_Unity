using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class PopupDisconnect : MonoBehaviour
    {
        [SerializeField] UIView selfView;
        [SerializeField] Button btnReconnect;
        [SerializeField] Button btnExit;
        [SerializeField] TextMeshProUGUI txtMess;


        private void Start()
        {
            btnReconnect.onClick.AddListener(OnClickReConnect);
            btnReconnect.onClick.AddListener(OnClickExitGame);
        }
        public void Show()
        {
            if (selfView.isHidden)
            {
                selfView.Show();
                ShowMess("Disconnected!", Color.red);
            }
        }
        public void Hide()
        {
            if (selfView.isVisible)
            {
                selfView.Hide();
            }
        }
        public void OnClickReConnect()
        {
            NetworkManager.Instance.ConnectToMaster();
            ShowMess("Connecting...", Color.green);
            Invoke(nameof(DelayText), 5);
        }
        public void OnClickExitGame()
        {
            Application.Quit();
        }
        void DelayText()
        {
            if (!NetworkManager.Instance.IsConnected)
            {
                ShowMess("Disconnected!", Color.red);
            }
        }
        public void ShowMess(string content, Color color)
        {
            txtMess.text = content;
            txtMess.color = color;
        }


    }
}
