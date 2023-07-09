using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class Ball : MonoBehaviour
    {
        public Rigidbody2D rig;
        CircleCollider2D col;
    
    
        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            col = GetComponent<CircleCollider2D>();
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



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bien"))
            {
                ReSpawn();
            }
        }

        public void ReSpawn() 
        {
            rig.velocity = Vector2.zero;
            col.enabled = false;
            var beginSCale = transform.localScale;
            transform.DOScale(0, 0.1f).OnComplete(() =>
             {
                 transform.position = Vector3.zero;
                 transform.DOScale(beginSCale, 0.1f).OnComplete(() =>
                 {
                     col.enabled = true;
                 });
             });
        }
    }
}
