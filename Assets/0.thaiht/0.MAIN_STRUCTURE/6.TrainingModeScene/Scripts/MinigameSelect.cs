using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace thaiht20183826
{
    public class MinigameSelect : MonoBehaviour
    {
        [HideInInspector] public UIButton selfButton;
        public MinigameSceneEnum minigame;


        private void Awake()
        {
            selfButton = GetComponent<UIButton>();
        }
        private void OnEnable()
        {
            selfButton.onClickEvent.AddListener(OnSelectMinigame);
        }
        private void OnDisable()
        {
            selfButton.onClickEvent.RemoveListener(OnSelectMinigame);
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

    }
}
