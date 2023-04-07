using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using System;

public class RecoveryView : View
{
    [Header("Recovery")]
    [SerializeField] TMP_InputField inputEmailRecovery;
    [SerializeField] Button btnRecovery;
    [SerializeField] Button btnClose;
    [SerializeField] TextMeshProUGUI txtMessageRecovery;
    public override void Initialize()
    {
        txtMessageRecovery.text = "";
        btnClose.onClick.AddListener(() => ViewManager.Show<LoginView>());
        btnRecovery.onClick.AddListener(RecoveryUser);
    }

    private void RecoveryUser()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = inputEmailRecovery.text,
            TitleId = "60FF7",
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySucces, OnRecoveryError);
    }

    private void OnRecoverySucces(SendAccountRecoveryEmailResult result)
    {
        txtMessageRecovery.ShowMessageText("Recovery Mail Sent", true);
    }

    private void OnRecoveryError(PlayFabError error)
    {
        txtMessageRecovery.ShowMessageText(error.ErrorMessage, false);
    }
}


