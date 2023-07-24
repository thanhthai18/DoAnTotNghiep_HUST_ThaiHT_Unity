using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class SpeedItem : MonoBehaviour
    {
        [SerializeField] int id;
        [SerializeField] float speedRatioScale;
        [SerializeField] float timeApply;
        [SerializeField] float timeDestroy;


        //public static event Action<float, float, Sprite> ActionOnChangeSpeed;
    
    
        void Awake()
        {
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

        public (float, float, int) OnTriggerItem()
        {
            //ActionOnChangeSpeed?.Invoke(speedRatioScale, timeApply, spriteRenderer.sprite);
            Destroy(gameObject, 0.1f);
            return (speedRatioScale, timeApply, id);
        }
    
    }
    
}
