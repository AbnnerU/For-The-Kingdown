using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private LoadLevelData levelData;

    private void Start()
    {      
       StartCoroutine(LoadLevelAsync());        
    }

 
    IEnumerator LoadLevelAsync()
    {
        yield return new WaitForSeconds(0.1f);
        //print(levelName);
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelData.levelToLoad);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }
    }
}
