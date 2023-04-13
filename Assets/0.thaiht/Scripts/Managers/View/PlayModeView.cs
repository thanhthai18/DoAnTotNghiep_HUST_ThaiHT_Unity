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
        btnBack.onClick.AddListener(() => gameObject.SetActive(false));

    }
    
}
