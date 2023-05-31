using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class TabPlayerInfo : MonoBehaviour
    {
        [SerializeField] Image imgBtnTab;
        [SerializeField] RectTransform selfRectTransform;
        [SerializeField] Button btnTab;
        private Vector2 archorPosHide;
        private Vector2 archorPosShow;

        private bool isShow;

    
    
        void Awake()
        {
            selfRectTransform = GetComponent<RectTransform>();
            transform.GetComponentAtPath("btnTabPlayer/imgTabPlayer", out imgBtnTab);
            transform.GetComponentAtPath("btnTabPlayer", out btnTab);
            archorPosHide = new Vector2(selfRectTransform.sizeDelta.x * 0.4f, 0);
            archorPosShow = new Vector2(-selfRectTransform.sizeDelta.x / 2, 0);
        }
        #region SUBSCRIBE
        private void OnEnable()
        {
        
        }
        private void OnDisable()
        {
        
        }
        #endregion
    
        void Start()
        {
            isShow = false;
            Hide();
            btnTab.onClick.AddListener(OnTab);
        }

        public void OnTab()
        {
            if (isShow)
            {
                isShow = false;
                Hide();
            }
            else
            {
                isShow = true;
                Show();
            }
        }

        private void Show()
        {
            btnTab.interactable = false;
            selfRectTransform.DOKill();
            selfRectTransform.DOAnchorPos(archorPosShow, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                imgBtnTab.transform.localScale = new Vector3(-1,1,1);
                btnTab.interactable = true;
            });
        }
        private void Hide()
        {
            selfRectTransform.DOKill();
            btnTab.interactable = false;
            selfRectTransform.DOAnchorPos(archorPosHide, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                imgBtnTab.transform.localScale = Vector3.one;
                btnTab.interactable = true;
            });
        }

    
    }
}
