using Doozy.Runtime.UIManager.Components;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class LeaderBoardEndGame : StaticInstance<LeaderBoardEndGame>
    {
        public List<DataElementLeaderBoardEndGame> listDataElementTop = new List<DataElementLeaderBoardEndGame>();
        [SerializeField] ElementLeaderBoardEndGame elementLeaderBoardEndGamePrefab;
        public List<ElementLeaderBoardEndGame> listElementLeaderBoardEndGame = new List<ElementLeaderBoardEndGame>();
        public Transform holderElement;


        public void SpawnElement(List<HolderPlayerIconInTab> listPlayerOnTab, int[] listScoreRank)
        {
            for (int i = 0; i < listPlayerOnTab.Count; i++)
            {
                var element = Instantiate(elementLeaderBoardEndGamePrefab, holderElement);
                element.transform.SetAsLastSibling();

                element.Init(i, listPlayerOnTab.Count, listPlayerOnTab[i].txtPlayerName.text, listPlayerOnTab[i].imgIconAvatar.sprite, listScoreRank[i], listPlayerOnTab[i].txtCount.text);

            }

        }
    }

    [System.Serializable]
    public class DataElementLeaderBoardEndGame
    {
        public Sprite spriteBorder;
        public Sprite spriteCup;
    }
}
