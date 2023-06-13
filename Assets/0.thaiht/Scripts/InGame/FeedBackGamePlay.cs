using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class FeedBackGamePlay : MonoBehaviour
    {
        public static FeedBackGamePlay instance;
        public Dictionary<string, MMF_Player> dicFeedback = new Dictionary<string, MMF_Player>();
    
    
        void Awake()
        {
            instance = this;
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
            dicFeedback.Add("Flicker", transform.GetComponentAtPath<MMF_Player>("Flicker"));

        }

        public void PlayFlicker()
        {
            dicFeedback["Flicker"].PlayFeedbacks();
        }

    
    }
}
