using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class MinigameSoccerView : View
    {
        public Button btnOptions;
        public TextMeshProUGUI txtTime;
        public TextMeshProUGUI txtScore;
        public PopupOptions popupOptions;
        //public UIButton btnBack;
    
        void Awake()
        {
        
        }
        #region SUBSCRIBE
        private void OnEnable()
        {
            //btnBack.onClickEvent.AddListener(Back);
        }
        private void OnDisable()
        {
        
        }
        #endregion
    


        public void SetTextScore(int score)
        {
            txtScore.text = "Score: " + score;
        }
        public void SetTextTime(int time)
        {
            txtTime.text = "Time: " + time + "s";
        }

        public override void Initialize()
        {

            btnOptions.onClick.AddListener(() => popupOptions.Show());
        }
        //public void Back()
        //{
        //    LoaderSystem.Loading(true);
        //    this.Wait(0.3f, () =>
        //    {
        //        LoaderSystem.Loading(false);
        //        SceneManager.LoadScene(SceneGame.SelectModeScene);
        //    });
        //}
    }
}
