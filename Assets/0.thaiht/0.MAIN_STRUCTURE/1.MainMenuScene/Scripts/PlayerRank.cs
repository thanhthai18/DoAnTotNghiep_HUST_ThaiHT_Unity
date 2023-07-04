using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace thaiht20183826
{
    public class PlayerRank : MonoBehaviour
    {
        public TextMeshProUGUI txtSTT;
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtRankScore;


        public void InitPlayerRank(string stt, string name, string rankScore)
        {
            txtSTT.text = stt;
            txtName.text = name;
            txtRankScore.text = rankScore;
        }
    
    }
}
