using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace thaiht20183826
{
    public class Ball : MonoBehaviour
    {
        public Rigidbody2D rig;
        CircleCollider2D col;
        public static event Action ActionOnGoal;
        public static event Action ActionOnOutSide;
        [SerializeField] GameObject vfx;
        private SpriteRenderer selfSpriteRenderer;
    
    
        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            col = GetComponent<CircleCollider2D>();
            selfSpriteRenderer = GetComponent<SpriteRenderer>();
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
            transform.position = Vector3.zero;
        }


        bool isOnRespawning = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isOnRespawning)
            {
                if (collision.CompareTag("Bien"))
                {
                    ActionOnOutSide?.Invoke();
                    ReSpawn();
                }
                else if (collision.CompareTag("Goal"))
                {
                    vfx.SetActive(false);
                    selfSpriteRenderer.enabled = false;
                    ActionOnGoal?.Invoke();
                    ReSpawn();
                }
            }
       
        }

        public void ReSpawn() 
        {
            isOnRespawning = true;
            col.enabled = false;
            var beginSCale = transform.localScale;
            this.Wait(0.5f, () =>
            {
                rig.velocity = Vector2.zero;
                transform.DOScale(0, 0.1f).OnComplete(() =>
                {
                    selfSpriteRenderer.enabled = true;
                    transform.position = PosBallRandom();
                    transform.DOScale(beginSCale, 0.1f).OnComplete(() =>
                    {
                        col.enabled = true;
                        isOnRespawning = false;
                        if (!vfx.activeSelf)
                        {
                            vfx.SetActive(true);
                        }
                    });
                });
            });
           
        }
        Vector3 PosBallRandom()
        {
            return new Vector3(Random.Range(-5.79f, 3.8f), Random.Range(-2.84f, 2.84f), 0);
        }
    }
}
