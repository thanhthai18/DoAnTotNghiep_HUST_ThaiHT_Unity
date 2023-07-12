using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace thaiht20183826
{
    public class MinigameSelect : MonoBehaviour
    {
        [HideInInspector] public UIButton selfButton;
        public MinigameSceneEnum minigame;
        public TextMeshProUGUI txtHighScore;
        public TextMeshProUGUI txtGlobalHighScore;


        private void Awake()
        {
            selfButton = GetComponent<UIButton>();
            PlayFabController.GetLeaderboardMinigame(minigame);

        }
        private void OnEnable()
        {
            selfButton.onClickEvent.AddListener(OnSelectMinigame);
            switch (minigame)
            {
                case MinigameSceneEnum.None:
                    break;
                case MinigameSceneEnum.TrainingScene_Minigame0:
                    PlayFabController.ActionOnLoadSuccessMinigameSoccer += OnLoadSuccessMinigame;
                    break;
            }

        }



        private void OnDisable()
        {
            selfButton.onClickEvent.RemoveListener(OnSelectMinigame);
            switch (minigame)
            {
                case MinigameSceneEnum.None:
                    break;
                case MinigameSceneEnum.TrainingScene_Minigame0:
                    PlayFabController.ActionOnLoadSuccessMinigameSoccer -= OnLoadSuccessMinigame;
                    break;
            }
        }

        public void OnSelectMinigame()
        {
            if (minigame == MinigameSceneEnum.None)
                return;
            LoaderSystem.Loading(true);
            this.Wait(0.5f, () =>
            {
                LoaderSystem.Loading(false);
                SceneManager.LoadScene(minigame.ToString());
            });
        }
        private void OnLoadSuccessMinigame(List<PlayFab.ClientModels.PlayerLeaderboardEntry> obj)
        {
            if (obj.Count > 0)
            {
                txtGlobalHighScore.text = $"Global high score: {obj[0].StatValue}";
            }
            else
            {
                txtGlobalHighScore.text = $"Global high score: {0}";
            }
            txtHighScore.text = $"High score: {0}";
            for (int i = 0; i < obj.Count; i++)
            {
                if (obj[i].DisplayName == MyPlayerValue.playerName)
                {
                    txtHighScore.text = $"High score: {obj[i].StatValue}";
                }
            }
        }
    }
}
