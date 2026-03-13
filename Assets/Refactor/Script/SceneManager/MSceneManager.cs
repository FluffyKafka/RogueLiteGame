using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagerSystem
{
    internal class MSceneManager : MonoBehaviour
    {
        [SerializeField] protected string currentSceneName;
        [SerializeField] protected string nextSceneName;

        protected void LoadNextScene()
        {
            LoadSceneNamed(nextSceneName);
        }

        protected void LoadSceneNamed(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}

