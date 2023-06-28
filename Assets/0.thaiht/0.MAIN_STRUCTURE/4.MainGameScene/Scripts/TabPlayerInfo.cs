using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class TabPlayerInfo : MonoBehaviourPunCallbacks
    {
        [SerializeField] Image imgBtnTab;
        [SerializeField] RectTransform selfRectTransform;
        [SerializeField] Button btnTab;
        private Vector2 archorPosHide;
        private Vector2 archorPosShow;

        public List<HolderPlayerIconInTab> listHolderPlayerIconInTab = new List<HolderPlayerIconInTab>();
        [SerializeField] HolderPlayerIconInTab holderPlayerIconInTabPrefab;
        [SerializeField] GameObject panelTabPlayer;

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
            PlayerGamePlay.OnSetCountScoreLifePlayer += CallSetTextCountLifePlayer;
        }


        private void OnDisable()
        {
            PlayerGamePlay.OnSetCountScoreLifePlayer -= CallSetTextCountLifePlayer;
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
            selfRectTransform.DOAnchorPos(archorPosShow, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                imgBtnTab.transform.localScale = new Vector3(-1, 1, 1);
                btnTab.interactable = true;
            });
        }
        private void Hide()
        {
            selfRectTransform.DOKill();
            btnTab.interactable = false;
            selfRectTransform.DOAnchorPos(archorPosHide, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                imgBtnTab.transform.localScale = Vector3.one;
                btnTab.interactable = true;
            });
        }

        public void SpawnPlayerTab(int idPlayer, string namePlayer, Sprite imgIconAvatar, ModeGame enumModeGame)
        {
            var tab = Instantiate(holderPlayerIconInTabPrefab, panelTabPlayer.transform);
            tab.idPhoton = idPlayer;
            tab.SetImgSpriteModeGame(enumModeGame);
            tab.txtPlayerName.text = namePlayer;
            tab.imgIconAvatar.sprite = imgIconAvatar;

            listHolderPlayerIconInTab.Add(tab);
            if (tab.isHeartIcon)
            {
                SetTextCountLifePlayer(idPlayer, GlobalValue.LIFE_HEART_RANK_MODE);
            }
            else
            {
                SetTextCountLifePlayer(idPlayer, 0);
            }
        }


        public void SortCountLifeTab()
        {
            if (listHolderPlayerIconInTab[0].isHeartIcon)
            {
                listHolderPlayerIconInTab = listHolderPlayerIconInTab.OrderByDescending(s => int.Parse(s.txtCount.text)).ThenBy(s =>
                    GamePlayController.instance?.GetPlayer(s.idPhoton).scoreRank).ToList();
            }
            else
            {
                listHolderPlayerIconInTab = listHolderPlayerIconInTab.OrderBy(s => int.Parse(s.txtCount.text)).ThenBy(s =>
                    GamePlayController.instance?.GetPlayer(s.idPhoton).scoreRank).ToList();
            }
            listHolderPlayerIconInTab.ForEach(s => s.gameObject.transform.SetAsLastSibling());
        }

        public void CallSetTextCountLifePlayer(int idPlayer, int count)
        {
            photonView.RPC(nameof(SetTextCountLifePlayer), RpcTarget.All, idPlayer, count);
        }
        [PunRPC]
        private void SetTextCountLifePlayer(int idPlayer, int count)
        {
            var playerTab = listHolderPlayerIconInTab.First(s => s.idPhoton == idPlayer);
            playerTab.SetTextCountLife(count);
            SortCountLifeTab();
        }
    }
}
