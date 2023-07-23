using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;
using System;
using UnityEngine.SceneManagement;

namespace thaiht20183826
{
    public class LoginView : View
    {
        [SerializeField] Button btnLogin;
        [SerializeField] Button btnOpenRegister;
        [SerializeField] Button btnOpenRecovery;

        [Header("Login")]
        [SerializeField] TMP_InputField inputEmailLogin;
        [SerializeField] TMP_InputField inputPasswordLogin;

        [SerializeField] TextMeshProUGUI txtMessageLogin;






        public override void Initialize()
        {
            txtMessageLogin.text = "";
            btnOpenRegister.onClick.AddListener(() => ViewManager.Show<RegisterView>());
            btnOpenRecovery.onClick.AddListener(() => ViewManager.Show<RecoveryView>());
            btnLogin.onClick.AddListener(Login);
        }

        public void Login()
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = inputEmailLogin.text,
                Password = inputPasswordLogin.text,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                }
            };



            //new LoaderSystem.Load();
            LoaderSystem.Loading(true);


            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucces, OnLoginError);

        }

        private void OnLoginSucces(LoginResult result)
        {
            LoaderSystem.Loading(false);
            txtMessageLogin.ShowMessageText("Dang nhap thanh cong!", true);
            PlayFabController.SetTimeByServer();
            string name = null;
            if (result.InfoResultPayload != null)
            {
                name = result.InfoResultPayload.PlayerProfile.DisplayName;
                MyPlayerValue.playerName = name;
                PlayFabController.GetLeaderboard();
                LoadNextScene();
            }

        }

        private void OnLoginError(PlayFabError error)
        {
            LoaderSystem.Loading(false);
            txtMessageLogin.ShowMessageText(error.ErrorMessage, false);
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}