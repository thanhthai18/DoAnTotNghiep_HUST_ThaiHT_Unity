using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectRoomView : View
{
    [SerializeField] Button btnBack;
    [SerializeField] TextMeshProUGUI txtMessage;
    public override void Initialize()
    {
        btnBack.onClick.AddListener(() =>
        {
            LoaderSystem.Loading(true);
            GlobalController.Instance.Wait(0.2f, () =>
            {
                SceneManager.LoadScene(SceneGame.SelectModeScene);
                LoaderSystem.Loading(false);
            });
        });

    }
}
