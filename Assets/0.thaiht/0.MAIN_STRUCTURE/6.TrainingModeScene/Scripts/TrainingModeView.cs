using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class TrainingModeView : View
    {
        [SerializeField] Button btnSound;
        [SerializeField] Button btnMusic;
        [SerializeField] UIButton btnBack;
        [SerializeField] PlayerChoose playerChoose;
    
    
        void Awake()
        {
        
        }
        public override void Initialize()
        {
            btnBack.onClickEvent.AddListener(Back);
        }
        #region SUBSCRIBE
        private void OnEnable()
        {
       
        }
        private void OnDisable()
        {
            btnBack.onClickEvent.RemoveListener(Back);
        }
        #endregion
    
        void Start()
        {
        
        }

        public void Back()
        {
            LoaderSystem.Loading(true);
            this.Wait(0.3f, () =>
            {
                LoaderSystem.Loading(false);
                SceneManager.LoadScene(SceneGame.SelectModeScene);
            });
        }

       
    }
}
