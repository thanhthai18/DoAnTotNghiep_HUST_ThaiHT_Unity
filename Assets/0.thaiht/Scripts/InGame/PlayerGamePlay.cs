using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace thaiht20183826
{


    public class PlayerGamePlay : MonoBehaviourPunCallbacks
    {


        [Space(10)]
        [Header("Move Info")]
        public float moveSpeed;
        public float dragRigidbody;


        [Space(10)]
        [Header("Components")]
        public Rigidbody2D rig;


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




        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
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
            rig.drag = dragRigidbody;
            isCanControl = true;
        }

        private void Update()
        {
            if (isCanControl)
            {
                if (id_Character == 0)
                {
                    Move();

                    if (Input.GetKeyDown(KeyCode.A))
                    {

                        DashSkill();

                    }


                    if (isDashing)
                    {
                        //rig.velocity = dashingDir.normalized * dashingVelocity;
                        //rig.AddForce(dashingDir.normalized * 0.5f, ForceMode2D.Impulse);
                        newPositionDash = Vector3.Lerp(dashOrigin, dashDestination, DashCurve.Evaluate(dashTimer / dashingTime));
                        dashTimer += Time.deltaTime;
                        rig.MovePosition(newPositionDash);
                        //transform.position = Vector2.Lerp(transform.position, newPositionDash, dashTimer);
                        return;
                    }
                }
            }


        }

        [PunRPC]
        public void InitPlayer(Player player, Character data)
        {
            //Player
            photonPlayer = player;
            id_Photon = player.ActorNumber;
            GamePlayController.instance.listPlayersGamePlay[id_Photon - 1] = this;
            if (!photonView.IsMine)
            {
                rig.isKinematic = true;
            }

            //Character
            id_Character = data.id;
            moveSpeed = data.moveSpeed;
            dashingTime = data.dashingTime;
            dashDistance = data.dashDistance;
            myTypeSkill = data.mySkill;
            rig.mass = data.weight;
            myAvatar = data.characterSprite;
        }


        void Move()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            // Vận tốc sẽ liên tục tăng
            // Được kiềm lại bằng Linear Drag của rigidbody2D ( có thể thay thế việc này bằng code chay)

            if (rig.velocity.magnitude < 2) //Tăng tốc lúc đầu sẽ dễ dàng hơn
            {
                x *= 2;
                y *= 2;
            }
            rig.velocity += new Vector2(x, y) * moveSpeed * Time.deltaTime;
        }


        void DashSkill()
        {
            if (canDash)
            {
                Debug.Log("Dash");
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


                playerFeedback_Dash.PlayFeedbacks();

                StartCoroutine(StopDashing());
            }
        }

        private IEnumerator StopDashing()
        {
            yield return new WaitForSeconds(dashingTime);
            isDashing = false;
            canDash = true;
            dashTimer = 0;
            playerFeedback_Dash.StopFeedbacks();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Rigidbody2D otherRigidbody = collision.collider.GetComponent<Rigidbody2D>();

                if (otherRigidbody != null)
                {
                    Debug.Log("push");
                    Vector2 pushDirection = otherRigidbody.transform.position - transform.position;
                    pushDirection = pushDirection.normalized;

                    otherRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                }
            }
        }


    }
}