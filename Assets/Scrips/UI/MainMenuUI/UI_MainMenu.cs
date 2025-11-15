using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour, ISaveManager
{
    public static UI_MainMenu instance;
    [SerializeField] private string defaultSeneName;
    private string sceneName;
    [SerializeField] private GameObject continueButton = null;
    public UI_DarkScreen fadeScreen;
    [SerializeField] private bool isHaveContinueButton = true;
    public GameObject LoadingUIGameObject;

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

    private void Start()
    {
        if(isHaveContinueButton)
        {
            if (!SaveManager.instance.HaveSaveData())
            {
                continueButton.gameObject.SetActive(false);
            }
            else
            {
                continueButton.gameObject.SetActive(true);
            }
        }
    }

    public void ContinueGame()
    {
        SceneLoadManager.instance.LoadSceneNamed(sceneName);
    }

    public void NewGame()
    {
        SaveManager.instance.NewGame();
        sceneName = defaultSeneName;

        SceneLoadManager.instance.LoadSceneNamed(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadData(GameData _data)
    {
        if(_data.currentSceneName != string.Empty)
        {
            sceneName = _data.currentSceneName;
        }
    }

    public void SaveData(ref GameData _data)
    {
        
    }
}
