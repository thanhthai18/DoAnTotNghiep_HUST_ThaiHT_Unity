using DG.Tweening;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileView : View
{
    [SerializeField] private Button btnClose;
    public static ProfileView instance;
    [SerializeField] GameObject panelRename;
    [SerializeField] Button btnOpenRename;
    [SerializeField] Button btnConfirmRename;
    [SerializeField] TMP_InputField inputNewName;
    [SerializeField] TextMeshProUGUI txtMessage;
    [SerializeField] Button btnClosePanelRename;
    [SerializeField] TextMeshProUGUI txtDisplayName;
    [SerializeField] TextMeshProUGUI txtDisplayRank;
    [SerializeField] TextMeshProUGUI txtDisplayScore;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        //MenuController.ActionOnSetOverviewProfileData += ShowInfo;
    }
    public override void Initialize()
    {
        btnClose.onClick.AddListener(() =>
        {
            isActive = false;
            gameObject.SetActive(isActive);
        });
      
    }
    private void OnEnable()
    {
        if (MenuController.Instance)
        {
            ShowInfo(MenuController.Instance.overviewProfileData);

        }
        txtMessage.gameObject.SetActive(false);
        btnOpenRename.onClick.AddListener(OpenRenamePanel);
        btnClosePanelRename.onClick.AddListener(CloseRenamePanel);
        btnConfirmRename.onClick.AddListener(Rename);
        panelRename.gameObject.SetActive(false);
    }
    
    public void ShowInfo(OverviewProfileData data)
    {
        txtDisplayName.text = data.displayName;
        txtDisplayRank.text = data.displayRank;
        txtDisplayScore.text = data.displayScore;
    }
    public void UpdateInfo(string newName)
    {
        txtDisplayName.text = newName;
        MenuController.Instance.SetNewNamePlayer(newName);
    }

    public bool CheckIsActive()
    {
        return isActive;
    }
    private void OpenRenamePanel()
    {
        panelRename.gameObject.SetActive(true);
    }
    private void CloseRenamePanel()
    {
        panelRename.gameObject.SetActive(false);
    }
    private void Rename()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = inputNewName.text,
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdated, OnError);
        }
    }

    private void OnError(PlayFabError obj)
    {
        txtMessage.ShowMessageText(obj.ErrorMessage, false);
    }

    private void OnDisplayNameUpdated(UpdateUserTitleDisplayNameResult obj)
    {
        txtMessage.ShowMessageText("Success!", true);
        UpdateInfo(obj.DisplayName);
    }

    private void OnDisable()
    {
        transform.DOKill();
        transform.DOScale(0, 0.5f);
        btnOpenRename.onClick.RemoveListener(OpenRenamePanel);
        btnClosePanelRename.onClick.RemoveListener(CloseRenamePanel);
        btnConfirmRename.onClick.RemoveListener(Rename);
        //MenuController.ActionOnSetOverviewProfileData -= ShowInfo;
    }
}
