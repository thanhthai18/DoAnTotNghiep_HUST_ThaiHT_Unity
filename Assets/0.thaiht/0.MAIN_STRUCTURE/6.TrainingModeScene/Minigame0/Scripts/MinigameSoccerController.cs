using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class MinigameSoccerController : MonoBehaviour
    {
        private int maxTime;
        [SerializeField] int currentTime;
        [SerializeField] bool isEndGame;
        [SerializeField] MinigameSoccerView minigameSoccerView;
        [SerializeField] int score;

        private void Start()
        {
            isEndGame = false;
            maxTime = GlobalValue.MAX_TIME_MINIGAME_SOCCER;
            currentTime = maxTime;
            StartCoroutine(StartCountTime());
        }

        public IEnumerator StartCountTime()
        {
            while (!isEndGame)
            {
                yield return new WaitForSeconds(1);
                currentTime--;
                minigameSoccerView.SetTextTime(currentTime);
                if(currentTime == 0)
                {
                    EndGame();
                }
            }
        }

        public void EndGame()
        {
            isEndGame = true;
        }

    }
}
