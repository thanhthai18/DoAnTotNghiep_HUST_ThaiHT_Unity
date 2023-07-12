using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace thaiht20183826
{
    public class MinigameSoccerController : MonoBehaviour
    {
        private int maxTime;
        [SerializeField] int currentTime;
        [SerializeField] bool isEndGame;
        [SerializeField] MinigameSoccerView minigameSoccerView;
        [SerializeField] int score;
        [SerializeField] Ball ball;
        [SerializeField] ParticleSystem particleGoal;

        private void Start()
        {
            isEndGame = false;
            SpawnPlayer();
            maxTime = GlobalValue.MAX_TIME_MINIGAME_SOCCER;
            score = 0;
            minigameSoccerView.SetTextScore(score);
            Ball.ActionOnGoal += OnGoal;
            Ball.ActionOnOutSide += OnOutSide;
            currentTime = maxTime;
            minigameSoccerView.SetTextTime(currentTime);
            StartCoroutine(StartCountTime());
        }

        private void OnOutSide()
        {
            currentTime -= 2;
            minigameSoccerView.SetTextTime(currentTime);
        }

        private void OnGoal()
        {
            score++;
            currentTime += 2;
            minigameSoccerView.SetTextTime(currentTime);
            minigameSoccerView.SetTextScore(score);
            Instantiate(particleGoal, ball.transform.position, Quaternion.identity);
        }

        public IEnumerator StartCountTime()
        {
            while (!isEndGame)
            {
                yield return new WaitForSeconds(1);
                currentTime--;
                minigameSoccerView.SetTextTime(currentTime);
                if(currentTime <= 0)
                {
                    EndGame();
                }
            }
        }

        public void SpawnPlayer()
        {
            int indexData = GlobalValue.indexCharacterTransfer;
            var dataSpawn = GlobalController.Instance.scriptableDataCharacter.listCharacter[indexData];
            PlayerGamePlay playerGamePlay = Instantiate(dataSpawn.characterPrefab, new Vector3(-3,0,0), Quaternion.identity).GetComponent<PlayerGamePlay>();
        }

        public void EndGame()
        {
            isEndGame = true;
            LoaderSystem.Loading(true);
            Ball.ActionOnGoal -= OnGoal;
            Ball.ActionOnOutSide -= OnOutSide;
            PlayFabController.SubmitScoreMinigame(score, MinigameSceneEnum.TrainingScene_Minigame0);
            this.Wait(1, () =>
            {
                LoaderSystem.Loading(false);
                SceneManager.LoadScene(SceneGame.TrainingModeScene);
            });
        }

    }
}
