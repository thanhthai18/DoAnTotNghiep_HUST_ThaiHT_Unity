using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using thaiht20183826;
using UnityEngine;
using UnityEngine.UI;

public class RankView : View
{
    [SerializeField] private Button btnClose;
    public PlayerRank playerRankPrefab;
    public static RankView instance;
    [SerializeField] GameObject content;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public override void Initialize()
    {
        btnClose.onClick.AddListener(() => 
        {
            isActive = false;
            gameObject.SetActive(false); ;
        });
    }

    public bool CheckIsActive()
    {
        return isActive;
    }

    private void OnEnable()
    {
        GlobalController.ActionOnUpdatedGlobalLeaderboard += SetupLeaderboardUI;
        PlayFabController.GetLeaderboard();
    }

    public void SetupLeaderboardUI()
    {
        GlobalValue.listPlayerLeaderBoard.OrderBy(s => s.Position);
        foreach (var value in GlobalValue.listPlayerLeaderBoard)
        {
            var t = Instantiate(playerRankPrefab, content.transform);
            t.InitPlayerRank(value.Position.ToString(), value.DisplayName, value.StatValue.ToString());
        }
    }

    private void OnDisable()
    {
        transform.DOKill();
        transform.localScale = Vector3.zero;
        GlobalController.ActionOnUpdatedGlobalLeaderboard -= SetupLeaderboardUI;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            var tmp = content.transform.GetChild(i);
            Destroy(tmp.gameObject);
        }
    }
}
