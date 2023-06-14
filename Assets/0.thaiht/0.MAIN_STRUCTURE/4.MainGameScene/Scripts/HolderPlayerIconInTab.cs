using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class HolderPlayerIconInTab : MonoBehaviour
    {
        public TextMeshProUGUI txtPlayerName;
        public Image imgIconAvatar;
        public Image imgIconModeGame;
        [SerializeField] private Sprite spriteHeart;
        [SerializeField] private Sprite spriteDead;
        public TextMeshProUGUI txtCount;
        public int idPhoton;
        [SerializeField] public bool isHeartIcon;

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

        public void SetImgSpriteModeGame(ModeGame mode)
        {
            if(mode == ModeGame.RoomMode)
            {
                isHeartIcon = false;
                imgIconModeGame.sprite = spriteDead;
            }
            else if(mode == ModeGame.RankMode)
            {
                isHeartIcon = true;
                imgIconModeGame.sprite = spriteHeart;
            }
        }

        public void SetTextCountLife(int count)
        {
            txtCount.text = count.ToString();
        }
    
    }
}
