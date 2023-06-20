using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class ElementLeaderBoardEndGame : MonoBehaviour
    {
        public Image imgCup;
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtScoreRank;
        public Image imgHuyHieu;
        public Image imgDeadIcon;
        public TextMeshProUGUI txtDeadCount;
        public Image imgBorder;
        public Image imgAvatar;


        void Awake()
        {
            imgBorder = GetComponent<Image>();
            transform.GetComponentAtPath("imgCup", out imgCup);
            transform.GetComponentAtPath("txtName", out txtName);
            transform.GetComponentAtPath("imgBorderScoreRank/txtScoreRank", out txtScoreRank);
            transform.GetComponentAtPath("imgHuyHieu", out imgHuyHieu);
            transform.GetComponentAtPath("DeadResult", out imgDeadIcon);
            transform.GetComponentAtPath("DeadResult/txtCount", out txtDeadCount);
            transform.GetComponentAtPath("MaskIcon/PlayerIcon", out imgAvatar);
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

        public void Init(int top, int maxPlayer, string namePlayer, Sprite spriteAvatar, int score, string textCountDead = "")
        {
            var data = LeaderBoardEndGame.Instance.listDataElementTop;

            if (textCountDead != "")
            {
                txtDeadCount.text = textCountDead;
            }
            txtName.text = namePlayer;
            txtScoreRank.text = (score >= 0) ? "+" + score.ToString() : score.ToString();
            imgAvatar.sprite = spriteAvatar;
            if (top == 0)
            {
                imgHuyHieu.gameObject.SetActive(true);
            }
            else
            {
                imgHuyHieu.gameObject.SetActive(false);
            }

            if (GlobalValue.currentModeGame == ModeGame.RoomMode)
            {
                imgDeadIcon.gameObject.SetActive(true);
            }
            else
            {
                imgDeadIcon.gameObject.SetActive(false);
            }


            if (maxPlayer - 1 - top == 0)
            {
                //lose
                imgCup.sprite = data[LeaderBoardEndGame.Instance.listDataElementTop.Count - 1].spriteCup;
                imgBorder.sprite = data[LeaderBoardEndGame.Instance.listDataElementTop.Count - 1].spriteBorder;
                txtScoreRank.color = Color.red;
            }
            else if (top < maxPlayer - 1)
            {
                txtScoreRank.color = Color.yellow;
                imgCup.sprite = data[top].spriteCup;
                imgBorder.sprite = data[top].spriteBorder;
            }
        }


    }
}
