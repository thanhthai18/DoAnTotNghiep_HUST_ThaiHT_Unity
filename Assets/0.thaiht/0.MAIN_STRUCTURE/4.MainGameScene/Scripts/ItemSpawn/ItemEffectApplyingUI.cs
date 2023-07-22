using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class ItemEffectApplyingUI : MonoBehaviour
    {
        [SerializeField] Image imgItemEffect;
        [SerializeField] Image fillImg;
    
    
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

        public void Init(Sprite spriteImg)
        {
            imgItemEffect.sprite = spriteImg;
            fillImg.sprite = spriteImg;
        }

        public void Fill(float value)
        {
            value = 1 - value;
            if (value > 1)
            {
                value = 1;
            }
            fillImg.fillAmount = value;
        }

    }
}
