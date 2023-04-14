using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuController : StaticInstance<MenuController>
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

    private void Start()
    {
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
        btnLogout.onClick.AddListener(() => SceneManager.LoadScene("LoginScene"));

        overviewProfileData = new OverviewProfileData
        {
            displayName = GlobalValue.playerName,
            displayRank = $"Rank: {"#1"}",
            displayScore = $"Score: {"3000"}",
        };

        LoadOverviewData();



        /*---Button Play (Select Mode)---*/
        btnPlay.onClick.AddListener(ClickButtonPlayMode);


    }

    public void LoadOverviewData()
    {
        txtDisplayName.text = overviewProfileData.displayName;
        txtDisplayRank.text = overviewProfileData.displayRank;
        txtDisplayScore.text = overviewProfileData.displayScore;
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
            ViewManager.Show<PlayModeView>();
            LoaderSystem.Loading(false);
        });
    }
}




public class OverviewProfileData
{
    public string displayName;
    public string displayRank;
    public string displayScore;
}
