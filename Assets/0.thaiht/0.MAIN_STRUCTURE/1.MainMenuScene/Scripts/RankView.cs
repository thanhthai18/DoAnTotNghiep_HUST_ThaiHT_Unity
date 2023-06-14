using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankView : View
{
    [SerializeField] private Button btnClose;
    public static RankView instance;

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

    private void OnDisable()
    {
        transform.DOKill();
        transform.localScale = Vector3.zero;
    }
}
