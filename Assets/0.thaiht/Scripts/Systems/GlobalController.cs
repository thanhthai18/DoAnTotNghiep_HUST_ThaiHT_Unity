using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : StaticInstance<GlobalController>
{
    public SceneGame currentScene;

    private void Start()
    {
        Debug.Log("Khoi tao GlobalController");
        //SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //currentScene = arg0.name;
    }
}
