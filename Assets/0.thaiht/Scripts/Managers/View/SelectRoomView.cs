using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectRoomView : View
{
    [SerializeField] Button btnBack;
    public override void Initialize()
    {
        btnBack.onClick.AddListener(() => SceneManager.LoadScene(SceneGame.MainMenuScene.ToString()));
    }
    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void LoadPreScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
