using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace thaiht20183826
{


    public class PlayerGamePlay : MonoBehaviourPunCallbacks//, IPunObservable
    {


        [Space(10)]
        [Header("Move Info")]
        public float moveSpeed;
        public float dragRigidbody;


        [Space(10)]
        [Header("Components")]
        public Rigidbody2D rig;
        public CharacterScript selfCharacterScript;
        public BoxCollider2D col;

        [Space(10)]
        [Header("Dash Info")]
        [SerializeField] private float dashingTime = 0.25f;
        private Vector2 dashingDir;
        private bool isDashing;
        private bool canDash = true;
        [SerializeField] private float dashDistance = 6f;
        Vector2 dashDestination;
        Vector2 dashOrigin;
        float dashTimer;
        Vector2 newPositionDash;
        public AnimationCurve DashCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        [SerializeField] private MMF_Player playerFeedback_Dash;
        [SerializeField] private MMFeedbackParticles playerFeedback_Dash_Particle;
        public float pushForce;

        [Space(10)]
        [Header("General")]
        public Player photonPlayer;
        public bool isCanControl;
        [SerializeField] public int id_Photon;
        [SerializeField] SkillCharacter myTypeSkill;
        [SerializeField] int id_Character;
        public Sprite myAvatar;
        public int countDead;
        public int countHeart;
        Vector3 beginScale;
        public int scoreRank;
        private Vector3 networkPosition;
        private Vector3 spawnPos;

        public static event Action<PlayerGamePlay> OnPlayerOutAreaMap;
        public static event Action<PlayerGamePlay> OnPlayerDaHoiSinh;
        public static event Action<int, int> OnSetCountScoreLifePlayer;
        public static event Action OnPlayerLostByHeart;



        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            col = GetComponent<BoxCollider2D>();
            selfCharacterScript = GetComponent<CharacterScript>();
            playerFeedback_Dash_Particle = playerFeedback_Dash.GetComponent<MMFeedbackParticles>();
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
            SetupPlayerTrainingMode();
            rig.drag = dragRigidbody;
            isCanControl = true;
            beginScale = transform.localScale;
            selfCharacterScript.AnimIdle();
        }

        private void Update()
        {
            if (photonView != null)
            {
                if (!photonView.IsMine)
                {
                    return;
                }
            }

            if (isCanControl)
            {
                Move();

                if (Input.GetKeyDown(KeyCode.Space))
                {

                    DashSkill();

                }


                if (isDashing)
                {
                    //rig.velocity = dashingDir.normalized * dashingVelocity;
                    rig.AddForce(dashingDir.normalized * dashDistance, ForceMode2D.Impulse);
                    //newPositionDash = Vector3.Lerp(dashOrigin, dashDestination, DashCurve.Evaluate(dashTimer / dashingTime));
                    dashTimer += Time.deltaTime;
                    //rig.MovePosition(newPositionDash);
                    //transform.position = Vector2.Lerp(transform.position, newPositionDash, dashTimer);
                    //rig.AddForce(new Vector2(x, y) * moveSpeed * 3, ForceMode2D.Force);
                    return;
                }
            }
        }
        public void SetupPlayerTrainingMode()
        {
            if (GlobalValue.currentModeGame == ModeGame.TrainingMode)
            {
                PhotonNetwork.OfflineMode = true;
                Destroy(GetComponent<PhotonRigidbody2DView>());
                Destroy(GetComponent<PhotonTransformView>());
                Destroy(GetComponent<PhotonView>());
                var data = GlobalController.Instance.scriptableDataCharacter.listCharacter[GlobalValue.indexCharacterTransfer];
                if (data != null)
                {
                    id_Character = data.id;
                    moveSpeed = data.moveSpeed;
                    dashingTime = data.dashingTime;
                    dashDistance = data.dashDistance;
                    myTypeSkill = data.mySkill;
                    rig.mass = data.weight;
                    myAvatar = data.characterSprite;
                }
            }
            else
            {
                PhotonNetwork.OfflineMode = false;
            }
        }

        [PunRPC]
        public void InitPlayer(Player player, int indexDataCharacter)
        {
            //Player
            countDead = 0;
            countHeart = GlobalValue.LIFE_HEART_RANK_MODE;
            spawnPos = transform.position;
            photonPlayer = player;
            id_Photon = player.ActorNumber;
            //GamePlayController.instance.listPlayersGamePlay[id_Photon - 1] = this;
            GamePlayController.instance.listPlayersGamePlay.Add(this);
            if (!photonView.IsMine)
            {
                rig.isKinematic = true;
            }
            else
            {
                scoreRank = GlobalController.Instance.GetRankScorePlayer(player.NickName);
            }

            //Character
            var data = GamePlayController.instance.dataCharacterScriptableObj.listCharacter[indexDataCharacter];
            if (data != null)
            {
                id_Character = data.id;
                moveSpeed = data.moveSpeed;
                dashingTime = data.dashingTime;
                dashDistance = data.dashDistance;
                myTypeSkill = data.mySkill;
                rig.mass = data.weight;
                myAvatar = data.characterSprite;
            }


            //Tab
            Invoke(nameof(DelaySpawnPlayerTab), 0.5f);
        }
        void DelaySpawnPlayerTab()
        {
            GamePlayController.instance.gamePlayView.tabPlayerInfo.SpawnPlayerTab(id_Photon, photonPlayer.NickName, myAvatar, GlobalValue.currentModeGame);
        }


        void Move()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            if (!isDashing && isCanControl)
            {
                if (x != 0 || y != 0)
                {
                    if (!selfCharacterScript.IsAnimRun())
                    {
                        if (!PhotonNetwork.OfflineMode)
                        {
                            photonView.RPC(nameof(CallAnimRun), RpcTarget.All);
                        }
                        else
                        {
                            CallAnimRun();
                        }
                    }
                }
                else if (x == 0 && y == 0)
                {
                    if (!selfCharacterScript.IsAnimIdle())
                    {
                        if (!PhotonNetwork.OfflineMode)
                        {
                            photonView?.RPC(nameof(CallAnimIdle), RpcTarget.All);
                        }
                        else
                        {
                            CallAnimIdle();
                        }
                    }
                }
            }
            if (x > 0)
            {
                if (transform.localScale.x != beginScale.x)
                {
                    transform.localScale = beginScale;
                    var childParticleScale = transform.GetChild(0).localScale;
                    transform.GetChild(0).localScale = new Vector3(-childParticleScale.x, childParticleScale.y, childParticleScale.z);
                }
            }
            else if (x < 0)
            {
                if (transform.localScale.x != -beginScale.x)
                {
                    transform.localScale = new Vector3(-1 * beginScale.x, beginScale.y, beginScale.z);
                    var childParticleScale = transform.GetChild(0).localScale;
                    transform.GetChild(0).localScale = new Vector3(-childParticleScale.x, childParticleScale.y, childParticleScale.z);
                }
            }

            // Vận tốc sẽ liên tục tăng
            // Được kiềm lại bằng Linear Drag của rigidbody2D ( có thể thay thế việc này bằng code chay)

            if (rig.velocity.magnitude < 2) //Tăng tốc lúc đầu sẽ dễ dàng hơn
            {
                x *= 2;
                y *= 2;
            }
            //rig.velocity += new Vector2(x, y) * moveSpeed * Time.deltaTime;
            rig.AddForce(new Vector2(x, y) * moveSpeed * 3, ForceMode2D.Force);
            if (GlobalValue.currentModeGame == ModeGame.TrainingMode)
            {
                if (SceneManagerHelper.ActiveSceneName == MinigameSceneEnum.TrainingScene_Minigame0.ToString())
                {
                    transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.53f, 6.37f), Mathf.Clamp(transform.position.y, -5.58f, 4.84f));
                }
            }
        }


        void DashSkill()
        {
            if (canDash)
            {
                Debug.Log("Dash");
                selfCharacterScript.AnimSkill();
                isDashing = true; //trigger dash
                canDash = false;
                dashingDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                if (dashingDir == Vector2.zero)
                {
                    dashingDir = new Vector2(transform.localScale.x, 0f);
                }

                dashTimer = 0;
                dashOrigin = transform.position;
                dashDestination = (Vector2)transform.position + dashingDir.normalized * dashDistance;

                if (!PhotonNetwork.OfflineMode)
                {
                    photonView.RPC(nameof(DashFeedbackEffect), RpcTarget.All, true);
                }
                else
                {
                    DashFeedbackEffect(true);
                }


                StartCoroutine(StopDashing());
            }
        }
        [PunRPC]
        public void DashFeedbackEffect(bool isPlay)
        {
            if (isPlay)
            {
                playerFeedback_Dash.PlayFeedbacks();
                //playerFeedback_Dash_Particle.Play(transform.position);
            }
            else
            {
                playerFeedback_Dash.StopFeedbacks();
                //playerFeedback_Dash_Particle.Stop(transform.position);
            }
        }

        private IEnumerator StopDashing()
        {
            yield return new WaitForSeconds(dashingTime);
            isDashing = false;
            canDash = true;
            dashTimer = 0;
            if (!PhotonNetwork.OfflineMode) //PhotonNetwork.IsConnected
            {
                photonView.RPC(nameof(DashFeedbackEffect), RpcTarget.All, false);
            }
            else
            {
                DashFeedbackEffect(false);
            }
        }

        bool isInMap = true;
        public void HoiSinh()
        {
            if (gameObject != null && this != null)
            {
                var beginScale = transform.localScale;
                isCanControl = false;
                selfCharacterScript.AnimDie();
                FeedBackGamePlay.instance.PlayFlicker();
                if (gameObject != null)
                {
                    GlobalController.Instance.Wait(1.3f, () =>
                    {
                        //    transform.DOMoveZ(transform.position.z, 1.3f).OnComplete(() =>
                        //{
                        //transform.DOScale(0, 0.1f).OnComplete(() =>
                        //{
                        try
                        {
                            transform.position = Vector3.zero;
                            selfCharacterScript.AnimIdle();
                            //transform.DOScale(beginScale, 0.1f).OnComplete(() =>
                            //{
                            col.isTrigger = false;
                            isCanControl = true;
                            OnPlayerDaHoiSinh?.Invoke(this);
                            isInMap = true;
                            //});
                            //});
                        }
                        catch (Exception e) { }


                    });
                }
            }



            //});


        }

        #region Call Anim PUN
        [PunRPC]
        public void CallAnimRun()
        {
            selfCharacterScript.AnimRun();
        }
        [PunRPC]
        public void CallAnimIdle()
        {
            selfCharacterScript.AnimIdle();
        }
        #endregion

        //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //{
        //    if (stream.IsWriting)
        //    {
        //        stream.SendNext(transform.position);
        //        stream.SendNext(rig.position);
        //        stream.SendNext(rig.rotation);
        //        stream.SendNext(rig.velocity);
        //    }
        //    else
        //    {
        //        networkPosition = (Vector3)stream.ReceiveNext();
        //        rig.position = (Vector3)stream.ReceiveNext();
        //        rig.rotation = (float)stream.ReceiveNext();
        //        rig.velocity = (Vector3)stream.ReceiveNext();
        //    }
        //}


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (GlobalValue.currentModeGame == ModeGame.TrainingMode)
            {
                if (collision.gameObject.CompareTag("Ball"))
                {
                    Ball ball = collision.collider.GetComponent<Ball>();
                    Rigidbody2D otherRigidbody = ball.rig;

                    if (otherRigidbody != null && !col.isTrigger)
                    {
                        col.isTrigger = true;

                        Debug.Log("push");
                        Vector2 pushDirection = otherRigidbody.transform.position - transform.position;
                        pushDirection = pushDirection.normalized;
                        if (isDashing)
                        {
                            otherRigidbody.AddForce(pushDirection * pushForce * 3, ForceMode2D.Impulse);
                        }
                        else
                        {
                            otherRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                        }
                        col.isTrigger = false;
                    }
                }
            }
            else if (isInMap)
            {
                if (collision.gameObject.CompareTag("Player"))
                {

                    if (collision.gameObject.GetComponent<PlayerGamePlay>().isInMap)
                    {
                        Rigidbody2D otherRigidbody = collision.collider.GetComponent<Rigidbody2D>();

                        if (otherRigidbody != null && !col.isTrigger)
                        {
                            col.isTrigger = true;

                            Debug.Log("push");
                            Vector2 pushDirection = otherRigidbody.transform.position - transform.position;
                            pushDirection = pushDirection.normalized;
                            if (isDashing)
                            {
                                otherRigidbody.AddForce(pushDirection * pushForce * 3, ForceMode2D.Impulse);
                            }
                            else
                            {
                                otherRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                            }
                            col.isTrigger = false;
                        }

                    }

                }
            }

        }

        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.CompareTag("Player"))
        //    {
        //        if (photonView.IsMine)
        //        {
        //            col.enabled = true;
        //        }
        //    }
        //}

        private void OnTriggerExit2D(Collider2D collision)
        {
            try
            {
                if (gameObject != null && this != null)
                {
                    if (collision.CompareTag("Map"))
                    {
                        if (isInMap)
                        {
                            isInMap = false;
                            col.isTrigger = true;
                            rig.velocity = Vector2.zero;
                            rig.angularVelocity = 0;
                            if (GlobalValue.currentModeGame == ModeGame.RoomMode)
                            {
                                countDead++;
                                OnSetCountScoreLifePlayer?.Invoke(id_Photon, countDead);
                            }
                            else if (GlobalValue.currentModeGame == ModeGame.RankMode)
                            {
                                if (countHeart > 0)
                                {
                                    countHeart--;
                                    OnSetCountScoreLifePlayer?.Invoke(id_Photon, countHeart);
                                    if (countHeart == 0)
                                    {
                                        OnPlayerLostByHeart?.Invoke();
                                    }
                                }
                            }
                            HoiSinh();
                            OnPlayerOutAreaMap?.Invoke(this);
                        }
                    }
                }
            }
            catch (Exception e) { }


        }
    }
}