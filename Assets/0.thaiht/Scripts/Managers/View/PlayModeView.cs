using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayModeView : View
{
    [SerializeField] Button btnBack;
    [SerializeField] Button btnRankMode;
    [SerializeField] Button btnTraningMode;
    [SerializeField] Button btnRoomMode;

    public override void Initialize()
    {       
        btnBack.onClick.AddListener(() =>
        {
            LoaderSystem.Loading(true);
            this.Wait(0.2f, () =>
            {
                LoaderSystem.Loading(false);
                gameObject.SetActive(false);
            });
        });

        btnRankMode.onClick.AddListener(() => { OpenMode(btnRankMode, 0); });
        btnTraningMode.onClick.AddListener(() => { OpenMode(btnTraningMode, 1); });
        btnRoomMode.onClick.AddListener(() => { OpenMode(btnRoomMode, 2); });
    }

    public void OpenMode(Button btn, int indexMode)
    {
        LoaderSystem.Loading(true);
        this.Wait(0.5f, () => { LoaderSystem.Loading(false); });
        btn.AnimButton(-1);
        switch (indexMode)
        {
            case 0:
                OpenRankMode();
                break;
            case 1:
                OpenTrainingMode();
                break;
            case 2:
                OpenRoomMode();
                break;
            default:
                break;
        }
    }

    public void OpenRankMode()
    {
        Debug.Log("Open Rank Mode");
    }
    public void OpenTrainingMode()
    {
        Debug.Log("Open Training Mode");
    }
    public void OpenRoomMode()
    {
        Debug.Log("Open Room Mode");
    }
}


