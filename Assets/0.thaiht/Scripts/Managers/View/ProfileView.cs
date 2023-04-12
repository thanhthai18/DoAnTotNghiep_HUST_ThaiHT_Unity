using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileView : View
{
    [SerializeField] private Button btnClose;
    public static ProfileView instance;

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
            gameObject.SetActive(isActive);
        });
    }

    public bool CheckIsActive()
    {
        return isActive;
    }

    private void OnDisable()
    {
        transform.DOKill();
        transform.DOScale(0, 0.5f);
    }
}
