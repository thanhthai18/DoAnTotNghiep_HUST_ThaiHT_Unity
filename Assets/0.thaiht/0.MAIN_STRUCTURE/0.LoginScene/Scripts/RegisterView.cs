using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class RegisterView : View
    {
        [SerializeField] Button btnClose;
        [SerializeField] Button btnConfirmRegister;

        [Header("Register")]
        [SerializeField] TMP_InputField inputUsernameRegister;
        [SerializeField] TMP_InputField inputEmailRegister;
        [SerializeField] TMP_InputField inputPasswordRegister;
        [SerializeField] TextMeshProUGUI txtMessageRegister;

        public override void Initialize()
        {
            txtMessageRegister.text = "";
            btnConfirmRegister.onClick.AddListener(RegisterUser);
            btnClose.onClick.AddListener(() => ViewManager.Show<LoginView>());
        }

        public void RegisterUser()
        {
            var request = new RegisterPlayFabUserRequest
            {
                DisplayName = inputUsernameRegister.text,
                Email = inputEmailRegister.text,
                Password = inputPasswordRegister.text,

                RequireBothUsernameAndEmail = false,
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucces, OnRegisterError);
        }
        private void OnRegisterSucces(RegisterPlayFabUserResult result)
        {
            txtMessageRegister.ShowMessageText("Dang ky thanh cong!", true);
        }
        private void OnRegisterError(PlayFabError error)
        {
            txtMessageRegister.ShowMessageText(error.ErrorMessage, false);
            Debug.Log(error.GenerateErrorReport());
        }


    }
}