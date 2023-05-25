using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public float moveSpeed;
    public float dashForce;

    [Header("Components")]
    public Rigidbody2D rig;
    public Player photonPlayer;

    [Header("Dash")]
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

    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {

            TryDashSkill();
           
        }

        

        if (isDashing)
        {
            //rig.velocity = dashingDir.normalized * dashingVelocity;
            //rig.AddForce(dashingDir.normalized * dashingVelocity , ForceMode2D.Force);
            newPositionDash = Vector3.Lerp(dashOrigin, dashDestination, DashCurve.Evaluate(dashTimer / dashingTime));
            dashTimer += Time.deltaTime;
            rig.MovePosition(newPositionDash);
            //transform.position = Vector2.Lerp(transform.position, newPositionDash, dashTimer);
            return;
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


    void Move()
    {
        float x = Input.GetAxis("Horizontal") /** moveSpeed*/;
        float y = Input.GetAxis("Vertical") /** moveSpeed*/;

        rig.velocity = new Vector3(x, y) * moveSpeed;
        //transform.position += new Vector3(x, y) * Time.deltaTime * moveSpeed;
    }

   
    void TryDashSkill()
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

    
}
