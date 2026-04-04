using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public interface IInitMenuUI
    {
        public void Init(IMenuSaveManager _save, IMenuGameManager _game, IMenuAudio _audio);
    }
    internal class CMainMenu : MonoBehaviour, IInitMenuUI
    {
        [SerializeField] protected Button continueButton;
        [SerializeField] protected Button newGameButton;
        [SerializeField] protected Button exitButton;
        [SerializeField] protected Scrollbar sceneLoadScroll;

        protected IMenuSaveManager save;
        protected IMenuGameManager game;
        protected IMenuAudio audioSystem;

        protected bool isLoadNextScene;

        public void Init(IMenuSaveManager _save, IMenuGameManager _game, IMenuAudio _audio)
        {
            save = _save;
            game = _game;
            audioSystem = _audio;

            continueButton?.gameObject.SetActive(save.HasOldGame());
            continueButton?.onClick.AddListener(ContinueGame);
            continueButton?.onClick.AddListener(() => audioSystem.ButtonClick(transform, true));
            newGameButton?.onClick.AddListener(NewGame);
            newGameButton?.onClick.AddListener(() => audioSystem.ButtonClick(transform, true));
            exitButton?.onClick.AddListener(Application.Quit);
            exitButton?.onClick.AddListener(() => audioSystem.ButtonClick(transform, true));
        }

        protected void ContinueGame()
        {
            string scene = save.CheckContinueSceneName();
            game.SwitchSceneTo(scene);
            isLoadNextScene = true;
        }

        protected void NewGame()
        {
            save.NewGame();
            string scene = save.CheckContinueSceneName();
            game.SwitchSceneTo(scene);
            isLoadNextScene = true;
        }

        private void Update()
        {
            if(isLoadNextScene)
            {
                if(game != null)
                {
                    sceneLoadScroll.value = game.CheckSceneLoadRate();
                }
                else
                {
                    sceneLoadScroll.value = 1;
                }
            }
        }
    }

    public interface IMenuSaveManager
    {
        public void NewGame();
        public string CheckContinueSceneName();
        public bool HasOldGame();
    }
    public interface IMenuGameManager
    {
        public void SwitchSceneTo(string _name);
        public float CheckSceneLoadRate();
    }
    public interface IMenuAudio
    {
        public void ButtonClick(Transform button, bool _isPlay);
    }
}

