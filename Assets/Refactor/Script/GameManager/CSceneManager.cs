using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagerSystem
{
    internal class CSceneManager : CGameManagerComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            game.SwitchSceneToNotice += SceneSwitchTo;
            game.CheckCurrentSceneNameNotice += CheckCurrentSceneName;
        }

        protected void SceneSwitchTo(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }

        protected string CheckCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}

