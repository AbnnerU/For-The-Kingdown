using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private LoadLevelData levelData;

    [SerializeField] private string loadingScene = "loading";

    public void LoadScene(string sceneName)
    {
        levelData.levelToLoad = sceneName;
        SceneManager.LoadScene(loadingScene);
    }
}
