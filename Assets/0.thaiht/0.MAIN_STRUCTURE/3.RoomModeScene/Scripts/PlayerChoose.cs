using Doozy.Runtime.UIManager.Components;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
namespace thaiht20183826
{
    public class PlayerChoose : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI txtPlayerName;

        public Image imgCharacterIcon;
        public Button btnReady;
        public Image imgReadyIcon;
        public Button btnRight;
        public Button btnLeft;
        public TextMeshProUGUI txtScoreRankDisplay;
        public GameObject keyHostIcon;
        public UIButton btnKick;

        [Space]
        private int ownerId;
        private bool isPlayerReady;

        public CharacterData currentCharacter;
        public Player myPlayerPhoton;
        public int myIndexCharacter;

        [SerializeField] int characterIndexRankMode = 0;
        [SerializeField] bool isPlayerOnWaitMatch = false;

        public Hashtable playerProperties = new Hashtable();

        #region UNITY



        public void Start()
        {
            if (GlobalValue.currentModeGame == ModeGame.TrainingMode)
            {
                myIndexCharacter = 0;
                SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[myIndexCharacter]);
                btnLeft.onClick.AddListener(OnClickLeftBtn);
                btnRight.onClick.AddListener(OnClickRightBtn);
            }


        }

        public void ApplyLocalPlayer(bool isApply)
        {
            if (PhotonNetwork.IsConnected)
            {
                if (!isApply)
                {
                    btnReady?.gameObject.SetActive(false);
                    btnLeft.gameObject.SetActive(false);
                    btnRight.gameObject.SetActive(false);
                    //PhotonNetwork.SetPlayerCustomProperties(playerProperties);
                }
                else
                {
                    btnLeft.onClick.AddListener(OnClickLeftBtn);
                    btnRight.onClick.AddListener(OnClickRightBtn);

                    Hashtable initialProps = new Hashtable() { { "isPlayerReady", isPlayerReady }/*, { AsteroidsGame.PLAYER_LIVES, AsteroidsGame.PLAYER_MAX_LIVES }*/ };
                    if (myPlayerPhoton.CustomProperties["isPlayerReady"] == null)
                    {
                        myPlayerPhoton.SetCustomProperties(initialProps);
                    }
                    else
                    {
                        PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                    }

                    btnReady?.onClick.AddListener(() =>
                    {
                        isPlayerReady = !isPlayerReady;
                        SetPlayerReady(isPlayerReady);
                        if (SceneManagerHelper.ActiveSceneName == SceneGame.RoomModeScene)
                        {
                            Hashtable props = new Hashtable() { { "isPlayerReady", isPlayerReady } };
                            if (myPlayerPhoton.CustomProperties["isPlayerReady"] == null)
                            {
                                myPlayerPhoton.SetCustomProperties(props);
                            }
                            else
                            {
                                myPlayerPhoton.CustomProperties["isPlayerReady"] = isPlayerReady;
                                myPlayerPhoton.SetCustomProperties(props);
                            }

                            if (PhotonNetwork.IsMasterClient)
                            {
                                RoomModeController.instance?.LocalPlayerPropertiesUpdated();
                            }
                        }

                    });
                }
            }


        }



        #endregion

        public void Initialize(int playerId, string playerName, int scoreRank, Player player)
        {
            ownerId = playerId;
            txtPlayerName.text = playerName;
            txtScoreRankDisplay.text = scoreRank.ToString();
            myPlayerPhoton = player;

            //currentCharacter = GlobalController.Instance.scriptableDataCharacter.listCharacter[0];
            //SetPlayerCharacter(currentCharacter);

            playerProperties = new Hashtable()
            {
                { "characterIndex", 0}
            };
            if (myPlayerPhoton.CustomProperties["characterIndex"] == null)
            {
                myPlayerPhoton.SetCustomProperties(playerProperties);
            }


            if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
            {
                ApplyLocalPlayer(false);
            }
            else
            {
                ApplyLocalPlayer(true);
            }
        }



        public void SetPlayerReady(bool playerReady)
        {
            btnReady.GetComponent<Image>().color = playerReady ? Color.red : Color.white;
            btnReady.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerReady ? "Cancel" : "Ready";
            imgReadyIcon.enabled = playerReady;
        }
        public void SetPlayerCharacter(CharacterData character)
        {
            currentCharacter = character;
            imgCharacterIcon.sprite = currentCharacter.characterSprite;
            GlobalValue.indexCharacterTransfer = currentCharacter.id;
        }

        public void OnClickLeftBtn()
        {
            if (GlobalValue.currentModeGame == ModeGame.TrainingMode)
            {

                if (myIndexCharacter == 0)
                {
                    myIndexCharacter = GlobalController.Instance.scriptableDataCharacter.listCharacter.Count - 1;
                }
                else
                {
                    myIndexCharacter = myIndexCharacter - 1;
                }
                SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[myIndexCharacter]);

            }
            else
            {
                if ((bool)myPlayerPhoton.CustomProperties["isPlayerReady"])
                {
                    return;
                }
                if ((int)myPlayerPhoton.CustomProperties["characterIndex"] == 0)
                {
                    playerProperties["characterIndex"] = GlobalController.Instance.scriptableDataCharacter.listCharacter.Count - 1;
                }
                else
                {
                    playerProperties["characterIndex"] = (int)myPlayerPhoton.CustomProperties["characterIndex"] - 1;
                }
                myPlayerPhoton.SetCustomProperties(playerProperties);
                SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[(int)playerProperties["characterIndex"]]);
            }

        }
        public void OnClickRightBtn()
        {
            if (GlobalValue.currentModeGame == ModeGame.TrainingMode)
            {
                if (myIndexCharacter == GlobalController.Instance.scriptableDataCharacter.listCharacter.Count - 1)
                {
                    myIndexCharacter = 0;
                }
                else
                {
                    myIndexCharacter = myIndexCharacter + 1;
                }
                SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[myIndexCharacter]);

            }
            else
            {
                if ((bool)myPlayerPhoton.CustomProperties["isPlayerReady"])
                {
                    return;
                }

                if ((int)myPlayerPhoton.CustomProperties["characterIndex"] == GlobalController.Instance.scriptableDataCharacter.listCharacter.Count - 1)
                {
                    playerProperties["characterIndex"] = 0;
                }
                else
                {
                    playerProperties["characterIndex"] = (int)myPlayerPhoton.CustomProperties["characterIndex"] + 1;
                }
                myPlayerPhoton.SetCustomProperties(playerProperties);
                SetPlayerCharacter(GlobalController.Instance.scriptableDataCharacter.listCharacter[(int)playerProperties["characterIndex"]]);
            }

        }
    }
}
