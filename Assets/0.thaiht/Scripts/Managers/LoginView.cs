using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;
using System;
using UnityEngine.SceneManagement;

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
        };

        
        
        //new LoaderSystem.Load();
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucces, OnLoginError);
    }

    private void OnLoginSucces(LoginResult result)
    {
        txtMessageLogin.ShowMessageText("Dang nhap thanh cong!", true);
        StartCoroutine(LoadNextScene());
    }

    private void OnLoginError(PlayFabError error)
    {
        txtMessageLogin.ShowMessageText(error.ErrorMessage, false);
    }

    private IEnumerator LoadNextScene()
    {
        using (new LoaderSystem.Load())
        {
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       
    }
}
