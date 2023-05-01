using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LobbyRoomView : View
{
    [SerializeField] public Button btnBack;
    [SerializeField] public TextMeshProUGUI txtMessage;
    [SerializeField] public Button btnCreateRoom;
    [SerializeField] public Button btnJoinRoom;
    [SerializeField] public TMP_InputField inputRoomName;
    [SerializeField] public GameObject contentSlot;

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
