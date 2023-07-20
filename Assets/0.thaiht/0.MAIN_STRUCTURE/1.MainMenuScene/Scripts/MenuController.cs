using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;
using Photon.Pun;
using System;

public class MenuController : Singleton<MenuController>
{
    [SerializeField] Button btnRank;
    [SerializeField] Button btnProfile;
    [SerializeField] Button btnGuide;
    [SerializeField] Button btnSettings;
    [SerializeField] Button btnPlay;
    [SerializeField] GameObject panelTool;
    [SerializeField] TextMeshProUGUI txtDisplayName;
    [SerializeField] TextMeshProUGUI txtDisplayRank;
    [SerializeField] TextMeshProUGUI txtDisplayScore;
    [SerializeField] Button btnLogout;
    public OverviewProfileData overviewProfileData;
    public static event Action<OverviewProfileData> ActionOnSetOverviewProfileData;

    private void Start()
    {
        GlobalController.ActionOnUpdatedGlobalLeaderboard += LoadOverviewData;
        PlayFabController.GetLeaderboard();

        /*---Button Tool---*/
        btnRank.onClick.AddListener(() =>
        {
            btnRank.AnimButton(0);
            if (RankView.instance != null)
            {
                if (!RankView.instance.CheckIsActive())
                {
                    ViewManager.ShowWithAnim<RankView>();
                }
            }
            else
            {
                ViewManager.ShowWithAnim<RankView>();
            }
        });
        btnProfile.onClick.AddListener(() =>
        {
            btnProfile.AnimButton(0);
            if (ProfileView.instance != null)
            {
                if (!ProfileView.instance.CheckIsActive())
                {
                    ViewManager.ShowWithAnim<ProfileView>();
                }
            }
            else
            {
                ViewManager.ShowWithAnim<ProfileView>();
            }
        });
        btnGuide.onClick.AddListener(() =>
        {
            btnGuide.AnimButton(0);
            if (GuideView.instance != null)
            {
                if (!GuideView.instance.CheckIsActive())
                {
                    ViewManager.ShowWithAnim<GuideView>();
                }
            }
            else
            {
                ViewManager.ShowWithAnim<GuideView>();
            }
        });
        btnSettings.onClick.AddListener(() =>
        {
            btnSettings.AnimButton(0);
            if (SettingsView.instance != null)
            {
                if (!SettingsView.instance.CheckIsActive())
                {
                    ViewManager.ShowWithAnim<SettingsView>();
                }
            }
            else
            {
                ViewManager.ShowWithAnim<SettingsView>();
            }
        });



        /*---Button Login---*/
        btnLogout.onClick.AddListener(() => { PhotonNetwork.Disconnect(); SceneManager.LoadScene(SceneGame.LoginScene); });


        //Invoke(nameof(LoadOverviewData), 0.1f);

        /*---Button Play (Select Mode)---*/
        btnPlay.onClick.AddListener(ClickButtonPlayMode);


    }



    public void LoadOverviewData()
    {
        string tmpRankPosition = "null";
       
        for (int i = 0; i < GlobalValue.listPlayerLeaderBoard.Count; i++)
        {
            if(GlobalValue.listPlayerLeaderBoard[i].DisplayName == MyPlayerValue.playerName)
            {
                tmpRankPosition = (GlobalValue.listPlayerLeaderBoard[i].Position + 1).ToString();
            }
        }
        overviewProfileData = new OverviewProfileData
        {
            displayName = MyPlayerValue.playerName,
            displayRank = $"Rank: {tmpRankPosition}",
            displayScore = $"Score: {GlobalController.Instance.GetRankScorePlayer(MyPlayerValue.playerName)}",
        };
        txtDisplayName.text = overviewProfileData.displayName;
        txtDisplayRank.text = overviewProfileData.displayRank;
        txtDisplayScore.text = overviewProfileData.displayScore;
        ActionOnSetOverviewProfileData?.Invoke(overviewProfileData);
    }
    public void SetNewNamePlayer(string newName)
    {
        txtDisplayName.text = newName;
    }



    public void ClickButtonPlayMode()
    {
        LoaderSystem.Loading(true);
        //transform.DOMoveZ(transform.position.z, 0.5f).OnComplete(() =>
        //{
        //    ViewManager.Show<PlayModeView>();
        //    LoaderSystem.Loading(false);
        //    blockPanel.SetActive(false);
        //});
        this.Wait(0.5f, () =>
        {
            SceneManager.LoadScene(SceneGame.SelectModeScene);
            LoaderSystem.Loading(false);
        });
    }

    private void OnDestroy()
    {
        GlobalController.ActionOnUpdatedGlobalLeaderboard -= LoadOverviewData;
    }
}




public class OverviewProfileData
{
    public string displayName;
    public string displayRank;
    public string displayScore;
}
