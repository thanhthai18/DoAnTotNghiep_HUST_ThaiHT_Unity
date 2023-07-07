using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class PlayerRank : MonoBehaviour
    {
        public TextMeshProUGUI txtSTT;
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtRankScore;
        [SerializeField] Image imgCup;
        [SerializeField] Sprite[] spritesCup;


        public void InitPlayerRank(string stt, string name, string rankScore)
        {
            txtSTT.text = stt;
            txtName.text = name;
            txtRankScore.text = rankScore;
            imgCup.gameObject.SetActive(true);
            SetupCup(int.Parse(stt));
        }

        void SetupCup(int stt)
        {
            if(stt <= 3)
            {
                imgCup.sprite = spritesCup[stt-1];
            }
            else
            {
                imgCup.gameObject.SetActive(false);
            }
        }
    
    }
}
