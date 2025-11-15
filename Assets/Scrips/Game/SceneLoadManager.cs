using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ILoadingUI
{
    public void LoadingBegin();
    public void LoadingUpdate(float _progress);
    public void LoadingEnd();
    public bool IsAnimFinish();
}

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;
    private ILoadingUI loadingUI;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneNamed(string _sceneName)
    {
        if (UI.instance != null)
        {
            if(UI.instance.loadingUIGameObject != null)
            {
                loadingUI = UI.instance.loadingUIGameObject.GetComponent<ILoadingUI>();
                
            }
        }
        else if (UI_MainMenu.instance != null)
        {
            if (UI_MainMenu.instance.LoadingUIGameObject != null)
            {
                loadingUI = UI_MainMenu.instance.LoadingUIGameObject.GetComponent<ILoadingUI>();
            }
        }
        StartCoroutine(LoadSceneNamed_Helper(_sceneName));
        SaveManager.instance.SaveGame();
    }
    private IEnumerator LoadSceneNamed_Helper(string _sceneName)
    {
        AsyncOperation sceneLoadOp = SceneManager.LoadSceneAsync(_sceneName);
        sceneLoadOp.allowSceneActivation = false;
        loadingUI?.LoadingBegin();

        while (loadingUI != null && !loadingUI.IsAnimFinish())
        {
            loadingUI?.LoadingUpdate(sceneLoadOp.progress);
            yield return null;
        }
        sceneLoadOp.allowSceneActivation = true;

        while (!sceneLoadOp.isDone)
        {
            loadingUI?.LoadingUpdate(sceneLoadOp.progress);
            yield return null;
        }
        loadingUI?.LoadingEnd();
    }
}
